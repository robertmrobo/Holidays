﻿using System;

namespace Holidays.Model
{
    public class PublicHoliday
    {
        public DateTime Date { get; set; }
        public string LocalName { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public bool Fixed { get; set; }
        public bool Global { get; set; }
        public int LaunchYear { get; set; }
        public string Type { get; set; }

        public override string ToString() => $"{Date:dd/MMM/yyy} {Date.Date.DayOfWeek}";
    }
}
