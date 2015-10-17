using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class WordPlayer : Page
    {
        XDocument xWordbook;
        ObservableCollection<WordItem> myWordList;
        DispatcherTimer _timer;
        int wordCount = 0;
       
        public WordPlayer()
        {
            this.InitializeComponent();
            WordPlayerInit();
        }

       

        private async  void WordPlayerInit()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile mtWordBook = await localFolder.GetFileAsync(App.WordBookFileName);
            String fileText = await FileIO.ReadTextAsync(mtWordBook);
            xWordbook = XDocument.Parse(fileText);
            XElement xWord = xWordbook.Root;
            IEnumerable<XElement> items = xWord.Elements(XName.Get("item"));
            myWordList = new ObservableCollection<WordItem>();
            foreach (XElement item in items)
            {
                WordItem words = new WordItem();
                words.Key = item.Element(XName.Get("key")).Value;
                words.Ps = item.Element(XName.Get("ps")).Value;
                words.Acception = item.Element(XName.Get("acceptation")).Value;
                myWordList.Add(words);
            }
            flipView.ItemsSource = myWordList;
           
          
        
        }

        private  async void AppBarButton_Click(object sender, RoutedEventArgs e)//删除
        {
            var currentWord = (WordItem)flipView.SelectedItem;
            foreach (var word in myWordList)
            {
                if (word.Key == currentWord.Key)
                {
                    myWordList.Remove(word);
                    break;
                }
            }
            
            XElement words = xWordbook.Root;
            XElement xword = words.Elements(XName.Get("item")).SingleOrDefault(w => w.Element(XName.Get("key")).Value == currentWord.Key);
            xword.Remove();
            wordCount = words.Elements().Count();
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await localFolder.GetFileAsync(App.WordBookFileName);
            await FileIO.WriteTextAsync(file, xWordbook.ToString());
            Help.UpdateBadge(wordCount);
 }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)//自动播放
        {
             _timer = new DispatcherTimer();//定义一个定时器
           _timer.Interval = TimeSpan.FromSeconds(2.0);
           _timer.Tick += ((sender1, et) =>//flipview控件当前选定项的索引不断循环
                      {
             if (flipView.SelectedIndex < flipView.Items.Count-1)
                    flipView.SelectedIndex++;
             else
                    flipView.SelectedIndex = 0;
                      });
           _timer.Start();
        }

      

        private void AppBarButton_Click_3(object sender, RoutedEventArgs e)//暂停
        {
            _timer.Stop();
        }

        private  async void AppBarButton_Click_2(object sender, RoutedEventArgs e)//语音
        {
            SpeechSynthesizer speech = new SpeechSynthesizer();//实例化对象
            // 朗读文本
            var currentWord = (WordItem)flipView.SelectedItem;
            SpeechSynthesisStream stream = await speech.SynthesizeTextToStreamAsync(currentWord.Key);//将文本框的内容转化为语音流输出
            if (stream != null)
            {
                mePlay.SetSource(stream, stream.ContentType);//将语音流设为MediaElement的源。
            }
           
        }

    }
}
