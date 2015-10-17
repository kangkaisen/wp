using Luchi.Common;
using Luchi.ViewModels;
using Luchi.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Store;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=391641 上有介绍

namespace Luchi
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {

        Accelerometer acc = null;
        //bool mainFlag = true;
        public MainPage()
        {
            this.InitializeComponent();

            Loaded += MainPage_Loaded;

            

            this.DataContext = new MainPageViewModel();
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

       

         void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

            acc = Accelerometer.GetDefault();
            if (acc!=null)
            {
                acc.ReportInterval = 100;
                acc.ReadingChanged += acc_ReadingChanged;
            }
            
        }


      


        private async void acc_ReadingChanged(Accelerometer sender, AccelerometerReadingChangedEventArgs args)
        {
           
                double x = args.Reading.AccelerationX * 100d;
                double y = args.Reading.AccelerationY * 100d;
                double z = args.Reading.AccelerationZ * 100d;

                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                    () =>
                    {

                        if (Math.Abs(x) > 145d || Math.Abs(y) > 140d || Math.Abs(z) > 155d)
                        {
                            if (App.appSettings.Values.ContainsKey("YaoYao"))
                            {
                                switch (App.appSettings.Values["YaoYao"].ToString())
                                {
                                    case "定位":
                                        {
                                            where();
                                            break;
                                        }
                                    case "路线导航":
                                        {
                                            navigate();
                                            break;
                                        }
                                    case "指南针":
                                        {

                                            NavigationHelp.NavigateTo(typeof(CompassView));
                                            break;
                                        }
                                    case "公交换乘":
                                        {

                                            NavigationHelp.NavigateTo(typeof(BusTransfer));
                                            break;
                                        }
                                    case "公交路线":
                                        {

                                            NavigationHelp.NavigateTo(typeof(BusRoute));
                                            break;
                                        }
                                    case "公交站点":
                                        {

                                            NavigationHelp.NavigateTo(typeof(BusStation));
                                            break;
                                        }
                                    case "周边公交":
                                        {

                                            NavigationHelp.NavigateTo(typeof(BusAround));
                                            break;
                                        }
                                    case "周边美食":
                                        {

                                            NavigationHelp.NavigateTo(typeof(BusinessView), "美食");
                                            break;

                                        }
                                    case "周边酒店":
                                        {

                                            NavigationHelp.NavigateTo(typeof(BusinessView), "酒店");
                                            break;

                                        }
                                    case "周边娱乐":
                                        {

                                            NavigationHelp.NavigateTo(typeof(BusinessView), "娱乐");
                                            break;

                                        }
                                    case "周边购物":
                                        {

                                            NavigationHelp.NavigateTo(typeof(BusinessView), "购物");
                                            break;

                                        }
                                    case "周边景点":
                                        {

                                            NavigationHelp.NavigateTo(typeof(BusinessView), "景点");
                                            break;

                                        }
                                }
                            }
                            else
                            {
                                NavigationHelp.NavigateTo(typeof(BusRoute));
                            }

                        }
                    });
           

        }
          
            
   



        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            if (App.InitFlag==true)
            {
                this.DataContext = new MainPageViewModel();
                App.InitFlag = false;
            }
          
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            acc.ReadingChanged -= acc_ReadingChanged;
            
        }
      
   

        private void where_Click(object sender, RoutedEventArgs e)
        {
            where();
        }

        private async void where()
        {
            string uri = string.Format("bingmaps:?cp={0}~{1}&lvl=20", App.longitude, App.latitude);
            await Windows.System.Launcher.LaunchUriAsync(new Uri(uri, UriKind.Absolute));
        }
        private  void navigate_Click(object sender, RoutedEventArgs e)
        {
            navigate();
        }

        private async void navigate()
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("bingmaps:?rtp=我的位置~", UriKind.Absolute));//表示查找从北京到西安的路线
        }
        private void compass_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CompassView));
        }

      

       

        private void food_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(BusinessView),"美食");
        }

        private void hotel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(BusinessView), "酒店");
        }

        private void entertain_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(BusinessView), "娱乐");
        }

        private void buy_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(BusinessView), "购物");
        }

        private void scenic_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(BusinessView), "景点");
        }

                 

        private void PhoneTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            string key=((MainPageViewModel)this.DataContext).KeyWord;

            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Frame.Navigate(typeof(BusinessView),key);
            }
        }

        private  async void feedback_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("http://www.bcmeng.com/bbs/forum.php?mod=viewthread&tid=437&extra=page%3D1", UriKind.Absolute));
        }

        private void setting_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Setting));
        }

    

        private async void appraise_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(
               new Uri("ms-windows-store:reviewapp?appid=" + CurrentApp.AppId));
        }

        private void donate_Click(object sender, RoutedEventArgs e)
        {
           
             Myprogress.ProgressIndicator progressIndicator = new Myprogress.ProgressIndicator();
             progressIndicator.Text = "本应用永久免费无广告,如果您愿意支持我的话.可以向我的支付宝捐赠:15809188214  收款人:康凯森.请注明WP用户捐赠!十分感谢!欢迎关注我的新浪微博:编程小梦";
             progressIndicator.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x23, 0xAC, 0x5B));
             progressIndicator.Width = 400;
             progressIndicator.Height = 200;
             progressIndicator.Show();
        }

       
      

      
    }
}
