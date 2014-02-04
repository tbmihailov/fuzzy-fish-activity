using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using OpenWeatherMap;
using Common.Helpers;

namespace FishActivityFuzzyCalculator.Tests
{
    [TestClass]
    public class OpenWeatherMapClient_Tests
    {
        [TestMethod]
        public void Get_Sozopol_History_Test()
        {
            string sozopolId = "726963";//from openstreetmap.org
            OpenWeatherMapClient owmClient = new OpenWeatherMapClient();

            DateTime fromDate = new DateTime(2013, 1, 1);
            DateTime toDate = new DateTime(2013, 4, 1);
            owmClient.GetHistoryDataAsync(sozopolId, fromDate, toDate,
                (a) =>
                {
                    var data = a.Data;
                    string cityId = data.city_id.ToString();

                    if (data.list != null)
                    {
                        foreach (var item in data.list)
                        {
                            foreach (var weather in item.weather)
                            {
                                Debug.WriteLine("{0}", DateHelper.FromUnixTimestamp(item.dt).ToString("yyyy-MM-dd"));
                            }
                        }
                    }
                });
        }

        public void test_get_from_2008_2013()
        {
            string sozopolId = "726963";//from openstreetmap.org
            OpenWeatherMapClient owmClient = new OpenWeatherMapClient();

            DateTime firstDate = new DateTime(2008, 1, 1);
            int pageLengthInMonths = 3;
            int pages = 4;// *6;

            List<WeatherList> allCalls = new List<WeatherList>();
            for (int i = 0; i < pages; i++)
            {
                DateTime fromDate = firstDate.AddMonths(i * pageLengthInMonths);
                DateTime toDate = firstDate.AddMonths((i + 1) * pageLengthInMonths);

                var data = owmClient.GetHistoryData(sozopolId, fromDate, toDate);

                string cityId = data.city_id.ToString();
                if (data.list != null)
                {
                    //foreach (var item in data.list)
                    //{
                    //    Debug.WriteLine("{0}", DateHelper.FromUnixTimestamp(item.dt).ToString("yyyy-MM-dd-HH-mm"));
                    //}

                    allCalls.AddRange(data.list);
                }
            }

            JsonWriter writer = new JsonTextWriter(new StreamWriter("Sozopol_Weather.json", false, Encoding.UTF8));
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(writer, allCalls);


        }
    }
}
