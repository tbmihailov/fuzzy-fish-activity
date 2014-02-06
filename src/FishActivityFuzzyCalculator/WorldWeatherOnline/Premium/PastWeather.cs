using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace premium.pastweather
{
    public class PastWeatherInput
    {
        public string query { get; set; }
        public string format { get; set; }
        public string extra { get; set; }
        public string date { get; set; }
        public string enddate { get; set; }
        public string includelocation { get; set; }
        public string callback { get; set; }

        public string tp { get; set; }
    }


    public class Request
    {
        public string query { get; set; }
        public string type { get; set; }
    }

    public class Astronomy
    {
        public string moonrise { get; set; }
        public string moonset { get; set; }
        public string sunrise { get; set; }
        public string sunset { get; set; }
    }

    public class WeatherDesc
    {
        public string value { get; set; }
    }

    public class WeatherIconUrl
    {
        public string value { get; set; }
    }

    public class Hourly
    {
        public string cloudcover { get; set; }
        public string DewPointC { get; set; }
        public string DewPointF { get; set; }
        public string FeelsLikeC { get; set; }
        public string FeelsLikeF { get; set; }
        public string HeatIndexC { get; set; }
        public string HeatIndexF { get; set; }
        public string humidity { get; set; }
        public string precipMM { get; set; }
        public string pressure { get; set; }
        public string tempC { get; set; }
        public string tempF { get; set; }
        public string time { get; set; }
        public string visibility { get; set; }
        public string weatherCode { get; set; }
        public List<WeatherDesc> weatherDesc { get; set; }
        public List<WeatherIconUrl> weatherIconUrl { get; set; }
        public string WindChillC { get; set; }
        public string WindChillF { get; set; }
        public string winddir16Point { get; set; }
        public string winddirDegree { get; set; }
        public string WindGustKmph { get; set; }
        public string WindGustMiles { get; set; }
        public string windspeedKmph { get; set; }
        public string windspeedMiles { get; set; }
    }

    public class Weather
    {
        public List<Astronomy> astronomy { get; set; }
        public string date { get; set; }
        public List<Hourly> hourly { get; set; }
        public string maxtempC { get; set; }
        public string maxtempF { get; set; }
        public string mintempC { get; set; }
        public string mintempF { get; set; }
    }

    public class Data
    {
        public List<Request> request { get; set; }
        public List<Weather> weather { get; set; }
    }

    public class PastWeather
    {
        public Data data { get; set; }
    }
}