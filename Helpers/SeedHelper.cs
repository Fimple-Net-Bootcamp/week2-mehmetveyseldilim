using System.Reflection;
using Newtonsoft.Json;
using Weather.API.Models;
using Weather.API.Utility;

namespace Weather.API.Helpers
{
    public static class SeedHelper 
    {
        // Seeding data for 3 different planets 
        public static List<WeatherObject> SeedData(ILogger<Program> logger, 
        IRandomGenerator randomGenerator,
        string[] planets, 
        DateOnly startDate)
        {
            List<WeatherObject> weatherObjects = new List<WeatherObject>();

            var endDate = DateOnly.FromDateTime(DateTime.Now);

            int counter = 0;

            for (var i = 0; i < planets.Length; i++)
            {
                for (DateOnly currentDate = startDate; currentDate <= endDate; currentDate = currentDate.AddDays(1))
                {
                    // Do something with currentDate
                    var createdObject = new WeatherObject() 
                    {
                        Date = currentDate,
                        Location = planets[i],
                        Mintemperature = randomGenerator.Next(-5_000, 0),
                        Maxtemperature = randomGenerator.Next(1, 5000),
                        Pressure = randomGenerator.Next(0, 1000),
                        AirQuality = (AirQuality)randomGenerator.Next(0, 4)
                    };

                    weatherObjects.Add(createdObject);
                    counter += 1;
                }
            }

            logger.LogInformation($"{counter} weather object has been added to the database.");
            
            return weatherObjects;
        }
    }
}

