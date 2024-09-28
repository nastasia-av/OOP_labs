using System;
using System.Collections.Generic;
using System.Linq;
using RacingSimulator.Transports;
using RacingSimulator.Weather;

namespace RacingSimulator.Races
{
    public enum RaceType
    {
        Ground,
        Air,
        Mixed
    }

    public class Race
    {
        private readonly List<ITransport> participants = new List<ITransport>();

        public double Distance { get; }
        public RaceType RaceType { get; }

        public Race(double distance, RaceType raceType)
        {
            Distance = distance;
            RaceType = raceType;
        }

        public bool IsTransportRegistered(ITransport transport)
        {
            return participants.Any(p => p.GetType() == transport.GetType());
        }

        public void RegisterTransport(ITransport transport)
        {
            switch (RaceType)
            {
                case RaceType.Ground:
                    if (!(transport is GroundTransport))
                        throw new InvalidOperationException($"Транспорт {transport.Name} не подходит для наземной гонки.");
                    break;
                case RaceType.Air:
                    if (!(transport is AirTransport))
                        throw new InvalidOperationException($"Транспорт {transport.Name} не подходит для воздушной гонки.");
                    break;
                case RaceType.Mixed:
                    break;
                default:
                    throw new ArgumentException("Неверный тип гонки.");
            }

            // Регистрируем транспорт
            participants.Add(transport);
        }

        public IEnumerable<ITransport> GetRegisteredTransports()
        {
            return participants;
        }

        public void StartRace(WeatherCondition weatherCondition)
        {
            if (participants.Count == 0)
            {
                Console.WriteLine("Нет зарегистрированных участников.");
                return;
            }

            var results = participants
                .Select(t => new { Transport = t, Time = t.CalculateTime(Distance, weatherCondition) })
                .OrderBy(t => t.Time);

            
            foreach (var result in results)
            {
                if (result.Time <= 0)
                    Console.WriteLine($"Транспорт: {result.Transport.Name}, не удалось пройти дистанцию.");
                else
                    Console.WriteLine($"Транспорт: {result.Transport.Name}, Время: {result.Time:F2} условных единиц.");
            }

            var winner = results.FirstOrDefault(r => r.Time > 0);
            Console.WriteLine("******************************************");
    
            if (winner != null)
                Console.WriteLine($"\nПобедитель: {winner.Transport.Name} со временем {winner.Time:F2} условных единиц.");
            else
                Console.WriteLine("Никому не удалось завершить гонку");
        }
    }
}
