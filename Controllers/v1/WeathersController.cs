using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Weather.API.Data;
using Weather.API.DTOs.Request;
using Weather.API.DTOs.Response;
using Weather.API.Models;

namespace Weather.API.Controllers.v1 
{
    [ApiController]
    [Route("api/v1/weather")]
    public class WeathersController : ControllerBase
    {
        private readonly WeatherDbContext _weatherDbContext;
        private readonly IMapper _mapper;

        private readonly ILogger<WeathersController> _logger;  // Add logger


        public WeathersController(WeatherDbContext weatherDbContext, 
        IMapper mapper, 
        ILogger<WeathersController> logger)
        {
            _weatherDbContext = weatherDbContext;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ReadWeatherObject>> GetAllWeatherObjects() 
        {
            _logger.LogInformation("HTTP GetAllWeatherObjects");

            var weatherObjects = _weatherDbContext.WeatherObjects.ToList();

            var mappedWeatherObjects = weatherObjects.Select(x => _mapper.Map<ReadWeatherObject>(x));

            _logger.LogInformation("Returning a list of weather objects => {@mappedWeatherObjects}", mappedWeatherObjects);

            return Ok(mappedWeatherObjects);

        }

        [HttpPost]
        public ActionResult<WeatherObject> CreateWeatherObject(CreateWeatherObject createWeatherObject) 
        {
            var weatherObjectToBeAdded = _mapper.Map<WeatherObject>(createWeatherObject);

            _weatherDbContext.WeatherObjects.Add(weatherObjectToBeAdded);
            _weatherDbContext.SaveChanges();

            // return CreatedAtRoute(nameof(GetCommandById),new { Id = commandReadDto.Id }, commandReadDto);

            return weatherObjectToBeAdded;
        }
    }
}
