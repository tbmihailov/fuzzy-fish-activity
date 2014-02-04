using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorldWeatherOnline.Models;

namespace FishActivityFuzzyCalculator
{
    class Program
    {
        static void Main(string[] args)
        {

            string cityName = "Sozopol";
            DateTime firstDate = new DateTime(2008, 7, 1);
            int pageLengthInMonths = 1;
            int pages = 67;// *6;

            List<Weather> allWeather = DownloadWeather(cityName, firstDate, pageLengthInMonths, pages);

            using (var streamWriter = new StreamWriter("Sozopol_Weather.json", false, Encoding.UTF8))
            {
                JsonWriter writer = new JsonTextWriter(streamWriter);
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, allWeather);
            }


            using (JsonReader reader = new JsonTextReader(new StreamReader("Sozopol_Weather.json")))
            {
                JsonSerializer deserializer = new JsonSerializer();
                var sozopolWeather = deserializer.Deserialize<List<Weather>>(reader);
                int readWeather = sozopolWeather.Count;
            }

        }

        private static List<Weather> DownloadWeather(string cityName, DateTime firstDate, int pageLengthInMonths, int pages)
        {
            List<Weather> allWeather = new List<Weather>();
            for (int i = 0; i < pages; i++)
            {
                DateTime fromDate = firstDate.AddMonths(i * pageLengthInMonths);
                DateTime toDate = firstDate.AddMonths((i + 1) * pageLengthInMonths);

                premium.pastweather.PastWeatherInput input = new premium.pastweather.PastWeatherInput();
                input.query = cityName;
                input.date = fromDate.ToString("yyyy-MM-dd");
                input.enddate = toDate.ToString("yyyy-MM-dd");
                input.format = "JSON";
                input.tp = "24";

                // call the past weather method with input parameters

                string apiKey = "5cgg9s6n6zftynps3mqnpqkm";
                Thread.Sleep(1000);
                PremiumAPI api = new PremiumAPI(apiKey);
                var pastWeather = api.GetPastWeather(input);


                if (pastWeather.data != null
                    && pastWeather.data.weather != null)
                {
                    allWeather.AddRange(pastWeather.data.weather);
                }
            }
            return allWeather;
        }
    }
}
