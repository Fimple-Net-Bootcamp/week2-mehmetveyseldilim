namespace Weather.API.Utility
{
    //* To make program unit testable
    public interface IRandomGenerator
    {
        int Next(int minValue, int maxValue);
    }
}