using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace DemoEx.WPF.Converters
{
    class StringLengthToMarginConverter : IValueConverter
    {
        private static double minTranslateDistanse = 16;
        private static double translateDistanse = 6.4;
        private Thickness translateX = new Thickness(minTranslateDistanse, 0, 0, 0);
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value.ToString().Length <= 1)
            {
                translateX.Left = minTranslateDistanse;
            }
            else
            {
                translateX.Left = minTranslateDistanse + translateDistanse * value.ToString().Length;
            }
            return translateX;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
