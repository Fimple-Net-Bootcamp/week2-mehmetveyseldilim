using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Weather.API.Data;
using Weather.API.DTOs.Request;
using Weather.API.DTOs.Response;
using Weather.API.Exceptions;
using Weather.API.Extensions;
using Weather.API.Models;
using Weather.API.RequestParameters;
using Weather.API.Utility.Attributes;

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

        // [HttpGet]
        // public ActionResult<IEnumerable<ReadWeatherObject>> GetAllWeatherObjects() 
        // {
        //     _logger.LogInformation("HTTP GetAllWeatherObjects");

        //     var weatherObjects = _weatherDbContext.WeatherObjects.ToList();

        //     var mappedWeatherObjects = weatherObjects.Select(x => _mapper.Map<ReadWeatherObject>(x));

        //     _logger.LogInformation("Returning a list of weather objects => {@mappedWeatherObjects}", mappedWeatherObjects);

        //     return Ok(mappedWeatherObjects);

        // }


        [HttpGet]
        [Route("{planetName}/weather-daily")]
        [ValidatePlanetName]
        public ActionResult<IEnumerable<ReadWeatherObject>> GetDailyWeatherDataForSinglePlanet(
            string planetName, 
            [FromQuery] WeatherParameters weatherParameters) 
        {
            _logger.LogInformation($"HTTP {nameof(GetDailyWeatherDataForSinglePlanet)} has been reached");

            if(!weatherParameters.ValidTemperatureRange) 
            {
                throw new MinMaxTemperatureBadRequest();
            }

            var weatherObjects = _weatherDbContext.WeatherObjects
                                .FilterWeathersByLocation(planetName)
                                .FilterWeathersByDate(weatherParameters.BeginningDate, weatherParameters.EndingDate)
                                .FilterWeathersByTemperature(weatherParameters.MinTemperature, weatherParameters.MaxTemperature)
                                .FilterWeathersByAirQuality(weatherParameters.AirQuality)
                                .Sort(weatherParameters.OrderBy!)
                                .AsQueryable(); 


            var pagedResult = PagedList<WeatherObject>.ToPagedList(weatherObjects, weatherParameters.PageNumber, weatherParameters.PageSize);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.MetaData));
            var mappedPagedResult = pagedResult.Select(x => _mapper.Map<ReadWeatherObject>(x)); 

             _logger.LogInformation("Returning a list of weather objects => {@mappedWeatherObjects}", mappedPagedResult);

            return Ok(mappedPagedResult);


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
