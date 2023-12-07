using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Weather.API.Data;
using Weather.API.Exceptions;

namespace Weather.API.Utility.Attributes 
{
    public class ValidatePlanetNameAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<WeatherDbContext>(); // Replace YourDbContext with your actual DbContext type

            // Assuming the planet name is provided as a query parameter named "planetName"
            if (context.ActionArguments.TryGetValue("planetName", out var planetNameObj) && planetNameObj is string planetName)
            {
                // Check if the planet with the given name exists in the database
                var planetExists = dbContext.WeatherObjects.Any(p => EF.Functions.Collate(p.Location, "NOCASE").Equals(planetName));

                if (!planetExists)
                {
                    throw new PlanetNotFoundException(planetName);
                }
            }

        }
    }
}