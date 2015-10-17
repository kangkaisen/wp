using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Draw
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DrawList : Page
    {
        public DrawList()
        {
            this.InitializeComponent();
        }

        List<DrawModel> ListPageDraw=null;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
         
            ObservableCollection<Note> noteList = await DataService.Current.GetNoteDataAsync();

            if (noteList!=null)
            {
                foreach (Note  item in noteList)
                {
                    if (ListPageDraw == null)
                    {
                        ListPageDraw = new List<DrawModel>();
                    }
                    else
                    {
                        ListPageDraw.Clear();
                    }
                    if (item.Draw!="")
                    {
                          string[] strArray = item.Draw.Split('#');
                          if (strArray != null && strArray.Length > 0)
                            {
                           for (int i = 0; i < strArray.Length; i++)
                             {
                            if (strArray[i] != "")
                             {
                                ListPageDraw.Add(new DrawModel(strArray[i]));
                                
                             }
                           }
                         ShowBody(ListPageDraw);
                    }
                    }
                   
                }

            }
          


        }

        private void ShowBody (List<DrawModel> ListPageDraw)
        {

        

            if ((ListPageDraw != null) && (ListPageDraw.Count > 0))
            {
                ShowDrawingsControl drawings2 = new ShowDrawingsControl
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    DataSource =ListPageDraw
                };
                ShowDrawingsControl drawings = drawings2;
                this.body.Items.Add(drawings);
            }


        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
     
    }
}
