using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace DemoEx.WPF.Converters
{
    class DiscountCostConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var value in values)
            {
                if (value == null) return null;
            }
            return (int)values[1] > 0 ? (decimal?)values[0] * (100 - (int)values[1]) / 100 : (decimal?)values[0];
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
