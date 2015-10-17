using Luchi.Common;
using Luchi.Models;
using Luchi.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Luchi.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BusinessView : Page
    {
        public BusinessView()
        {
            this.InitializeComponent();
            
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string para = e.Parameter as string;
            this.DataContext = new BusinessViewModel(para);
        }

        private async  void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BusinessModel.bizModel Item =(BusinessModel.bizModel)e.AddedItems.First() ;
            string uri = string.Format("bingmaps:?cp={0}~{1}&lvl=10&q={2}&where={3}",Item.lng,Item.lat, Item.name, App.City);
            await Windows.System.Launcher.LaunchUriAsync(new Uri(uri, UriKind.Absolute));
            
        }
    }
}
