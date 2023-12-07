namespace Weather.API.Exceptions
{
    public class PropertyNotFoundException : BadRequestException
    {
        public PropertyNotFoundException(string propertyName) : base($"Property with name '{propertyName}' was not found")
        {
            
        }
    }
}