using CityTraffic.Models;
using System.Globalization;

namespace CityTraffic.Infrastructure.Converters
{
    public class ObjValuesConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values is not null && values.Length == 2)
            {
                if (values[0] is not null && values[1] is not null) 
                    return new RouteIdStoppointId {RouteId = (string)values[0], StoppointId = (int)values[1] };
            }
            return default;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
