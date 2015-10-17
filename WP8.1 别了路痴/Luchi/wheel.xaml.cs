using Luchi.Common;
using Luchi.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Luchi
{
    public sealed partial class wheel : UserControl
    {
        public wheel()
        {
            this.InitializeComponent();
        }

        private void circlePanel_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            rotateTransform.CenterX = circlePanel.X;
            rotateTransform.CenterY = circlePanel.Y;

            storyboard.Begin();
            e.Complete();
        }

        private async  void transfer_Click(object sender, RoutedEventArgs e)
        {
            

           Frame frame = Window.Current.Content as Frame;
           await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
           frame.Navigate(typeof(BusTransfer));
            });
           
           
        }
        private async  void route_Click(object sender, RoutedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
            await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                frame.Navigate(typeof(BusRoute));
            });
            
        }

        private async void station_Click(object sender, RoutedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
            await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                frame.Navigate(typeof(BusStation));
            });
            
            
        }

        private  async void around_Click(object sender, RoutedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
            await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                frame.Navigate(typeof(BusAround));
            });
           
        }
    }
}
