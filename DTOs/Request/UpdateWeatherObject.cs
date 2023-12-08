using Weather.API.Models;

namespace Weather.API.DTOs.Request
{
    public class UpdateWeatherObject
    {
        public int Mintemperature {get; set;}

        public int Maxtemperature {get; set;}

        public int Pressure {get; set;}

        public required string AirQuality {get; set;} 
    }
}