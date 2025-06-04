using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace WaveApp
{
    internal class EnergyToProgressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double currentEnergy && parameter is double maxCapacity)
            {
                return currentEnergy / maxCapacity;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
