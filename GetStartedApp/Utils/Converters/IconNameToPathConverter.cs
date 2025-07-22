using Avalonia;
using Avalonia.Data.Converters;
using Projektanker.Icons.Avalonia;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Utils.Converters
{
    public class IconNameToPathConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string iconName)
            {
                // 获取 MaterialDesign 图标的 Geometry
                return iconName;
            }
            return AvaloniaProperty.UnsetValue;
        }
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return AvaloniaProperty.UnsetValue;
        }   
    }
}
