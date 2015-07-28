using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace View.LogMail.App.Converter
{
    class CrtSolidColorBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime? obj = value as DateTime?;
            if (null != obj && obj.HasValue)
            {
                DateTime dt = obj.Value;
                if (dt.DayOfWeek ==  DayOfWeek.Sunday)
                    return new SolidColorBrush(Color.FromArgb(0xFF, 0xFE, 0x14, 0x13));
                else if (dt.DayOfWeek == DayOfWeek.Monday)
                    return new SolidColorBrush(Color.FromArgb(0xFF, 0xFE, 0xA1, 0x15));
                else if (dt.DayOfWeek == DayOfWeek.Tuesday)
                    return new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xD0, 0x14));
                else if (dt.DayOfWeek == DayOfWeek.Wednesday)
                    return new SolidColorBrush(Color.FromArgb(0xFF, 0x71, 0xD0, 0x14));
                else if (dt.DayOfWeek == DayOfWeek.Thursday)
                    return new SolidColorBrush(Color.FromArgb(0xFF, 0x5A, 0xC4, 0xEC));
                else if (dt.DayOfWeek == DayOfWeek.Friday)
                    return new SolidColorBrush(Color.FromArgb(0xFF, 0x42, 0x14, 0xCF));
                else if (dt.DayOfWeek == DayOfWeek.Saturday)
                    return new SolidColorBrush(Color.FromArgb(0xFF, 0x9A, 0x00, 0xFF));
            }

            return new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x1E));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
