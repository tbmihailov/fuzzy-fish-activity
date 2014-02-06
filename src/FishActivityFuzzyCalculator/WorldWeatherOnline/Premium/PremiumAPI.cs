using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

using premium.localweather;
using premium.locationsearch;
using premium.timezone;
using premium.marineweather;
using premium.pastweather;
using RestSharp;

/// <summary>
/// Summary description for PremiumAPI
/// </summary>
public class PremiumAPI
{
    public string ApiBaseURL = "http://api.worldweatheronline.com/premium/v1/";
    public string PremiumAPIKey = "w9ve379xdu8etugm7e2ftxd6";


    public PremiumAPI()
    {

    }

    public PremiumAPI(string apiKey)
    {
        this.PremiumAPIKey = apiKey;
    }

    public LocalWeather GetLocalWeather(LocalWeatherInput input)
    {
        // create URL based on input paramters
        string apiURL = ApiBaseURL + "weather.ashx?q=" + input.query + "&format=" + input.format + "&extra=" + input.extra + "&num_of_days=" + input.num_of_days + "&date=" + input.date + "&fx=" + input.fx + "&cc=" + input.cc + "&includelocation=" + input.includelocation + "&show_comments=" + input.show_comments + "&callback=" + input.callback + "&key=" + PremiumAPIKey;

        RestClient client;
        RestRequest request;
        PrepareRequest(apiURL, out client, out request);

        var response = client.Execute<LocalWeather>(request);
        return response.Data;
    }

    public LocationSearch SearchLocation(LocationSearchInput input)
    {
        // create URL based on input paramters
        string apiURL = ApiBaseURL + "search.ashx?q=" + input.query + "&format=" + input.format + "&timezone=" + input.timezone + "&popular=" + input.popular + "&num_of_results=" + input.num_of_results + "&callback=" + input.callback + "&key=" + PremiumAPIKey;

        RestClient client;
        RestRequest request;
        PrepareRequest(apiURL, out client, out request);

        var response = client.Execute<LocationSearch>(request);
        return response.Data;
    }

    public Timezone GetTimeZone(TimeZoneInput input)
    {
        // create URL based on input paramters
        string apiURL = ApiBaseURL + "tz.ashx?q=" + input.query + "&format=" + input.format + "&callback=" + input.callback + "&key=" + PremiumAPIKey;

        RestClient client;
        RestRequest request;
        PrepareRequest(apiURL, out client, out request);

        var response = client.Execute<Timezone>(request);
        return response.Data;
    }

    public MarineWeather GetMarineWeather(MarineWeatherInput input)
    {
        // create URL based on input paramters
        string apiURL = ApiBaseURL + "marine.ashx?q=" + input.query + "&format=" + input.format + "&fx=" + input.fx + "&callback=" + input.callback + "&key=" + PremiumAPIKey;

        RestClient client;
        RestRequest request;
        PrepareRequest(apiURL, out client, out request);

        var response = client.Execute<MarineWeather>(request);
        return response.Data;
    }

    public PastWeather GetPastWeather(PastWeatherInput input)
    {
        // create URL based on input paramters
        string apiURL = ApiBaseURL + "past-weather.ashx?q=" + input.query + "&format=" + input.format + "&extra=" + input.extra + "&enddate=" + input.enddate + "&date=" + input.date + "&includelocation=" + input.includelocation + "&tp=" + input.tp + "&callback=" + input.callback + "&key=" + PremiumAPIKey;

        RestClient client;
        RestRequest request;
        PrepareRequest(apiURL, out client, out request);

        var response = client.Execute<PastWeather>(request);
        return response.Data;
    }



    private void PrepareRequest(string resourceUrl, out RestClient client, out RestRequest request)
    {
        client = new RestClient();

        request = new RestRequest(resourceUrl, Method.GET);
        //request.RequestFormat = DataFormat.Json;
    }

}