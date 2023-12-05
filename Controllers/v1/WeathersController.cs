using Microsoft.AspNetCore.Mvc;
using Weather.API.Data;
using Weather.API.Models;

namespace Weather.API.Controllers.v1 
{
    [ApiController]
    [Route("api/v1/weather")]
    public class WeathersController : ControllerBase
    {
        private readonly WeatherDbContext _weatherDbContext;

        public WeathersController(WeatherDbContext weatherDbContext)
        {
            _weatherDbContext = weatherDbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<WeatherObject>> GetAllWeatherObjects() 
        {
            var weatherObjects = _weatherDbContext.WeatherObjects.ToList();

            return weatherObjects;
        }

        [HttpPost]
        public ActionResult<WeatherObject> CreateWeatherObject(WeatherObject weatherObject) 
        {
            _weatherDbContext.WeatherObjects.Add(weatherObject);
            _weatherDbContext.SaveChanges();

            return weatherObject;
        }
    }
}
