using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Extensions.Logging;
using Weather.API.Extensions;

public class Program 
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAutoMapperService();
        builder.Services.AddEfSqLiteInMemoryDb();
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.ConfigureSerilogLogging(builder.Configuration);

        var app = builder.Build();
        var logger = app.Services.GetRequiredService<ILogger<Program>>(); // Use ILogger<Startup> here
        app.ConfigureExceptionHandler(logger);


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseSerilogRequestLogging();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}


