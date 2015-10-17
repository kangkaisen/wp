using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Draw
{
   public  class NavigationHelp
    {
        public static object Parameter; //导航传递参数
        private static Frame frame;//当前框架

        private static bool CanUseFrame()
        {
            if (frame != null)
            {
                return true;
            }
            frame = Window.Current.Content as Frame;//获取当前框架
            if (frame != null)
            {
                return true;
            }
            return false;
        }

        public static void GoBack()//返回上一个页面
        {
            if (CanUseFrame() && frame.CanGoBack)
            {
                Parameter = null;
                frame.GoBack();
            }
        }

        public static void GoBack(object para)//带参返回上一个页面
        {
            if (CanUseFrame() && frame.CanGoBack)
            {
                Parameter = para;
                frame.GoBack();
            }
        }


        public static void NavigateTo(Type uri)//导航至一个页面
        {
            if (CanUseFrame())
            {
                Parameter = null;
                frame.Navigate(uri);
            }
        }

        public static void NavigateTo(Type uri, object para)//带参导航至一个页面
        {
            if (CanUseFrame())
            {
                Parameter = para;
                frame.Navigate(uri);
            }
        }
    }
}
