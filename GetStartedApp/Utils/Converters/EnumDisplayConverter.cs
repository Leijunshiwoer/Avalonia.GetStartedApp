using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Utils.Converters
{
    internal class EnumDisplayConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value != null)
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());
                if (fi != null)
                {
                    DisplayAttribute attribute = (DisplayAttribute)fi.GetCustomAttribute(typeof(DisplayAttribute), false);
                    return attribute?.Name;
                }
            }
            return AvaloniaProperty.UnsetValue;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return AvaloniaProperty.UnsetValue;
        }
    }
}
