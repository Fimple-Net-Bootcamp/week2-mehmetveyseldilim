using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Weather.API.Data;

namespace Weather.API.Extensions
{
    public static class ServiceCollectionExtensions 
    {
        public static void AddEfSqLiteInMemoryDb(this IServiceCollection services) 
        {
            var cnn = new SqliteConnection("DataSource=:memory:");
            cnn.Open();


            services.AddDbContext<WeatherDbContext>(options => 
            {
                options.UseSqlite(cnn);
            });
        }

        public static void AddAutoMapperService(this IServiceCollection services) 
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        
    }
}