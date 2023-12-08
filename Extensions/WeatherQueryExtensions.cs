using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Weather.API.Helpers;
using Weather.API.Models;

namespace Weather.API.Extensions
{
    public static class WeatherQueryExtensions
    {
        public static IQueryable<WeatherObject> FilterWeathersByTemperature(
            this IQueryable<WeatherObject> weathers, int minTemp, int maxTemp)
        {
            return weathers.Where(e => e.Mintemperature >= minTemp && e.Maxtemperature <= maxTemp);
        }

        public static IQueryable<WeatherObject> FilterWeathersByLocation(
            this IQueryable<WeatherObject> weathers, string location)
        {
            return weathers.Where(p => EF.Functions.Collate(p.Location, "NOCASE").Equals(location));
        }
        
        public static IQueryable<WeatherObject> Sort(this IQueryable<WeatherObject> weathers, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return weathers.OrderByDescending(e => e.Date);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<WeatherObject>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return weathers.OrderByDescending(e => e.Date);

            return weathers.OrderBy(orderQuery);
        }



        public static IQueryable<WeatherObject> FilterWeathersByAirQuality(
            this IQueryable<WeatherObject> weathers, AirQuality? airQuality)
        {
            if(airQuality is null) 
            {
                return weathers;
            }
            return weathers.Where(e => e.AirQuality == airQuality);
        }

        public static IQueryable<WeatherObject> FilterWeathersByDate(this IQueryable<WeatherObject> weathers,
        DateOnly startingDate, 
        DateOnly endingDate) 
        {
            return weathers.Where(w => w.Date <= endingDate && w.Date >= startingDate);
        }

    }
}