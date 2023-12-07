using Microsoft.EntityFrameworkCore;
using Weather.API.Helpers;
using Weather.API.Models;
using Weather.API.Utility;

namespace Weather.API.Extensions
{
    public static class DbContextExtensions 
    {
        public static void SeedDatabase(this DbContext context, 
        ILogger<Program> logger, 
        IRandomGenerator randomGenerator ,
        string[] planets,
        DateOnly startingDate)
        {
            if (!context.Set<WeatherObject>().Any())
            {
                var objects = SeedHelper.SeedData(logger, randomGenerator, planets, startingDate);
                context.Set<WeatherObject>().AddRange(objects);
            }
        }
    }

}