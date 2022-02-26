using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace DemoEx.WPF.Converters
{
    class RemainingTimeToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan remainingTime = (TimeSpan)value;
            TimeSpan minTime = new TimeSpan(1, 0, 0);
            return remainingTime < minTime ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
