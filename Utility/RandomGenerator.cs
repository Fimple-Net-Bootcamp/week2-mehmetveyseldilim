namespace Weather.API.Utility
{
    public class RandomGenerator : IRandomGenerator
{
    private readonly Random random;

    public RandomGenerator()
    {
        random = new Random();
    }

    public int Next(int minValue, int maxValue)
    {
        return random.Next(minValue, maxValue);
    }
}
}