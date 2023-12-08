
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Weather.API.Models;
using Weather.API.Utility.Attributes;

namespace Weather.API.RequestParameters
{
    public class WeatherParameters : RequestParameters
    {
        public WeatherParameters() 
        {
            OrderBy = "date desc";
        }

        public int MinTemperature { get; set; } = int.MinValue;
        public int MaxTemperature { get; set; } = int.MaxValue;

        public DateOnly EndingDate {get; set;} = DateOnly.FromDateTime(DateTime.Now);

        public DateOnly BeginningDate {get; set;} = DateOnly.FromDateTime(DateTime.MinValue);

        public AirQuality? AirQuality {get; set;}

        [BindNever]
        public bool ValidTemperatureRange => MaxTemperature >= MinTemperature;

        // public string? SearchTerm { get; set; }

    }
}