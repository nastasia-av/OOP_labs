using System;
using RacingSimulator.Races;
using RacingSimulator.Transports;
using RacingSimulator.Weather;

namespace RacingSimulator
{
    public class Program
    {
        public static void Main()
        {
            Console.Title = "Гоночный Симулятор";

            bool restart = true;
            while (restart)
            {
                try
                {
                    ShowMainMenu();

                    var raceType = SelectRaceType();

                    var distance = InputRaceDistance();

                    var weatherCondition = SelectWeatherType();

                    var race = new Race(distance, raceType);

                    RegisterTransports(race);

                    if (!ShowRaceSetup(race, weatherCondition.Type))
                    {
                        restart = true;
                        continue;
                    }

                    StartRaceAndShowResults(race, weatherCondition);

                    restart = false;
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");
                }
            }
        }

        private static void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("*****************************************************");
            Console.WriteLine("*                                                   *");
            Console.WriteLine("*        ДОБРО ПОЖАЛОВАТЬ В ГОНОЧНЫЙ СИМУЛЯТОР!     *");
            Console.WriteLine("*                                                   *");
            Console.WriteLine("*****************************************************");
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        private static RaceType SelectRaceType()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("******************************************");
                Console.WriteLine("*             ВЫБОР ТИПА ГОНКИ           *");
                Console.WriteLine("******************************************");
                Console.WriteLine("1. Наземная");
                Console.WriteLine("2. Воздушная");
                Console.WriteLine("3. Смешанная");
                Console.Write("Введите номер варианта: ");

                string? raceTypeInput = Console.ReadLine();
                RaceType raceType = raceTypeInput switch
                {
                    "1" => RaceType.Ground,
                    "2" => RaceType.Air,
                    "3" => RaceType.Mixed,
                    _ => RaceType.Mixed
                };

                Console.WriteLine($"Вы выбрали тип гонки: {raceType}");
                Console.WriteLine("1. Подтвердить выбор");
                Console.WriteLine("2. Изменить выбор");
                string? confirm = Console.ReadLine();
                if (confirm == "1") return raceType;
            }
        }

        private static double InputRaceDistance()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("******************************************");
                Console.WriteLine("*           ВВЕДИТЕ ДИСТАНЦИЮ ГОНКИ      *");
                Console.WriteLine("******************************************");
                Console.Write("Введите дистанцию гонки (в условных единицах): ");
                if (!double.TryParse(Console.ReadLine(), out double distance) || distance <= 0)
                {
                    Console.WriteLine("Неверный ввод. Попробуйте снова.");
                    Console.ReadKey();
                    continue;
                }

                Console.WriteLine($"Дистанция гонки: {distance} условных единиц");
                Console.WriteLine("1. Подтвердить выбор");
                Console.WriteLine("2. Изменить выбор");
                string? confirm = Console.ReadLine();
                if (confirm == "1") return distance;
            }
        }

        private static WeatherCondition SelectWeatherType()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("*******************************************");
                Console.WriteLine("*       ВЫБОР ТИПА ПОГОДЫ ДЛЯ ГОНКИ       *");
                Console.WriteLine("*******************************************");
                Console.WriteLine("1. Солнечная");
                Console.WriteLine("2. Дождь");
                Console.WriteLine("3. Снег");
                Console.WriteLine("4. Ветер");
                Console.WriteLine("5. Туман");
                Console.WriteLine("6. Шторм");
                Console.WriteLine("7. Жара");
                Console.WriteLine("8. Метель");
                Console.WriteLine("9. Облачность");
                Console.WriteLine("10. Ураган");
                Console.WriteLine("11. Торнадо");

                Console.Write("Введите номер варианта: ");

                string? weatherTypeInput = Console.ReadLine();
                WeatherCondition? weatherType = weatherTypeInput switch
                {
                    "1" => new WeatherCondition(WeatherCondition.WeatherType.Clear),
                    "2" => new WeatherCondition(WeatherCondition.WeatherType.Rain),
                    "3" => new WeatherCondition(WeatherCondition.WeatherType.Snow),
                    "4" => new WeatherCondition(WeatherCondition.WeatherType.Wind),
                    "5" => new WeatherCondition(WeatherCondition.WeatherType.Fog),
                    "6" => new WeatherCondition(WeatherCondition.WeatherType.Storm),
                    "7" => new WeatherCondition(WeatherCondition.WeatherType.HeatWave),
                    "8" => new WeatherCondition(WeatherCondition.WeatherType.Blizzard),
                    "9" => new WeatherCondition(WeatherCondition.WeatherType.Overcast),
                    "10" => new WeatherCondition(WeatherCondition.WeatherType.Hurricane),
                    "11" => new WeatherCondition(WeatherCondition.WeatherType.Tornado),
                    _ => new WeatherCondition(WeatherCondition.WeatherType.Clear)
                };

                Console.WriteLine($"Вы выбрали тип погоды: {weatherType.Type}");
                Console.WriteLine("1. Подтвердить выбор");
                Console.WriteLine("2. Изменить выбор");
                string? confirm = Console.ReadLine();
                if (confirm == "1") return weatherType; 
            }
        }

        private static void RegisterTransports(Race race)
        {
            bool adding = true;

            while (adding)
            {
                Console.Clear();
                Console.WriteLine("******************************************");
                Console.WriteLine("*         ВЫБЕРИТЕ ТРАНСПОРТ ДЛЯ ГОНКИ   *");
                Console.WriteLine("******************************************");
                Console.WriteLine("1. Кентавр");
                Console.WriteLine("2. Сапоги-скороходы");
                Console.WriteLine("3. Карета-тыква");
                Console.WriteLine("4. Избушка на курьих ножках");
                Console.WriteLine("5. Метла");
                Console.WriteLine("6. Летучий корабль");
                Console.WriteLine("7. Ковер-самолет");
                Console.WriteLine("8. Ступа Бабы Яги");
                Console.WriteLine("9. Завершить регистрацию");
                Console.WriteLine();

                ShowRegisteredTransports(race);

                Console.Write("Введите номер транспорта: ");

                string? transportInput = Console.ReadLine();

                ITransport? transport = transportInput switch
                {
                    "1" => new Centaur(),
                    "2" => new Boots(),
                    "3" => new Carriage(),
                    "4" => new YagaHut(),
                    "5" => new MagicBroom(),
                    "6" => new FlyingShip(),
                    "7" => new FlyingCarpet(),
                    "8" => new Mortar(),
                    "9" => null,
                    _ => null
                };

                if (transport == null)
                {
                    if (transportInput == "9")
                    {
                        adding = false;
                        continue;
                    }
                    Console.WriteLine("Неверный выбор транспорта. Попробуйте снова.");
                    Console.ReadKey();
                    continue;
                }

                if (race.IsTransportRegistered(transport))
                {
                    Console.WriteLine($"Транспорт {transport.Name} уже зарегистрирован.");
                    Console.ReadKey();
                    continue;
                }

                try
                {
                    race.RegisterTransport(transport);
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Ошибка регистрации: {ex.Message}");
                    Console.ReadKey();
                    continue;
                }
            }
        }

        private static void ShowRegisteredTransports(Race race)
        {
            Console.WriteLine("******************************************");
            if (race.GetRegisteredTransports().Any())
            {
                Console.WriteLine("Текущие транспортные средства:");
                foreach (var t in race.GetRegisteredTransports())
                {
                    Console.WriteLine($"- {t.Name}");
                }
            }
            else
            {
                Console.WriteLine("Нет зарегистрированных транспортных средств.");
            }
            Console.WriteLine("******************************************");
        }

        private static bool ShowRaceSetup(Race race, WeatherCondition.WeatherType weatherType)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("*****************************************");
                Console.WriteLine("*            ГОТОВНОСТЬ К ГОНКЕ          *");
                Console.WriteLine("*****************************************");
                Console.WriteLine($"Тип гонки: {race.RaceType}");
                Console.WriteLine($"Дистанция гонки: {race.Distance} условных единиц");
                Console.WriteLine($"Тип погоды: {weatherType}");
                Console.WriteLine("Участники:");
                foreach (var t in race.GetRegisteredTransports())
                {
                    Console.WriteLine($"- {t.Name}");
                }
                Console.WriteLine();
                Console.WriteLine("1. Начать гонку");
                Console.WriteLine("2. Выбрать заново");
                string? choice = Console.ReadLine();
                if (choice == "1") return true;
                if (choice == "2") return false;
            }
        }

        private static void StartRaceAndShowResults(Race race, WeatherCondition weatherCondition)
        {
            Console.Clear();
            Console.WriteLine("*****************************************");
            Console.WriteLine("*               РЕЗУЛЬТАТЫ ГОНКИ         *");
            Console.WriteLine("*****************************************");

            race.StartRace(weatherCondition);

            Console.WriteLine("\nНажмите любую клавишу для завершения...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}