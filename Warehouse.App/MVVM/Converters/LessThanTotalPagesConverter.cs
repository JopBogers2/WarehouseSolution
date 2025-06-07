﻿using System.Globalization;

namespace Warehouse.App.MVVM.Converters
{
    public class LessThanTotalPagesConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is int pageNumber && values[1] is int totalPages)
                return pageNumber < totalPages;
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
