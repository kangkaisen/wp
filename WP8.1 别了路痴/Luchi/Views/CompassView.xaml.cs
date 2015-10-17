using Luchi.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;



namespace Luchi.Views
{
   
    public sealed partial class CompassView : Page
    {
        RotateTransform rotatTransform = null;
        Compass _compass;
        public CompassView()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
             Loaded += MainPage_Loaded;
        }

         void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            _compass = Compass.GetDefault();
            if (_compass == null)
            {
                HelpShow.Show("亲,您的手机不支持指南针!");
                return;
            }
            uint v = _compass.MinimumReportInterval;
            uint Interval = 20;
       
            _compass.ReportInterval = (Interval <= v) ? v : Interval;
            _compass.ReadingChanged += _compass_ReadingChanged;
            rotatTransform = new RotateTransform();
            ge.RenderTransform = rotatTransform;

        }
      

    
     

        async void _compass_ReadingChanged(Compass sender, CompassReadingChangedEventArgs args)
        {
            var res = args.Reading;
            
            double val = res.HeadingTrueNorth ?? res.HeadingMagneticNorth;
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {
                    rotatTransform.Angle = -val;
                    Display(val);
                });
        }



    
        private void Display(double v)
        {
            string d = "";
            int ind = Convert.ToInt32(v);
            if (ind == 0 || ind == 360)
            {
                d = "正北";
            }
            else if (ind == 90)
            {
                d = "正东";
            }
            else if (ind == 180)
            {
                d = "正南";
            }
            else if (ind == 270)
            {
                d = "正西";
            }
            else if (ind > 0 && ind < 90)
            {
                d = "东北";
            }
            else if (ind > 90 && ind < 180)
            {
                d = "东南";
            }
            else if (ind > 180 && ind < 270)
            {
                d = "西南";
            }
            else if (ind > 270 && ind < 360)
            {
                d = "西北";
            }
            tbW.Text = string.Format("{0}°（{1}）", ind, d);
        }
    }
}
