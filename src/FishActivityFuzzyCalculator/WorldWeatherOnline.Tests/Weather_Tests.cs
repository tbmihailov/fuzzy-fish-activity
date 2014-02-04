using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using premium.pastweather;
using System.Text;

namespace WorldWeatherOnline.Tests
{
    [TestClass]
    public class Weather_Tests
    {
        [TestMethod]
        public void Test_History_Api()
        {
            // set input parameters for the API
            PastWeatherInput input = new PastWeatherInput();
            input.query = "Sozopol";
            input.date = "2013-03-01";
            input.enddate = "2013-03-03";
            input.format = "JSON";
            input.tp = "24";

            // call the past weather method with input parameters
            string apiKey = "5cgg9s6n6zftynps3mqnpqkm";
            PremiumAPI api = new PremiumAPI(apiKey);
            var pastWeather = api.GetPastWeather(input);


            // printing a few values to show how to get the output
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\r\n Date: " + pastWeather.data.weather[0].date);
            sb.AppendLine("\r\n Max Temp(C): " + pastWeather.data.weather[0].maxtempC);
            sb.AppendLine("\r\n Max Temp(F): " + pastWeather.data.weather[0].maxtempF);
            sb.AppendLine("\r\n Min Temp(C): " + pastWeather.data.weather[0].mintempC);
            sb.AppendLine("\r\n Min Temp(F): " + pastWeather.data.weather[0].mintempF);
            sb.AppendLine("\r\n Cloud Cover: " + pastWeather.data.weather[0].hourly[0].cloudcover);
        }
    }
}
