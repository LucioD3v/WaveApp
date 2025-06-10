using Microsoft.Maui.Graphics;
using System.Globalization;

namespace WaveApp.Converters
{
    public class ColorNameToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string colorName)
            {
                // Direct color mapping as fallback
                return colorName switch
                {
                    "PrimaryBlue" => Color.FromArgb("#2A9DF4"),
                    "AccentGreen" => Color.FromArgb("#00FF88"),
                    "WarningOrange" => Color.FromArgb("#FF6B35"),
                    _ => Colors.White
                };
            }
            return Colors.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}