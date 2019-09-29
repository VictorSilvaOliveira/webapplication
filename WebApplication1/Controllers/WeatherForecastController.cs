using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ITracer _tracer;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ITracer tracer)
        {
            _logger = logger;
            _tracer = tracer;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            using (IScope scope = _tracer.BuildSpan("WeatherForecast").StartActive(finishSpanOnDispose: true))
            {

                await Task.Delay(1000);

                var rng = new Random();
                return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
            }
        }
    }
}
