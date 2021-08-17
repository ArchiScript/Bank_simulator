using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem.Services
{
   public static class IntExtensions
    {

        public static TimeSpan Seconds(this int num)
        {
            return new TimeSpan(0, 0, num);
        }
        public static TimeSpan Minutes(this int num)
        {
            return new TimeSpan(0, num, 0);
        }
        public static TimeSpan Hours(this int num)
        {
            return new TimeSpan(num, 0, 0);
        }
    }
}
