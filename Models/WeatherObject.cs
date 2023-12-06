using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Weather.API.Models 
{
    [Keyless]
    public class WeatherObject
    {
        public DateOnly Date {get; set;}
        public required string Location {get; set;}
        public int Mintemperature {get; set;}

        public int Maxtemperature {get; set;}

        public int Pressure {get; set;}

        public AirQuality AirQuality {get; set;} 




    }

    public enum AirQuality 
    {
        Unknown,
        Poor,
        Medium,
        High
    }
}