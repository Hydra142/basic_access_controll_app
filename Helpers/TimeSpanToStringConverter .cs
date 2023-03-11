using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMessenge.Helpers;

public class TimeSpanToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        TimeSpan timeSpan = (TimeSpan)value;
        return timeSpan.ToString("hh\\:mm");
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        try
        {
            string timeString = (string)value;
            TimeSpan timeSpan = TimeSpan.Parse(timeString);
            return timeSpan;
        }
        catch (Exception)
        {
            return new TimeSpan();
        }
    }
}

