
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Weather.API.Models;
using Weather.API.Utility.Attributes;

namespace Weather.API.RequestParameters
{
    public class WeatherParameters : RequestParameters
    {
        public WeatherParameters() 
        {
            Console.WriteLine("Buraya giriyor mu merak ettim.");
            OrderBy = "date tesc";
        }

        public int MinTemperature { get; set; } = int.MinValue;
        public int MaxTemperature { get; set; } = int.MaxValue;

        public AirQuality AirQuality {get; set;}

        [BindNever]
        public bool ValidTemperatureRange => MaxTemperature >= MinTemperature;

        // public string? SearchTerm { get; set; }

    }
}