using RacingSimulator.Weather;

namespace RacingSimulator.Transports
{
    // Абстракция воздушного транспорта
    public abstract class AirTransport : ITransport
    {
        public string Name { get; }
        protected double Speed { get; }

        protected AirTransport(string name, double speed)
        {
            Name = name;
            Speed = speed;
        }

        protected abstract double GetAccelerationCoefficient(double distance);

        public virtual double CalculateTime(double distance, WeatherCondition weatherCondition)
        {
            double accelerationCoefficient = GetAccelerationCoefficient(distance);
            double impactCoefficient = weatherCondition.GetImpact("Air", Name); // Получаем коэффициент влияния погоды
            double effectiveSpeed = Speed * (1 - impactCoefficient); // Корректируем скорость с учетом погодных условий

            if(effectiveSpeed <= 0) return 0;
            return distance / (effectiveSpeed * accelerationCoefficient);
        }
    }

    // Конкретные воздушные транспортные средства с уникальными формулами для коэффициента ускорения
    public class MagicBroom : AirTransport
    {
        public MagicBroom() : base("Метла", 25) { }

        protected override double GetAccelerationCoefficient(double distance) 
        {
            return 1.05 + 0.01 * distance; // Линейная зависимость от дистанции
        }
    }

    public class FlyingShip : AirTransport
    {
        public FlyingShip() : base("Летучий корабль", 30) { }

        protected override double GetAccelerationCoefficient(double distance) 
        {
            return 1.2 * (1 + 0.005 * distance); // Увеличение коэффициента с расстоянием
        }
    }

    public class FlyingCarpet : AirTransport
    {
        public FlyingCarpet() : base("Ковер-самолет", 20) { }

        protected override double GetAccelerationCoefficient(double distance) 
        {
            return 1.1 + Math.Sin(distance / 50); // Синусоидальная зависимость
        }
    }

    public class Mortar : AirTransport
    {
        public Mortar() : base("Ступа Бабы Яги", 22) { }

        protected override double GetAccelerationCoefficient(double distance) 
        {
            return 1.15 * Math.Log(distance + 1); // Логарифмическая зависимость
        }
    }
}