using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: May 28, 2024
 * Course: Application Development III
 * Description: Converter for converting a tab index to a boolean stating if a tab should be visible or not.
 */

namespace Growers.io.Converters
{
    /// <summary>
    /// Converter for converting a tab index to a boolean stating if a tab should be visible or not.
    /// </summary>
    class IndexToVis : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int selectedIndex && parameter is string tabIndexString && int.TryParse(tabIndexString, out int tabIndex))
            {
                return selectedIndex == tabIndex ? true : false;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
