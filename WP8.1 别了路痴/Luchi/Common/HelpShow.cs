using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Luchi.Common
{
   public static  class HelpShow
    {
       public static void Show(string text)
       {
           Myprogress.ProgressIndicator progressIndicator = new Myprogress.ProgressIndicator();
           progressIndicator.Text = text;
           progressIndicator.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x23, 0xAC, 0x5B));
           progressIndicator.Width = 400;
           progressIndicator.Height = 60;
           progressIndicator.Show();
       }
    }
}
