using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using MoonUtils;

namespace FishActivityFuzzyCalculator.Tests
{
    [TestClass]
    public class MoonHelper_Tests
    {
        [TestMethod]
        public void Test_MoonPhase_Generation()
        {
            var someDay = new DateTime(2012, 01, 01);
            while (someDay.Month < DateTime.Now.Month + 2)
            {
                int moonPhasValue = MoonHelper.GetMoonPhase(someDay);
                MoonPhase moonPhase = (MoonPhase)moonPhasValue;
                Debug.WriteLine(string.Format("{0} - {1}", someDay.ToString("MM-dd"), moonPhase.ToMoonPhaseString()));
                someDay = someDay.AddDays(1);
            }
        }
    }
}
