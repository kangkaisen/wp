using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace Dictionary
{

    public sealed partial class About : Page
    {
        public About()
        {
            this.InitializeComponent();
        }

       

        private async void emailButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.ApplicationModel.Email.EmailMessage mail = new Windows.ApplicationModel.Email.EmailMessage();

            mail.Subject = "小梦词典意见反馈";
        
             mail.To.Add(new Windows.ApplicationModel.Email.EmailRecipient("kangkaisen@live.com"));
             await Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(mail);
        }

        private void bbsButton_Click(object sender, RoutedEventArgs e)
        {
            web.Width = mainGrid.ActualWidth;
            web.Height=mainGrid.ActualHeight;
            Uri targetUri = new Uri("http://www.bcmeng.com/bbs/forum.php?mod=viewthread&tid=175&extra=page%3D1");
            web.Navigate(targetUri);
            web.Visibility = Visibility.Visible;
        }

        private async  void appraiseButton_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(
                new Uri("ms-windows-store:reviewapp?appid=" + CurrentApp.AppId));
        }
    }
}
