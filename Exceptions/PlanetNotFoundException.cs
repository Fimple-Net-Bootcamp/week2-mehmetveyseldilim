namespace Weather.API.Exceptions
{
    public class PlanetNotFoundException: NotFoundException
    {
        public PlanetNotFoundException(string planetName) : base($"Planet with name: '{planetName}' doesn't exist in the database")
        {
            
        }
    }
}