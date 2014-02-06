using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FishActivityFuzzyCalculator
{
    public class FuzzyCatchItem
    {
        public string CityName { get; set; }

        public VesselLength VesselLengthFuzzy { get; set; }
        

        public DateTime Date { get; set; }

        public string FishName { get; set; }

        public double OverallCatch { get; set; }

        public double AvgCatch { get; set; }

        public int DeclarationsCount { get; set; }

        public double VesselLength { get; set; }

        public Season DateSeason { get; set; }

        public string W_WeatherCode { get; set; }

        public int W_WindSpeedKmph { get; set; }

        public int W_CloudCover { get; set; }

        public int W_FeelsLikeC { get; set; }

        public double W_Humidity { get; set; }

        public int W_Pressure { get; set; }

        public int W_TempC { get; set; }

        public int W_TempCMax { get; set; }

        public int W_TempCMin { get; set; }

        public string W_WeatherDescription { get; set; }

        public MoonUtils.MoonPhase MoonPhase { get; set; }

        public int MoonPhaseInt { get; set; }

        public string MoonPhaseString { get; set; }

        public int DateMonth { get; set; }

        public CatchLevel CatchLevel { get; set; }
    }
}
