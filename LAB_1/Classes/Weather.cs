namespace RacingSimulator.Weather
{
    public class WeatherCondition
    {
        public enum WeatherType
        {
            Clear,      // Солнечно
            Rain,       // Дождь
            Snow,       // Снег
            Wind,       // Ветер
            Fog,        // Туман
            Storm,      // Шторм
            HeatWave,   // Жара
            Blizzard,   // Метель
            Overcast,    // Облачность
            Hurricane,   // Ураган
            Tornado,     // Торнадо
        }

        public WeatherType Type { get; }

        public WeatherCondition(WeatherType type)
        {
            Type = type;
        }

        public double GetImpact(string transportGroup, string transportName)
        {
            double groupImpact = (Type, transportGroup) switch
            {
                (WeatherType.Clear, "Ground") => 0.0,
                (WeatherType.Clear, "Air") => 0.0,
                (WeatherType.Rain, "Ground") => 0.2,
                (WeatherType.Rain, "Air") => 0.3, 
                (WeatherType.Snow, "Ground") => 0.5,
                (WeatherType.Snow, "Air") => 0.4,
                (WeatherType.Wind, "Ground") => 0.1,
                (WeatherType.Wind, "Air") => 0.6, 
                (WeatherType.Fog, "Ground") => 0.15,
                (WeatherType.Fog, "Air") => 0.25,
                (WeatherType.Storm, "Ground") => 0.5,
                (WeatherType.Storm, "Air") => 0.8, 
                (WeatherType.HeatWave, "Ground") => 0.1,
                (WeatherType.HeatWave, "Air") => 0.05,
                (WeatherType.Blizzard, "Ground") => 0.6,
                (WeatherType.Blizzard, "Air") => 0.4,
                (WeatherType.Overcast, "Ground") => 0.05,
                (WeatherType.Overcast, "Air") => 0.1,
                (WeatherType.Hurricane, "Ground") => 0.7, 
                (WeatherType.Hurricane, "Air") => 1.0, 
                (WeatherType.Tornado, "Ground") => 1.0, 
                (WeatherType.Tornado, "Air") => 1.0, 
                _ => 0.0
            };

            double specificImpact = (Type, transportName) switch
            {
                (WeatherType.Rain, "Кентавр") => 0.2,
                (WeatherType.Rain, "Сапоги-скороходы") => 0.3,
                (WeatherType.Snow, "Кентавр") => 0.4,
                (WeatherType.Snow, "Летучий корабль") => 0.3,
                (WeatherType.Wind, "Ковер-самолет") => -0.5,
                (WeatherType.Wind, "Ступа Бабы Яги") => 0.1, 
                (WeatherType.Storm, "Ковер-самолет") => 0.6,
                (WeatherType.Storm, "Метла") => 0.7, 
                (WeatherType.Blizzard, "Кентавр") => 0.5,
                (WeatherType.Blizzard, "Летучий корабль") => 0.3,
                (WeatherType.Hurricane, "Кентавр") => 0.8,
                (WeatherType.Hurricane, "Летучий корабль") => 0.9,
                (WeatherType.Tornado, "Кентавр") => 0.9,
                (WeatherType.Tornado, "Метла") => 0.8,
                _ => 0.0
            };

            return groupImpact + specificImpact;
        }
    }

}