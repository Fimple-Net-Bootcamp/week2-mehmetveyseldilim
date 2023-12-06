namespace Weather.API.DTOs.Response
{
    public class ReadWeatherObject 
    {
        public DateOnly Date {get; set;}
        public required string Location {get; set;}
        public int Mintemperature {get; set;}

        public int Maxtemperature {get; set;}

        public int Pressure {get; set;}

        public required string AirQuality {get; set;} 
    }

}