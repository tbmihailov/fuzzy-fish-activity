using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Helpers;

namespace OpenWeatherMap
{
    public class OpenWeatherMapClient
    {
        const string PARAM_AUTH_TOKEN = "AUTH_TOKEN";
        string _accessToken;
        string _apiUrl;

        public OpenWeatherMapClient(string apiUrl = "http://api.openweathermap.org/data/2.5/")
            : this(apiUrl, string.Empty)
        {

        }

        public OpenWeatherMapClient(string apiUrl, string accessToken)
        {
            _apiUrl = apiUrl;
            _accessToken = accessToken;
        }

        public void GetHistoryDataAsync(string cityId, DateTime fromDate, DateTime toDate, Action<IRestResponse<RootObject>> callback)
        {
            RestClient client;
            RestRequest request;
            PrepareGetHistoryData(cityId, fromDate, toDate, out client, out request);

            client.ExecuteAsync(request, callback);
        }

        public RootObject GetHistoryData(string cityId, DateTime fromDate, DateTime toDate)
        {
            RestClient client;
            RestRequest request;
            PrepareGetHistoryData(cityId, fromDate, toDate, out client, out request);

            var response = client.Execute<RootObject>(request);
            return response.Data;
        }

        private void PrepareGetHistoryData(string cityId, DateTime fromDate, DateTime toDate, out RestClient client, out RestRequest request)
        {
            client = new RestClient(_apiUrl);

            //"history/city?id=726963&type=day&start=1369728000&end=1369789200"
            string fromDateStr = fromDate.ToUnixTimestamp().ToString();
            string toDateStr = toDate.ToUnixTimestamp().ToString();
            string resourceUrl = string.Format("history/city?id={0}&type=day&start={1}&end={2}", cityId, fromDateStr, toDateStr);
            request = new RestRequest(resourceUrl, Method.GET);
            request.RequestFormat = DataFormat.Json;
        }
    }
}
