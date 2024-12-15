using System;
using System.Globalization;
using System.Windows.Data;
using LAB_4.DAL.Models;

namespace LAB_4.Presentation.Utils
{
    public class PersonToDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Person person)
            {
                return $"{person.Id}. {person.LastName} {person.FirstName} {person.MiddleName}".Trim();
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
