using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace Dictionary
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class WordBook : Page
    {
        public WordBook()
        {
            this.InitializeComponent();
          
        }
        XDocument xBook = null;
        int wordCount = 0;
        ObservableCollection<WordGroup> newItems;

       
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            WordBookInit();
            
        }

        async private void WordBookInit()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await localFolder.GetFileAsync(App.WordBookFileName);
            String fileText = await FileIO.ReadTextAsync(file);
            xBook = XDocument.Parse(fileText);
            XElement xWord = xBook.Root;
            IEnumerable<XElement> items = xWord.Elements(XName.Get("item"));
            ObservableCollection<WordItem> myWordList = new ObservableCollection<WordItem>();
            foreach(XElement item in items)
            {
                WordItem words = new WordItem();
                words.Key = item.Element(XName.Get("key")).Value;
                words.Ps = item.Element(XName.Get("ps")).Value;
                words.Acception = item.Element(XName.Get("acceptation")).Value;
                words.Time = item.Element(XName.Get("time")).Value;
                myWordList.Add(words);
            }

            List<WordGroup> Items = (from item in myWordList
                                     group item by item.Time into newitems
                                     select new WordGroup { TimeTitle = newitems.Key, WordItemContent = new ObservableCollection<WordItem>(newitems) }).ToList();
           newItems = new ObservableCollection<WordGroup>(Items);

        
            CollectionViewSource ListWordSource = new CollectionViewSource();
            ListWordSource.IsSourceGrouped = true;
            ListWordSource.Source = newItems;
            ListWordSource.ItemsPath = new PropertyPath("WordItemContent");
            outView.ItemsSource = ListWordSource.View.CollectionGroups;
            inView.ItemsSource = ListWordSource.View;
           
        
        }

      

     

        private async void btnDelete_Click(object sender, RoutedEventArgs e)//删除
        {
            for (int k = 0; k < newItems.Count;k++ )
            {
                if ( newItems[k].TimeTitle == delete.Tag.ToString())
                {
                    for (int i = 0; i < newItems[k].WordItemContent.Count; i++)
                    {
                        if (newItems[k].WordItemContent[i].Key == voice.Tag.ToString())
                        {
                            newItems[k].WordItemContent.RemoveAt(i);
                            break;
                        }
                    }
                    if (newItems[k].WordItemContent.Count == 0)
                    {
                        newItems.RemoveAt(k);
                    }
                }

            }
           
            XElement words = xBook.Root;
            XElement xword = words.Elements(XName.Get("item")).SingleOrDefault(w => w.Element(XName.Get("key")).Value == voice.Tag.ToString());
            xword.Remove();
            wordCount = words.Elements().Count();
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await localFolder.GetFileAsync(App.WordBookFileName);
            await FileIO.WriteTextAsync(file, xBook.ToString());
            Help.UpdateBadge(wordCount);

        }

        private  async void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            SpeechSynthesizer speech = new SpeechSynthesizer();//实例化对象
            // 朗读文本
            SpeechSynthesisStream stream = await speech.SynthesizeTextToStreamAsync(voice.Tag.ToString());//将文本框的内容转化为语音流输出
            if (stream != null)
            {
                     mePlay.SetSource(stream, stream.ContentType);//将语音流设为MediaElement的源。
            }
           

          
       }

      

      

        private void AppBarButton_Click(object sender, RoutedEventArgs e)//播放页面
        {
            Frame.Navigate(typeof(WordPlayer));
        }

        private void Border_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);  
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)//同步页面
        {
            Frame.Navigate(typeof(OneDrive));
        }
    }
}
