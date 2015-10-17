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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=391641 上有介绍

namespace Draw
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.draw.Undo();
        }

        private void redo_Click(object sender, RoutedEventArgs e)
        {
            this.draw.Rendo();
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            this.draw.Clear();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
               //List<DrawModel> drawList = this.draw.GetListPage().ToList<DrawModel>();
               //Note note = new Note();
               //note.Title = DateTime.Now.ToString();
               //note.Draw= DataService.Current.SerializeListDrawSub(drawList);
               //DataService.Current.AddNote(note);
            this.draw.ShowBody();
            this.draw.NextPage();
 
        }

        private void previous_Click(object sender, RoutedEventArgs e)
        {
            this.draw.PreviousPage();
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            this.draw.NextPage();
        }

        //private void home_Click(object sender, RoutedEventArgs e)
        //{
        //    Frame.Navigate(typeof(DrawList));
        //}

        private void paly_Click(object sender, RoutedEventArgs e)
        {
            this.draw.palyBack();
        }
    }
}
