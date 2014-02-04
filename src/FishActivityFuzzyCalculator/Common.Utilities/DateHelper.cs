using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public static class DateHelper
    {
        public static Int32 ToUnixTimestamp(this DateTime value)
        {
            Int32 unixTimestamp = (Int32)(value.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return unixTimestamp;
        }

        public static DateTime FromUnixTimestamp(int value)
        {
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(value / 1d)).ToLocalTime();
            return dt;
        }

        

    }
}
