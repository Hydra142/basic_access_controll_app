using Microsoft.UI.Xaml.Data;
using SafeMessenge.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SafeMessenge.Helpers;

internal class EnumToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is Enum enumValue)
        {
            DisplayAttribute displayAttribute = enumValue.GetType()
            .GetMember(value.ToString())
            .FirstOrDefault()
            ?.GetCustomAttribute<DisplayAttribute>();

            return displayAttribute?.GetName() ?? enumValue.ToString();
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}

