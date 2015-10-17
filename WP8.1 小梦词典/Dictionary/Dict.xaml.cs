using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using System.Collections.ObjectModel;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace Dictionary
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Dict : Page
    {
        public Dict()
        {
            this.InitializeComponent();
            httpClient = new HttpClient();
            httpClient.MaxResponseContentBufferSize = 256000;
            httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
          
        }

     
        public enum SearchType
        {
            Enter=1,
            History=2,
        }
        string keyWord = null;
        string keyUpdate = null;
        string acceptationUpdate = null;
        int wordCount = 0;
        
        HttpClient httpClient = null;
        public static string loadKey = "D24FFB4A7F12C838F3BA7C776F9724B5";
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void txtKeywords_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Search(SearchType.Enter);
            }
        }

        async void Search(SearchType searchType)
        {
            bool isOnline = CheckNetwork();
            if(isOnline)
            {
                string temp = txtKeywords.Text.Trim();
                if(temp!=keyWord&&!string.IsNullOrEmpty(temp))
                {
                    keyWord = temp;
                    bool haveResult = await SearchWordFromAPI(keyWord);
                    if(haveResult)
                    {
                        HandleAddNewWordButton();
                        SaveHistory(keyWord);
                    }
                }
            }

        }

       

        private async void SaveHistory(string keyWord)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await localFolder.GetFileAsync(App.HistoryFileName);
            String historyWord = await FileIO.ReadTextAsync(file);
            XDocument xHistory = XDocument.Parse(historyWord);
            XElement xword = xHistory.Root;
            IEnumerable<XElement> items=xword.Elements(XName.Get("item"));
            foreach (XElement item in items)
            {
                XElement xkey = item.Element(XName.Get("key"));
                if (xkey!=null&&xkey.Value==keyWord)
                {
                    item.Remove();
                }
            }
            XElement newItem = new XElement(XName.Get("item"));

            XElement key=new XElement (XName.Get("key"));
            key.Value=keyWord;
            newItem.Add(key);

            XElement acceptation=new XElement (XName.Get("acceptation"));
            IEnumerable<TextBlock> tbs=spAcceptions.Children.OfType<TextBlock>();
            foreach (TextBlock tb in tbs)

	         {
                 acceptation.Value += tb.Text.Trim() + " ";
	         }
            newItem.Add(acceptation);

            newItem.Add(new XElement(XName.Get("time")), DateTime.Now.ToString("yyyy-MM-dd"));

            xword.AddFirst(newItem);
            items = xword.Elements(XName.Get("item"));
            if (items.Count()>1000)
            {
                items.LastOrDefault().Remove();
            }
            String historyXml = xword.ToString();
            await FileIO.WriteTextAsync(file,historyXml);
          
        }

        private  async void HandleAddNewWordButton()
        {
            bool isExist = await WordISExistInWordbookAsync(keyWord);
            if (isExist)
            {
                addAppBarButton.Label= "已添加";
                addAppBarButton.IsEnabled = false;
            }
            else
            {
                addAppBarButton.Label = "添加到生词本";
                addAppBarButton.IsEnabled = true;
            }
        }

        private  async Task<bool> WordISExistInWordbookAsync(string keyWord)
                        {
                            bool isExist = false;
                            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                            StorageFile myWordBook = await localFolder.GetFileAsync(App.WordBookFileName);
                            string fileText = await FileIO.ReadTextAsync(myWordBook);
                            XDocument xBook = XDocument.Parse(fileText);
                            XElement root = xBook.Root;
                            IEnumerable<XElement> items = root.Elements(XName.Get("item"));
                            foreach (XElement item in items)
                            {
                                XElement key = item.Element(XName.Get("key"));
                                if (key != null && key.Value == keyWord)
                                {
                                    isExist = true;
                                    break;
                                }
                            }
                            return isExist;
 
                       }
     

      

        private  async Task<bool> SearchWordFromAPI(string keyWord)
        {
            bool haveResult = false;
            string url = "http://dict-co.iciba.com/api/dictionary.php?w=" + keyWord + "&key=" +loadKey;
            XDocument xResult = null;
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                Stream responseBodyAsStream = await response.Content.ReadAsStreamAsync();
                xResult = XDocument.Load(responseBodyAsStream);
                XElement dict = null;
                if (xResult != null)
                {
                    dict = xResult.Root;
                }
                if (dict.Elements().Count() <= 1)
                {
                    txtMag.Visibility = Visibility.Visible;
                    spResult.Visibility = Visibility.Collapsed;
                    txtMag.Text = "亲：对不起！没有找到" + keyWord + "的相关词典解释";
                }
                else
                {
                    haveResult = true;
                    txtMag.Visibility = Visibility.Collapsed;
                    spResult.Visibility = Visibility.Visible;
                   
                 
                    IEnumerable<XElement> pss = dict.Elements(XName.Get("ps"));
                    if (pss.Count() == 2)
                    {
                        spPron.Visibility = Visibility.Visible;
                        List<XElement> psList = pss.ToList();
                        txtPsUK.Text = "英:" + "["+psList[0].Value+"]";
                        txtPsUs.Text = "美:" + "["+psList[1].Value+"]";
                    }
                    else if (pss.Count() == 1)
                    {
                        spPron.Visibility = Visibility.Visible;
                        XElement ps = pss.FirstOrDefault();
                        txtPsUK.Text = "[" + ps.Value + "]";
                        txtPsUs.Text = string.Empty;
                    }
                    else
                    {
                        txtPsUK.Text = string.Empty;
                        txtPsUs.Text = string.Empty;
                        spPron.Visibility = Visibility.Collapsed;
                    }

                    IEnumerable<XElement> prons = dict.Elements(XName.Get("pron"));
                    if (prons.Count() == 2)
                    {
                        List<XElement> pronlist = prons.ToList();
                        mePronUK.Source = new Uri(pronlist[0].Value);
                        mePronUs.Source = new Uri(pronlist[1].Value);
                        btnPronUK.Visibility = Visibility.Visible;
                        btnPronUs.Visibility = Visibility.Visible;
                    }
                    else if (prons.Count() == 1)
                    {
                        XElement pron = prons.FirstOrDefault();
                        mePronUK.Source = new Uri(pron.Value);
                        btnPronUK.Visibility = Visibility.Visible;
                        btnPronUs.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        btnPronUK.Visibility = Visibility.Collapsed;
                        btnPronUs.Visibility = Visibility.Collapsed;
                    }
                    IEnumerable<XElement> poss = dict.Elements(XName.Get("pos"));
                    List<string> posList = new List<string>();
                    if (poss.Count() > 0)
                    {
                        foreach (XElement pos in poss)
                        {
                            posList.Add(pos.Value);
                        }
                    }

                    IEnumerable<XElement> acceptations = dict.Elements(XName.Get("acceptation"));
                    spAcceptions.Children.Clear();
                    if (acceptations.Count() > 0)
                    {
                        int i = 0;
                        foreach (XElement acceptation in acceptations)
                        {
                            TextBlock textAcceptation = new TextBlock();
                            textAcceptation.FontSize = 20;
                            textAcceptation.TextWrapping = TextWrapping.Wrap;
                            textAcceptation.Margin = new Thickness(0);
                            textAcceptation.Text = posList[i] + acceptation.Value;
                            i++;
                            spAcceptions.Children.Add(textAcceptation);
                        }
                    }

                    IEnumerable<XElement> sents = dict.Elements(XName.Get("sent"));
                    spSends.Children.Clear();
                    if (sents.Count() > 0)
                    {
                        foreach (XElement sent in sents)
                        {
                            XElement orig = sent.Element(XName.Get("orig"));
                            TextBlock textOrig = new TextBlock();
                            textOrig.FontSize = 20;
                            textOrig.TextWrapping = TextWrapping.Wrap;
                            textOrig.Text = orig.Value;
                            spSends.Children.Add(textOrig);
                            XElement trans = sent.Element(XName.Get("trans"));
                            TextBlock textTrans = new TextBlock();
                            textTrans.FontSize = 20;
                            textTrans.TextWrapping = TextWrapping.Wrap;
                            textTrans.Text = trans.Value;
                            spSends.Children.Add(textTrans);
                        }
                    }


                }
            }
            catch
            {
                txtMag.Visibility = Visibility.Visible;
                spResult.Visibility = Visibility.Collapsed;
                txtMag.Text = "亲：网络访问失败！";
            }

            return haveResult;
        }

       bool CheckNetwork()
        {
            bool isOnline = false;
            ConnectionProfile InternetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();
            if (InternetConnectionProfile==null)

            {
                txtMag.Visibility = Visibility.Visible;
                spResult.Visibility = Visibility.Collapsed;
                txtMag.Text = "亲：断网情况下无法显示词典内容！";
                
            }
           else
            {
                isOnline = true;
            }
            return isOnline;
        }
        private void lvHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WordItem word = (WordItem)((ListView)sender).SelectedItem;
            if (word!=null)
            {
                txtKeywords.Text = word.Key;
                popupHistory.IsOpen = false;
                Search(SearchType.History);
            }
        }

        private void GetHistory_Click(object sender, RoutedEventArgs e)
        {
            if (popupHistory.IsOpen==false)
            {
                popupHistory.IsOpen = true;
           
                GetHistory();
            }
            else
            {
                popupHistory.IsOpen = false;
            
            }
        }

        private async  void GetHistory()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile wordHistory = await localFolder.GetFileAsync(App.HistoryFileName);
            string fileText = await FileIO.ReadTextAsync(wordHistory);
            XDocument xBook = XDocument.Parse(fileText);
            XElement root = xBook.Root;
            IEnumerable<XElement> items = root.Elements(XName.Get("item"));
            ObservableCollection<WordItem> historyList = new ObservableCollection<WordItem>();
            foreach  (XElement item in items)
            {
                WordItem history = new WordItem();
                XElement xkey= item.Element(XName.Get("key"));
                history.Key = xkey.Value;
                historyList.Add(history);

            }
            lvHistory.ItemsSource = historyList;

        }

       

        private void btnPronUK_Click(object sender, RoutedEventArgs e)
        {
            mePronUK.Play();
        }

        private void txtPronUs_Click(object sender, RoutedEventArgs e)
        {
            mePronUs.Play();
        }

        private async void AddNewWord_Click(object sender, RoutedEventArgs e)
        {
            string word = txtKeywords.Text.Trim();
            keyUpdate = word;
            if (!string .IsNullOrEmpty(word))
            {
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFile myWordBook = await localFolder.GetFileAsync(App.WordBookFileName);
                string fileText = await FileIO.ReadTextAsync(myWordBook);
                XDocument xBook = XDocument.Parse(fileText);
                XElement wordBook = xBook.Root;
                XElement newItem = new XElement(XName.Get("item"));
                XElement key = new XElement(XName.Get("key"));
                key.Value = word;
                newItem.Add(key);
                XElement ps = new XElement(XName.Get("ps"));
                ps.Value += txtPsUK.Text + " ";
                ps.Value += txtPsUs.Text;
                newItem.Add(ps);
                XElement acceptation = new XElement(XName.Get("acceptation"));
                IEnumerable<TextBlock> tbs = spAcceptions.Children.OfType<TextBlock>();
               
                foreach(TextBlock tb in tbs)
                {
                    acceptation.Value += tb.Text + " ";
                    
                }
                acceptationUpdate = acceptation.Value;
                newItem.Add(acceptation);
                XElement time=new XElement (XName.Get("time"));
                time.Value = DateTime.Now.ToString("yyyy-MM-dd");
                newItem.Add(time);
                wordBook.Add(newItem);
                wordCount = wordBook.Elements().Count();
                string newXml = wordBook.ToString();
                await FileIO.WriteTextAsync(myWordBook, newXml);
                UpdateTitle();
                Help.UpdateBadge(wordCount);
                addAppBarButton.Label = "已添加";
                addAppBarButton.IsEnabled = false;
                

            }
        }

        private void UpdateTitle()
        {
            XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150PeekImageAndText02);

            XmlNodeList tileTextAttributes = tileXml.GetElementsByTagName("text");
            tileTextAttributes[0].InnerText = keyUpdate;
            tileTextAttributes[1].InnerText = acceptationUpdate;

            XmlNodeList tileImageAttributes = tileXml.GetElementsByTagName("image");
            ((XmlElement)tileImageAttributes[0]).SetAttribute("src", "ms-appx:///assets/Logo.scale-240.png");

            TileNotification tileNotification = new TileNotification(tileXml);

            tileNotification.ExpirationTime = DateTimeOffset.UtcNow.AddHours(24);

            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
            
        }

      

        private void txtKeywords_GotFocus(object sender, RoutedEventArgs e)
        {
            txtKeywords.Text = string.Empty;
            txtKeywords.Focus(FocusState.Keyboard);
        }

      
    }
}
