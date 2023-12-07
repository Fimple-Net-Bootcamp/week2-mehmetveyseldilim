using Microsoft.EntityFrameworkCore;
using Weather.API.Data;
using Weather.API.Utility;

namespace Weather.API.Extensions 
{
    public static class ApplicationBuilderExtensions
    {
        public static void Seed(this IApplicationBuilder applicationBuilder, 
        IHostEnvironment hostEnvironment, ILogger<Program> logger)
        {

            using (var scope = applicationBuilder.ApplicationServices.CreateScope()) 
            {
                using (var context = scope.ServiceProvider.GetService<WeatherDbContext>()) 
                {
                    var random = scope.ServiceProvider.GetService<IRandomGenerator>();

                    if(random == null) throw new ArgumentNullException(nameof(random),"The required random service is null.");
                    
                    if(context == null) 
                    {
                        logger.LogError($"{nameof(context)} is null. Seeding stops");
                        throw new ArgumentNullException(nameof(context),"The required DB service is null. Seeding could not be completed");
                    }

                
                
                    context.Database.Migrate();

                    if (hostEnvironment.IsDevelopment()) 
                    {
                        string[] planets = {"Mars", "Jupiter", "Venus"};
                        context.SeedDatabase(logger, random, planets, new DateOnly(2010,1,1));

                    }

                    context.SaveChanges();
                    


                } 
            }


        }
    }
}