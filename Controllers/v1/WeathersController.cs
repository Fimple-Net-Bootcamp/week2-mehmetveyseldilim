using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet("{planetName}/weather/{date}", Name = "GetWeatherDataForSpecificPlanetAndDate")]
        [ValidatePlanetName]
        public ActionResult<ReadWeatherObject> GetWeatherDataForSpecificPlanetAndDate([FromRoute]string planetName, [FromRoute]DateOnly date) 
        {
            var weatherObject = GetWeatherObject(planetName, date);

            var mappedWeatherObject = _mapper.Map<ReadWeatherObject>(weatherObject);

            return mappedWeatherObject;
        }

        [HttpPost]
        public IActionResult CreateWeatherObject([FromBody] CreateWeatherObject createWeatherObject) 
        {
            var weatherObjectToBeAdded = _mapper.Map<WeatherObject>(createWeatherObject);

            _weatherDbContext.WeatherObjects.Add(weatherObjectToBeAdded);
            _weatherDbContext.SaveChanges();

            var mappedDTO = _mapper.Map<ReadWeatherObject>(weatherObjectToBeAdded);

            string location = mappedDTO.Location;
            string date = mappedDTO.Date.ToString("yyyy.MM.dd");

            return CreatedAtRoute(nameof(GetWeatherDataForSpecificPlanetAndDate),
            new { planetName = location, date = date }, 
            mappedDTO);

        }

        [HttpPut("{planetName}/weather/{date}")]
        [ValidatePlanetName]
        public IActionResult UpdateWeatherObject(string planetName, 
        DateOnly date, 
        [FromBody] UpdateWeatherObject updateWeatherObject) 
        {
            var weatherObjectToBeUpdated = GetWeatherObject(planetName, date);

           _mapper.Map(updateWeatherObject, weatherObjectToBeUpdated);
           _weatherDbContext.SaveChanges();

            return NoContent();

        }

        [HttpDelete("{planetName}/weather/{date}")]
        [ValidatePlanetName]
        public IActionResult DeleteWeatherObject(string planetName, DateOnly date) 
        {
            var weatherObjectToBeDeleted = GetWeatherObject(planetName, date);

            _weatherDbContext.WeatherObjects.Remove(weatherObjectToBeDeleted);

            _weatherDbContext.SaveChanges();

            return NoContent();
        }


        private WeatherObject GetWeatherObject(string planetName, DateOnly date) 
        {
             var weatherObject = _weatherDbContext
                                .WeatherObjects
                                .Where(w => EF.Functions.Collate(w.Location, "NOCASE").Equals(planetName) && w.Date == date)
                                .SingleOrDefault();

            if(weatherObject is null) 
            {
                throw new WeatherNotFound($"No weather data recorded for this date: {date}");
            }

            return weatherObject;
        }
    }
}
