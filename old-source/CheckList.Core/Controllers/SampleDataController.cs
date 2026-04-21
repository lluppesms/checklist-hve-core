using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Check2.Controllers
{
    [Route("api/[controller]")]
    //[Microsoft.AspNetCore.Authorization.Authorize]
    public class SampleDataController : Controller
    {
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }

        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }

        [HttpGet("[action]")]
        public IEnumerable<ActionItem> ActionItems()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new ActionItem
            {
                id = rng.Next(1, 55),
                itemText = $"Action Item {rng.Next(1, 55)}",
                itemStatus = false
            });
        }

        public class ActionItem
        {
            public int id { get; set; }
            public string itemText { get; set; }
            public bool itemStatus { get; set; }
        }
    }
}
