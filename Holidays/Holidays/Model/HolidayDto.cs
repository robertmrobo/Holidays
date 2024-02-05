using System;

namespace Holidays.Model
{
    public class HolidayDto
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }

        public HolidayDto()
        {
        }

        public HolidayDto(DateTime date, string name)
        {
            Date = date;
            Name = name;
        }
        
        public override string ToString() => $"{Date:dd/MMM/yyy} {Date.Date.DayOfWeek}";
    }
}
