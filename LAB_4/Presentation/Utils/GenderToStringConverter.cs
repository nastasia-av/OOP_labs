using System;
using System.Globalization;
using System.Windows.Data;
using LAB_4.DAL.Models.Enums;

namespace LAB_4.Presentation.Utils
{
    public class GenderToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Gender gender)
            {
                switch (gender)
                {
                    case Gender.Male: return "Мужской";
                    case Gender.Female: return "Женский";
                    default: return "Неизвестно";
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value as string)
            {
                case "Мужской": return Gender.Male;
                case "Женский": return Gender.Female;
                default: return Gender.Male;
            }
        }
    }
}
