using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.Modules.SysLib.Lib
{
    public class DateClass
    {
        //get current time based on Singapore Standard Time 
        //SGT - UTC +8
        public DateTime GetCurrentDate()
        {
            DateTime _localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));
            _localTime = _localTime.Date;

            return _localTime;
        }

        public DateTime GetCurrentDateTime()
        {
            DateTime _localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));

            return _localTime;
        }

        public string[] GetMonthsList()
        {
            string[] names = DateTimeFormatInfo.CurrentInfo.MonthNames;

            return names;
        }


        public List<int> GetYearsList()
        {
            var yearToday = GetCurrentDate().Year;

            var yearList = new List<int>();

            for (var i = 0; i < 5; i++)
            {
                yearList.Add(yearToday - i);
            }

            return yearList;
        }

        public string GetMonthName(int month)
        {
            return CultureInfo.CurrentCulture.
                DateTimeFormat.GetMonthName
                (month);
        }


    }
}
