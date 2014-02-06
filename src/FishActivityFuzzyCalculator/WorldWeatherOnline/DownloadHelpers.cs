using Newtonsoft.Json;
using premium.localweather;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldWeatherOnline
{
     public class DownloadHelpers
    {
         public static LocalWeather GetLocalWeather(string location, DateTime date)
         {
             // set input parameters for the API
             LocalWeatherInput input = new LocalWeatherInput();
             input.query = location;
             input.date = date.ToString("yyyy-MM-dd");// "2014-02-06";
             input.format = "JSON";
             input.num_of_days = "5";
             input.tp = "3";

             // call the past weather method with input parameters
             string apiKey = "5cgg9s6n6zftynps3mqnpqkm";
             PremiumAPI api = new PremiumAPI(apiKey);
             var localWeather = api.GetLocalWeather(input);

             return localWeather;
         }

         private static LocalWeather LoadLocalWeatherFromFile(string jsonFileName)
         {
             LocalWeather weatherList = null;
             using (JsonReader reader = new JsonTextReader(new StreamReader(jsonFileName)))
             {
                 JsonSerializer deserializer = new JsonSerializer();
                 weatherList = deserializer.Deserialize<LocalWeather>(reader);
             }
             return weatherList;
         }

         private static void SaveLocalWeatherToFile(LocalWeather allWeather, string jsonFileName)
         {
             using (var streamWriter = new StreamWriter(jsonFileName, false, Encoding.UTF8))
             {
                 JsonWriter writer = new JsonTextWriter(streamWriter);
                 JsonSerializer serializer = new JsonSerializer();
                 serializer.Serialize(writer, allWeather);
             }
         }
    }
}
