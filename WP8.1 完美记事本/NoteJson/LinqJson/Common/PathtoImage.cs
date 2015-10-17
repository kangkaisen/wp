using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace LinqJson.Common
{
    public class PathtoImage : IValueConverter
    {
        BitmapImage BI;
        public  object Convert(object value, Type targetType, object parameter, string language)
        {
         
            if (value==null)
            {
                BI = new BitmapImage(new Uri("ms-appx:///Assets/1.png", UriKind.RelativeOrAbsolute));
            }
            else
            {
                BI = new BitmapImage(new Uri((string)value, UriKind.RelativeOrAbsolute));

            }
            return BI;
        }

      
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
