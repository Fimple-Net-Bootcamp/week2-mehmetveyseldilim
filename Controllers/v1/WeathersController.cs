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

        public WeathersController(WeatherDbContext weatherDbContext, IMapper mapper)
        {
            _weatherDbContext = weatherDbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ReadWeatherObject>> GetAllWeatherObjects() 
        {
            var weatherObjects = _weatherDbContext.WeatherObjects.ToList();

            var mappedWeatherObjects = weatherObjects.Select(x => _mapper.Map<ReadWeatherObject>(x));

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
