using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Script.Serialization;
using free.localweather;
using free.locationsearch;
using free.timezone;
using free.marineweather;

/// <summary>
/// Summary description for FreeAPI
/// </summary>
public class FreeAPI
{
    public string ApiBaseURL = ConfigurationManager.AppSettings["FreeApiBaseURL"];
    public string FreeAPIKey = ConfigurationManager.AppSettings["FreeAPIKey"];

    public LocalWeather GetLocalWeather(LocalWeatherInput input)
    {
        // create URL based on input paramters
        string apiURL = ApiBaseURL + "weather.ashx?q=" + input.query + "&format=" + input.format + "&extra=" + input.extra + "&num_of_days=" + input.num_of_days + "&date=" + input.date + "&fx=" + input.fx + "&cc=" + input.cc + "&includelocation=" + input.includelocation + "&show_comments=" + input.show_comments + "&callback=" + input.callback + "&key="+FreeAPIKey;

        // get the web response
        string result = RequestHandler.Process(apiURL);

        // serialize the json output and parse in the helper class
        LocalWeather lWeather = (LocalWeather)new JavaScriptSerializer().Deserialize(result, typeof(LocalWeather));
        
        return lWeather;
    }

    public LocationSearch SearchLocation(LocationSearchInput input)
    {
        // create URL based on input paramters
        string apiURL = ApiBaseURL + "search.ashx?q=" + input.query + "&format=" + input.format + "&timezone=" + input.timezone + "&popular=" + input.popular + "&num_of_results=" + input.num_of_results + "&callback=" + input.callback + "&key=" + FreeAPIKey;

        // get the web response
        string result = RequestHandler.Process(apiURL);

        // serialize the json output and parse in the helper class
        LocationSearch locationSearch = (LocationSearch)new JavaScriptSerializer().Deserialize(result, typeof(LocationSearch));

        return locationSearch;
    }

    public Timezone GetTimeZone(TimeZoneInput input)
    {
        // create URL based on input paramters
        string apiURL = ApiBaseURL + "tz.ashx?q=" + input.query + "&format=" + input.format + "&callback=" + input.callback + "&key=" + FreeAPIKey;

        // get the web response
        string result = RequestHandler.Process(apiURL);

        // serialize the json output and parse in the helper class
        Timezone timeZone = (Timezone)new JavaScriptSerializer().Deserialize(result, typeof(Timezone));

        return timeZone;
    }

    public MarineWeather GetMarineWeather(MarineWeatherInput input)
    {
        // create URL based on input paramters
        string apiURL = ApiBaseURL + "marine.ashx?q=" + input.query + "&format=" + input.format + "&fx=" + input.fx + "&callback=" + input.callback + "&key=" + FreeAPIKey;

        // get the web response
        string result = RequestHandler.Process(apiURL);

        // serialize the json output and parse in the helper class
        MarineWeather mWeather = (MarineWeather)new JavaScriptSerializer().Deserialize(result, typeof(MarineWeather));

        return mWeather;
    }


}