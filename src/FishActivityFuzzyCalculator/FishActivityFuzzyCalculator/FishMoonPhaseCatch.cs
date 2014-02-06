using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FishActivityFuzzyCalculator
{
    class FishMoonPhaseCatch
    {
        public MoonUtils.MoonPhase MoonPhase { get; set; }

        public string FishName { get; set; }

        public double AvgCatch { get; set; }

        public string MoonPhaseImg { get; set; }

        public double Catch { get; set; }

        public double MinAvgCatch { get; set; }

        public double MaxAvgCatch { get; set; }

        public VesselLength VesselCategory { get; set; }
    }
}
