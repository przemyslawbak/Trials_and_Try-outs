using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Testy_07_property_call_mvvm
{

    public class BooleanStart : IValueConverter //text decoration
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Models.SendingStatus sendingStatus = (Models.SendingStatus)value;
            if (sendingStatus == Models.SendingStatus.Sending || sendingStatus == Models.SendingStatus.Waiting)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BooleanStop : IValueConverter //text decoration
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Models.SendingStatus sendingStatus = (Models.SendingStatus)value;
            if (sendingStatus == Models.SendingStatus.Sending || sendingStatus == Models.SendingStatus.Waiting)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

