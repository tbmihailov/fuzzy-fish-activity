using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FishActivityFuzzyCalculator
{
    class CatchItem
    {
        public string CityName { get; set; }

        public string FishName { get; set; }

        public double VesselLength { get; set; }

        public DateTime Date { get; set; }

        public double CatchAll { get; set; }

        public double CatchAvg { get; set; }

        public int DeclarationsCnt { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }
    }
}
