using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace premium.localweather
{
    public class LocalWeatherInput
    {
        public string query { get; set; }
        public string format { get; set; }
        public string extra { get; set; }
        public string num_of_days { get; set; }
        public string date { get; set; }
        public string fx { get; set; }
        public string cc { get; set; }
        public string includelocation { get; set; }
        public string show_comments { get; set; }
        public string tp { get; set; }
        public string callback { get; set; }
    }

    public class Month
    {
        public string absMaxTemp { get; set; }
        public string absMaxTemp_F { get; set; }
        public string avgMinTemp { get; set; }
        public string avgMinTemp_F { get; set; }
        public string index { get; set; }
        public string name { get; set; }
    }

    public class ClimateAverage
    {
        public List<Month> month { get; set; }
    }

    public class WeatherDesc
    {
        public string value { get; set; }
    }

    public class WeatherIconUrl
    {
        public string value { get; set; }
    }

    public class CurrentCondition
    {
        public string cloudcover { get; set; }
        public string FeelsLikeC { get; set; }
        public string FeelsLikeF { get; set; }
        public string humidity { get; set; }
        public string observation_time { get; set; }
        public string precipMM { get; set; }
        public string pressure { get; set; }
        public string temp_C { get; set; }
        public string temp_F { get; set; }
        public string visibility { get; set; }
        public string weatherCode { get; set; }
        public List<WeatherDesc> weatherDesc { get; set; }
        public List<WeatherIconUrl> weatherIconUrl { get; set; }
        public string winddir16Point { get; set; }
        public string winddirDegree { get; set; }
        public string windspeedKmph { get; set; }
        public string windspeedMiles { get; set; }
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

    public class WeatherDesc2
    {
        public string value { get; set; }
    }

    public class WeatherIconUrl2
    {
        public string value { get; set; }
    }

    public class Hourly
    {
        public string chanceoffog { get; set; }
        public string chanceoffrost { get; set; }
        public string chanceofhightemp { get; set; }
        public string chanceofovercast { get; set; }
        public string chanceofrain { get; set; }
        public string chanceofremdry { get; set; }
        public string chanceofsnow { get; set; }
        public string chanceofsunshine { get; set; }
        public string chanceofthunder { get; set; }
        public string chanceofwindy { get; set; }
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
        public List<WeatherDesc2> weatherDesc { get; set; }
        public List<WeatherIconUrl2> weatherIconUrl { get; set; }
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
        public List<ClimateAverage> ClimateAverages { get; set; }
        public List<CurrentCondition> current_condition { get; set; }
        public List<Request> request { get; set; }
        public List<Weather> weather { get; set; }
    }

    public class LocalWeather
    {
        public Data data { get; set; }
    }
}