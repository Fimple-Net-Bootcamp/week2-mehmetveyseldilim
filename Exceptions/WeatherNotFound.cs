namespace Weather.API.Exceptions
{
    public class WeatherNotFound : NotFoundException
    {
        public WeatherNotFound(string errorMessage) : base(errorMessage)
        {
            
        }
    }
}