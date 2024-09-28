using RacingSimulator.Weather;

namespace RacingSimulator.Transports
{
    public abstract class GroundTransport : ITransport
    {
        public string Name { get; }
        protected double Speed { get; }
        protected double TimeBeforeRest { get; }
        protected abstract double GetRestDuration(int restNumber);  

        protected GroundTransport(string name, double speed, double timeBeforeRest)
        {
            Name = name;
            Speed = speed;
            TimeBeforeRest = timeBeforeRest;
        }

        public double CalculateTime(double distance, WeatherCondition weatherCondition)
        {
            double totalTime = 0;
            double traveled = 0;
            int restCount = 0;

           double impactCoefficient = weatherCondition.GetImpact("Ground", Name);

            while (traveled < distance)
            {
                double effectiveSpeed = Speed * (1 - impactCoefficient);
                if (effectiveSpeed <= 0) return 0;

                if (traveled + effectiveSpeed * TimeBeforeRest >= distance)
                {
                    totalTime += (distance - traveled) / effectiveSpeed;
                    traveled = distance;
                }
                else
                {
                    traveled += effectiveSpeed * TimeBeforeRest;
                    totalTime += TimeBeforeRest;
                    restCount++;
                    totalTime += GetRestDuration(restCount);
                }
            }

            return totalTime;
        }

    }

    public class Centaur : GroundTransport
    {
        public Centaur() : base("Кентавр", 15, 8) { }

        protected override double GetRestDuration(int restNumber)
        {
            return 3 * Math.Sqrt(restNumber); 
        }
    }

    public class Boots : GroundTransport
    {
        public Boots() : base("Сапоги-скороходы", 20, 10) { }

        protected override double GetRestDuration(int restNumber)
        {
            return 1.5;  
        }
    }

    public class Carriage : GroundTransport
    {
        public Carriage() : base("Карета-тыква", 12, 7) { }

        protected override double GetRestDuration(int restNumber)
        {
            return Math.Pow(1.5, restNumber);  
        }
    }
    public class YagaHut : GroundTransport
    {
        public YagaHut() : base("Избушка на курьих ножках", 18, 9) { }


        protected override double GetRestDuration(int restNumber)
        {
            Random random = new Random();
            return 5 + restNumber * 3 + random.Next(1, 5);  
        }
    }
}