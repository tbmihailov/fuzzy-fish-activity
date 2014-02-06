using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorldWeatherOnline;
using MoonUtils;
using premium.pastweather;

namespace FishActivityFuzzyCalculator
{
    public enum VesselLength
    {
        From0to6m,
        From6to12m,
        From12to18m,
        From18to24m,
        Over24m,
    }

    public enum Season
    {
        Spring = 0,
        Summer = 1,
        Autumn = 2,
        Winter = 3,
    }

    public enum CatchLevel
    {
        Zero = 0,
        VeryBad = 1,
        Bad = 2,
        Good = 3,
        VeryGood = 4,
        Excellent = 5
    }



    class Program
    {
        public static int DetermineCatchLevel(double value, double bestValue, int descreteValues)
        {
            double step = bestValue / descreteValues;
            if (value <= 0.00)
            {
                return 0;
            }

            int res = (int)(Math.Floor(value / step)+1);
            if (res > descreteValues)
            {
                res = descreteValues;
            }
            return res;
        }

        static void Main(string[] args)
        {

            string cityName = "Sozopol";

            //load data
            string weatherFileName = "Res\\Sozopol_Weather.json";
            List<Weather> weatherList = LoadWeatherFromFile(weatherFileName);

            string catchFileName = "Res\\Catches-2008-2013.csv";
            List<CatchItem> catchItems = LoadCatchDataFromCsvFile(catchFileName);


            //Common fuzzy
            var fuzzyCatch = GetFuzzyCatchWithWeatherAndMoon(weatherList, catchItems);
            

            //max min by fish
            var maxMinCatchByFishAndVes = (from c in fuzzyCatch
                                           group c by new
                                           {
                                               c.FishName,
                                               c.VesselLengthFuzzy
                                           } into g
                                           select new FishMoonPhaseCatch
                                           {
                                               FishName = g.Key.FishName,
                                               VesselCategory = g.Key.VesselLengthFuzzy,
                                               Catch = g.Sum(ci => ci.OverallCatch),
                                               AvgCatch = g.Sum(ci => ci.OverallCatch) / g.Sum(ci => ci.DeclarationsCount),
                                               MaxAvgCatch = g.Max(ci => ci.AvgCatch),
                                               MinAvgCatch = g.Min(ci => ci.AvgCatch)

                                           }).ToList();
            SaveCatchDataToCsvFile<FishMoonPhaseCatch>("FishAndVes.csv", maxMinCatchByFishAndVes, true, ",");

          
            foreach (var item in fuzzyCatch)
            {
                var maxCatchByFishAndVessel = maxMinCatchByFishAndVes.FirstOrDefault(f => f.VesselCategory == item.VesselLengthFuzzy && f.FishName == item.FishName);
                if(maxCatchByFishAndVessel != null){
                    item.CatchLevel = (CatchLevel)DetermineCatchLevel(item.AvgCatch, maxCatchByFishAndVessel.AvgCatch*2, 5);
                }
            }

            SaveCatchDataToCsvFile("FuzzyCatchWithWeather.csv", fuzzyCatch, true);


            //max min by fish
            var maxMinCatchByFish = (from c in fuzzyCatch
                                     group c by new
                                     {
                                         c.FishName
                                     } into g
                                     select new FishMoonPhaseCatch
                                     {
                                         FishName = g.Key.FishName,
                                         Catch = g.Sum(ci => ci.OverallCatch),
                                         AvgCatch = g.Sum(ci => ci.OverallCatch) / g.Sum(ci => ci.DeclarationsCount),
                                         MaxAvgCatch = g.Max(ci => ci.AvgCatch),
                                         MinAvgCatch = g.Min(ci => ci.AvgCatch)

                                     }).ToList();

            SaveCatchDataToCsvFile<FishMoonPhaseCatch>("Fish.csv", maxMinCatchByFish, true, ",");


            //Fish and moon
            var catchByFishAndMoon = (from c in fuzzyCatch
                                             group c by new
                                             {
                                                 c.MoonPhase,
                                                 c.MoonPhaseString,
                                                 c.FishName
                                             } into g
                                             select new FishMoonPhaseCatch
                                             {
                                                 MoonPhase = g.Key.MoonPhase,
                                                 MoonPhaseImg = g.Key.MoonPhaseString,
                                                 FishName = g.Key.FishName,
                                                 Catch = g.Sum(ci=>ci.OverallCatch),
                                                 AvgCatch = g.Sum(ci=>ci.OverallCatch)/g.Sum(ci=>ci.DeclarationsCount),
                                                 MaxAvgCatch = g.Max(ci=>ci.AvgCatch),
                                          MinAvgCatch = g.Min(ci=>ci.AvgCatch)

                                             }).ToList();
            SaveCatchDataToCsvFile<FishMoonPhaseCatch>("FishAndMoon.csv", catchByFishAndMoon, true,",");

//by moon
            var catchByMoon = (from c in fuzzyCatch
                            group c by new
                            {
                                c.MoonPhase,
                                c.MoonPhaseString,
                            } into g
                            select new FishMoonPhaseCatch
                            {
                                MoonPhase = g.Key.MoonPhase,
                                MoonPhaseImg = g.Key.MoonPhaseString,
                                Catch = g.Sum(ci => ci.OverallCatch),
                                AvgCatch = g.Sum(ci => ci.OverallCatch) / g.Sum(ci => ci.DeclarationsCount),
                                MaxAvgCatch = g.Max(ci => ci.AvgCatch),
                                MinAvgCatch = g.Min(ci => ci.AvgCatch)
                            }).ToList();
            SaveCatchDataToCsvFile<FishMoonPhaseCatch>("Moon.csv", catchByMoon, true, ",");
        }

        private static List<FuzzyCatchItem> GetFuzzyCatchWithWeatherAndMoon(List<Weather> weatherList, List<CatchItem> catchItems)
        {
            var fuzzyCatch = catchItems.Select(ci => new FuzzyCatchItem
            {
                CityName = ci.CityName,
                //vessel
                VesselLength = ci.VesselLength,
                VesselLengthFuzzy = ToVesselLength(ci.VesselLength),

                //date
                Date = ci.Date,
                DateSeason = GetSeason(ci.Date),
                DateMonth = ci.Date.Month,

                //Weather
                FishName = ci.FishName.Replace(',', '_'),
                OverallCatch = ci.CatchAll,
                AvgCatch = ci.CatchAvg,
                DeclarationsCount = ci.DeclarationsCnt
            }).ToList();

            foreach (var fCatch in fuzzyCatch)
            {
                fCatch.MoonPhase = MoonHelper.GetMoonPhaseEnum(fCatch.Date);
                fCatch.MoonPhaseInt = MoonHelper.GetMoonPhase(fCatch.Date);
                fCatch.MoonPhaseString = fCatch.MoonPhase.ToMoonPhaseString();

                var weather = weatherList.FirstOrDefault(w => w.date == fCatch.Date.ToString("yyyy-MM-dd"));
                if (weather == null)
                {
                    continue;
                }

                fCatch.W_WeatherCode = weather.hourly[0].weatherCode;
                fCatch.W_WeatherDescription = weather.hourly[0].weatherDesc[0].value;
                fCatch.W_WindSpeedKmph = Convert.ToInt32(weather.hourly[0].windspeedKmph);
                fCatch.W_CloudCover = Convert.ToInt32(weather.hourly[0].cloudcover);
                fCatch.W_FeelsLikeC = Convert.ToInt32(weather.hourly[0].FeelsLikeC);
                fCatch.W_Humidity = Convert.ToDouble(weather.hourly[0].humidity);
                fCatch.W_Pressure = Convert.ToInt32(weather.hourly[0].pressure);
                fCatch.W_TempCMax = Convert.ToInt32(weather.maxtempC);
                fCatch.W_TempCMin = Convert.ToInt32(weather.mintempC);
                fCatch.W_TempC = Convert.ToInt32(weather.hourly[0].tempC);
            }

            return fuzzyCatch;
        }



        private static Season GetSeason(DateTime date)
        {
            //using decimal to avoid any inaccuracy issues
            decimal value = date.Month + date.Day / 100M;   // <month>.<day(2 digit)>
            if (value < 3.21m || value >= 12.22m) return Season.Winter;   // Winter
            if (value < 6.21m) return Season.Spring; // Spring
            if (value < 9.23m) return Season.Summer; // Summer
            return Season.Autumn;   // Autumn
        }


        public static VesselLength ToVesselLength(double length)
        {
            VesselLength vesselLength;
            if (length < 6)
            {
                vesselLength = VesselLength.From0to6m;
            }
            else if (length >= 6 && length < 12)
            {
                vesselLength = VesselLength.From6to12m;
            }
            else if (length >= 12 && length < 18)
            {
                vesselLength = VesselLength.From12to18m;
            }
            else if (length >= 18 && length < 24)
            {
                vesselLength = VesselLength.From18to24m;
            }
            else
            {
                vesselLength = VesselLength.Over24m;
            }

            return vesselLength;
        }



        private static void SaveCatchDataToCsvFile<T>(string catchFileName, List<T> data, bool writeHeaders, string delimiter = ",") where T:class
        {
            using (var textReader = new StreamWriter(catchFileName, false, Encoding.UTF8))
            {
                var csvConfig = new CsvConfiguration()
                {
                    Delimiter = delimiter,
                    HasHeaderRecord = true
                };

                var csv = new CsvWriter(textReader, csvConfig);
                if (writeHeaders)
                {
                    csv.WriteHeader(typeof(T));
                }

                foreach (var item in data)
                {
                    csv.WriteRecord(item);
                }
            }
        }

        private static void SaveCatchDataToCsvFile(string catchFileName, List<FuzzyCatchItem> data, bool writeHeaders, string delimiter=",")
        {
            using (var textReader = new StreamWriter(catchFileName, false, Encoding.UTF8))
            {
                var csvConfig = new CsvConfiguration()
                {
                    Delimiter = delimiter,
                    HasHeaderRecord = true
                };

                var csv = new CsvWriter(textReader, csvConfig);
                if (writeHeaders)
                {
                    csv.WriteHeader(typeof(FuzzyCatchItem));
                }

                foreach (var item in data)
                {
                    csv.WriteRecord(item);
                }
            }
        }


        private static List<CatchItem> LoadCatchDataFromCsvFile(string catchFileName)
        {
            List<CatchItem> catchesList = new List<CatchItem>();
            using (var textReader = new StreamReader(catchFileName, Encoding.UTF8))
            {
                var csvConfig = new CsvConfiguration()
                {
                    Delimiter = ";",
                    HasHeaderRecord = true
                };

                var csv = new CsvReader(textReader, csvConfig);
                while (csv.Read())
                {
                    //2008;1;Созопол;Цаца (копърка, трицона, шпрот);25.5;10.01.2008;375;187.5;2;
                    var catchItem = new CatchItem();

                    catchItem.Year = csv.GetField<int>(0);
                    catchItem.Month = csv.GetField<int>(1);
                    catchItem.CityName = csv.GetField<string>(2);
                    catchItem.FishName = csv.GetField<string>(3);
                    catchItem.VesselLength = csv.GetField<double>(4);

                    //convert string to date
                    string strDate = csv.GetField<string>(5);
                    DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                    dtfi.ShortDatePattern = "dd.MM.yyyy";
                    dtfi.DateSeparator = "-";
                    DateTime objDate = Convert.ToDateTime(strDate, dtfi);

                    catchItem.Date = objDate;
                    catchItem.CatchAll = csv.GetField<double>(6);
                    catchItem.CatchAvg = csv.GetField<double>(7);
                    catchItem.DeclarationsCnt = csv.GetField<int>(8);

                    catchesList.Add(catchItem);
                }
            }

            return catchesList;
        }





        private static List<Weather> LoadWeatherFromFile(string jsonFileName)
        {
            List<Weather> weatherList = null;
            using (JsonReader reader = new JsonTextReader(new StreamReader(jsonFileName)))
            {
                JsonSerializer deserializer = new JsonSerializer();
                weatherList = deserializer.Deserialize<List<Weather>>(reader);
            }
            return weatherList;
        }

        private static void SaveWeatherToFile(List<Weather> allWeather, string jsonFileName)
        {
            using (var streamWriter = new StreamWriter(jsonFileName, false, Encoding.UTF8))
            {
                JsonWriter writer = new JsonTextWriter(streamWriter);
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, allWeather);
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
