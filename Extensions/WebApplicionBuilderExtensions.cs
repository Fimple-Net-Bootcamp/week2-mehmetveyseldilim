using Serilog;

namespace Weather.API.Extensions
{
    public static class WebApplicationBuilderExtensions 
    {
        public static void ConfigureSerilogLogging(this WebApplicationBuilder webApplicationBuilder, IConfiguration configuration) 
        {
            // //* Adding Serilog Logger
            // var logger = new LoggerConfiguration()
            //     .ReadFrom.Configuration(configuration)
            //     .Enrich.FromLogContext()
            //     .CreateLogger();

            // webApplicationBuilder.Logging
            //     .ClearProviders()
            //     .AddSerilog(logger);

            // // webApplicationBuilder.Host.UseSerilog();

            webApplicationBuilder.Host
            .UseSerilog((context, configuration) => 
                configuration.ReadFrom.Configuration(context.Configuration));



        }
    }
}