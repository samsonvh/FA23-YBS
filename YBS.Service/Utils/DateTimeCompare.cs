using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Service.Utils
{
    public static class DateTimeCompare
    {
        public static int DateCompare (DateTime firstDate, DateTime secondDate)
        {
            DateTime firstDateCompare = new DateTime(firstDate.Year,firstDate.Month,firstDate.Day);
            DateTime secondDateCompare = new DateTime(secondDate.Year,secondDate.Month,secondDate.Day);
            int areDatesEqual = firstDate.CompareTo(secondDateCompare);
            return areDatesEqual;
        }
    }
}