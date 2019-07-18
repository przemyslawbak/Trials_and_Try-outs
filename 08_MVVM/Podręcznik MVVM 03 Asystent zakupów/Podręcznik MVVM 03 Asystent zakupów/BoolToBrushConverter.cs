using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Podręcznik_MVVM_03_Asystent_zakupów
{
    class BoolToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture) //konwersja
        {
            bool b = (bool)value;
            return b ? Brushes.Black : Brushes.Red; //czarny lub czerw
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture) //nie kumam jeszcze jaki to wyjątek
        {
            throw new NotImplementedException();
        }
    }
}
