using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Luchi.Common
{
    public class StringToVisibilityConverter:IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string s = value as string;
            Visibility vb;
            if (s=="")
            {
                vb = Visibility.Collapsed;
            }
            else
            {
                vb = Visibility.Visible;
            }
            return vb;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
