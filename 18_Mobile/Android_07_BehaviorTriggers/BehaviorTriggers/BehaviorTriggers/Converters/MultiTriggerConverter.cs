using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace BehaviorTriggers.Converters
{
    public class MultiTriggerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
        object parameter, CultureInfo culture)
        {
            if ((int)value > 0)
                return true; // Użytkownik wpisał dane
            else
                return false; // Pole jest puste
        }
        public object ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
