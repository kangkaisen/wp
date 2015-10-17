using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Xml.Linq;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechSynthesis;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;



namespace Dictionary
{
    
    public sealed partial class Translate : Page
    {
        
        
        public Translate()
        {
            this.InitializeComponent();
            //this.NavigationCacheMode = NavigationCacheMode.Required;
            
        }
        string from="auto" ;
        string to="auto";
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
           
                LoadSettings();
        }

        private async  void LoadSettings()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile settings = await localFolder.GetFileAsync(App.SettingFileName);
            String fileText = await FileIO.ReadTextAsync(settings);
            XDocument xSettings = XDocument.Parse(fileText);
            XElement root = xSettings.Root;
            XElement From = root.Element("from");
            XElement To = root.Element("to");
            if (From!=null)
            {
                from = From.Value;
            }
            if (To != null)
            {
                to = To.Value;
            }
          }
           

      

        private void translate_Click(object sender, RoutedEventArgs e)
        {
            bool isOnline = CheckNetwork();
            if (isOnline)
            {
                TranslateFromApi();
            }
        }

        bool CheckNetwork()
        {
            bool isOnline = false;
            ConnectionProfile InternetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();
            if (InternetConnectionProfile == null)
            {

                txtSrc.Text = "断网情况下无法显示词典内容！";

            }
            else
            {
                isOnline = true;
            }
            return isOnline;
        }

        void TranslateFromApi()
        {
            string client_id ="5viaBptRhtEjv0HmrB0zN0OV";
            string src = txtSrc.Text.Trim();
            txtDst.Text = "正在翻译";
           HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://openapi.baidu.com/public/2.0/bmt/translate?client_id=" + client_id + "&q=" + src + "&from=" + from + "&to=" + to);
           request.Method = "GET";
           request.Headers["Cookie"] = "name=value";
           request.BeginGetResponse(ResponseCallback, request);
        }

        private  async  void ResponseCallback(IAsyncResult result)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)result.AsyncState;
                WebResponse webResponse = httpWebRequest.EndGetResponse(result);
                using (Stream stream = webResponse.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    string content = reader.ReadToEnd();
                    JsonObject joResponse = JsonObject.Parse(content);
                    if (joResponse.ContainsKey("error_msg"))
                    {
                        txtDst.Text = joResponse["error_msg"].GetString();
                    }
                    else
                    {
                        if (joResponse.ContainsKey("trans_result"))
                        {
                            string resultend = null;
                            foreach (JsonValue item in joResponse["trans_result"].GetArray())
                            {
                                resultend += item.GetObject().GetNamedString("dst") + Environment.NewLine;
                            }
                            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,() =>
                            {
                                txtDst.Text = resultend;
                            });

                        }
                    }
                }
            }
            catch
            {
                txtDst.Text = "网络访问失败！";
            }
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            txtSrc.Text = string.Empty;
            txtSrc.Focus(FocusState.Keyboard);
        }

        private void setting_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LanguageSetting));
        }

        private void txtSrc_GotFocus(object sender, RoutedEventArgs e)
        {
            txtSrc.Text = string.Empty;
        }

        private  async void play_Click(object sender, RoutedEventArgs e)
        {   
            
            SpeechSynthesizer speech = new SpeechSynthesizer();
           
            SpeechSynthesisStream stream = await speech.SynthesizeTextToStreamAsync(txtDst.Text);
            MediaElement mePlay = new MediaElement();
            if (stream != null)
            {
                mePlay.SetSource(stream, stream.ContentType);
            }
        }
    }
}
