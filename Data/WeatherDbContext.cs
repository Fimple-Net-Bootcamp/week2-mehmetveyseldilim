using Microsoft.EntityFrameworkCore;
using Weather.API.Data.DatabaseModelConfigurations;
using Weather.API.Models;

namespace Weather.API.Data
{
    public class WeatherDbContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "WeatherAPIDB";
        public DbSet<WeatherObject> WeatherObjects {get; set;}

        public WeatherDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();

            // if(Database.IsRelational())
            // {
            //     Console.WriteLine("Its relational");
            // }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new WeatherObjectEntitySchemaDefinition());


            base.OnModelCreating(modelBuilder);
        }
    }
}