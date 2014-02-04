using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoonUtils
{
    public enum MoonPhase
    {
        FULL = 4,
        WAXING_GIBBOUS = 5,
        FIRST_QUARTER = 6,
        WAXING_CRESCENT = 7,
        NEW = 0,
        WANING_CRESCENT = 1,
        THIRD_QUARTER = 2,
        WANING_GIBBOUS = 3
    };

    public static class MoonHelper
    {
        public static int GetMoonPhase(DateTime value)
        {
            int moonPhase = GetMoonPhase(value.Year, value.Month, value.Day);
            return moonPhase;
        }

       

        public static string ToMoonPhaseString(this MoonPhase phase)
        {
            string value = string.Empty;
            switch (phase)
            {
                case MoonPhase.FULL:
                    value = "(****)";
                    break;
                case MoonPhase.WAXING_GIBBOUS:
                    value = "( ***)";
                    break;
                case MoonPhase.FIRST_QUARTER:
                    value = "(  **)";
                    break;
                case MoonPhase.WAXING_CRESCENT:
                    value = "(   *)";
                    break;
                case MoonPhase.NEW:
                    value = "(    )";
                    break;
                case MoonPhase.WANING_CRESCENT:
                    value = "(*   )";
                    break;
                case MoonPhase.THIRD_QUARTER:
                    value = "(**  )";
                    break;
                case MoonPhase.WANING_GIBBOUS:
                    value = "(*** )";
                    break;
                default:
                    break;
            }
            return value;
        }

        /// <summary>
        /// calculates the moon phase (0-7), accurate to 1 segment.
        ///   0 = > new moon.
        ///   4 => full moon.
        /// </summary>
        /// <param name="y"></param>
        /// <param name="m"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static int GetMoonPhase(int y, int m, int d)
        {


            int c, e;
            double jd;
            int b;

            if (m < 3)
            {
                y--;
                m += 12;
            }
            ++m;
            c = (int)Math.Floor(365.25 * y);
            e = (int)Math.Floor(30.6 * m);
            jd = c + e + d - 694039.09;  /* jd is total days elapsed */
            jd /= 29.53;           /* divide by the moon cycle (29.53 days) */
            b = (int)jd;		   /* int(jd) -> b, take integer part of jd */
            jd -= b;		   /* subtract integer part to leave fractional part of original jd */
            b = (int)(jd * 8 + 0.5);	   /* scale fraction from 0-8 and round by adding 0.5 */
            b = b & 7;		   /* 0 and 8 are the same so turn 8 into 0 */
            return b;
        }
    }
}
