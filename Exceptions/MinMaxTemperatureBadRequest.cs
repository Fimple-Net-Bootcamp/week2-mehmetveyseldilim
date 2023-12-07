namespace Weather.API.Exceptions
{
    public sealed class MinMaxTemperatureBadRequest : BadRequestException
    {
        public MinMaxTemperatureBadRequest() 
        : base("Max temperature can't be less than min temperature.")
        {

        }

    }
}