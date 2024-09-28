using RacingSimulator.Weather;

namespace RacingSimulator.Transports
{
    public interface ITransport
    {
        string Name { get; }
        double CalculateTime(double distance, WeatherCondition weatherCondition);  // Метод для расчета времени на заданную дистанцию
    }
}
