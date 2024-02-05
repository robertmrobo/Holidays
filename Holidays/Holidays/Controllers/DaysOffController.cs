using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Holidays.Data;
using Holidays.Interfaces;
using Holidays.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Holidays.Controllers;

[Route("api/[controller]")]
public class DaysOffController : Controller
{
    private readonly IPublicHolidayService _publicHolidayService;
    private readonly HolidayContext _holidayContext;

    public DaysOffController(IPublicHolidayService publicHolidayService, HolidayContext holidayContext)
    {
        _publicHolidayService = publicHolidayService;
        _holidayContext = holidayContext;
    }

    [HttpGet("{year:int}")]
    public async Task<ActionResult<IEnumerable<HolidayDto>>> GetDaysOff(int year)
    {
        var publicHolidays = await _publicHolidayService.GetByYearAsync(year);
        var workHolidays = await _holidayContext.Holidays.ToListAsync();

        // Adjust work holidays to the year requested
        // This is necessary because the work holidays in seed data are stored with a year of 1992
        foreach (var workHoliday in workHolidays)
        {
            workHoliday.Date = workHoliday.Date.AddYears(year - workHoliday.Date.Year);
        }
        
        var holidays = new List<HolidayDto>();

        // Add all public holidays to the list
        holidays.AddRange(publicHolidays.Select(ph => new HolidayDto(ph.Date, ph.Name)));

        // Process work holidays to adjust for weekends and public holidays
        foreach (var workHoliday in workHolidays)
        {
            var nextAvailableDay = workHoliday.Date;
            // Check if the work holiday falls on a weekend or a public holiday and adjust accordingly
            while (nextAvailableDay.DayOfWeek == DayOfWeek.Saturday || 
                   nextAvailableDay.DayOfWeek == DayOfWeek.Sunday || 
                   publicHolidays.Any(ph => ph.Date == nextAvailableDay))
            {
                nextAvailableDay = nextAvailableDay.AddDays(1);
            }
            
            // Add the work holiday to the list if not already added
            var all = holidays.All(h => h.Date != nextAvailableDay);

            if (all)
            {
                holidays.Add(new HolidayDto(nextAvailableDay, workHoliday.Name));
            }
        }
        
        return Ok(holidays.OrderBy(h => h.Date));
    }
}
