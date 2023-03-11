using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMessenge.Helpers;

public class DateTimeToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        DateTime timeSpan = (DateTime)value;
        return timeSpan.ToString("HH\\:mm");
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        try
        {
            string timeString = (string)value;
            DateTime timeSpan = DateTime.Parse(timeString);
            return timeSpan;
        }
        catch (Exception)
        {
            return new DateTime();
        }
    }
}

