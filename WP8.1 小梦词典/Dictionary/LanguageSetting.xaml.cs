using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
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
    public sealed partial class LanguageSetting : Page
    {
        public LanguageSetting()
        {
            this.InitializeComponent();
        }
        Setting setting;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            List<SetttingLanguage> languages = new List<SetttingLanguage>();
            languages.Add(new SetttingLanguage { Language = "自动识别" });
            languages.Add(new SetttingLanguage { Language = "中-英" });
            languages.Add(new SetttingLanguage { Language = "中-法" });
            languages.Add(new SetttingLanguage { Language = "中-日" });
            languages.Add(new SetttingLanguage { Language = "中-韩" });
            languages.Add(new SetttingLanguage { Language = "中-德" });
            languages.Add(new SetttingLanguage { Language = "中-俄" });
            languages.Add(new SetttingLanguage { Language = "英-中" });
            languages.Add(new SetttingLanguage { Language = "法-中" });
            languages.Add(new SetttingLanguage { Language = "日-中" });
            languages.Add(new SetttingLanguage { Language = "韩-中" });
            languages.Add(new SetttingLanguage { Language = "德-中" });
            languages.Add(new SetttingLanguage { Language = "俄-中" });
            listBox.ItemsSource = languages;

            setting = new Setting();
           
        }

        public class SetttingLanguage
        {
            public string Language { get; set; }
        }

       
        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string lang = ((SetttingLanguage)listBox.SelectedItem).Language;
          
            if (lang!=null)
            {
                switch(lang)
                { 
                    case "中-英":
                {
                    setting.from = "zh"; 
                    setting.to = "en";
                    break;
                }
                    case  "英-中":
                {
                    setting.from = "en";
                    setting.to = "zh";
                    break;
                }
                    case  "中-日":
                {
                    setting.from = "zh";
                    setting.to = "jp";
                    break;
                }
                    case "日-中":
                {
                    setting.from = "jp";
                    setting.to = "zh";
                    break;
                }
                    case "中-法":
                {
                    setting.from = "zh";
                    setting.to = "fra";
                    break;
                }
                    case "中-德":
                {
                    setting.from = "zh";
                    setting.to = "de";
                     break;
                }
                    case "中-俄":
                {
                    setting.from = "zh";
                    setting.to = "ru";
                     break;
                }
                    case "中-韩":
                {
                    setting.from = "zh";
                    setting.to = "kor";
                     break;
                }
                    case "法-中":
                {
                    setting.from = "fra";
                    setting.to = "zh";
                     break;
                }
                    case "德-中":
                {
                    setting.from = "de";
                    setting.to = "zh";
                     break;
                }
                    case "俄-中":
                {
                    setting.from = "ru";
                    setting.to = "zh";
                     break;
                }
                    case "韩-中":
                {
                    setting.from = "kor";
                    setting.to = "zh";
                     break;
                }
                
                    default:
                      {
                              setting.from = "auto";
                               setting.to = "auto";
                           break;
                      }



                }
            }

            Save(setting.from, setting.to);
           
         
        }

        private async void Save(string p1, string p2)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile settings = await localFolder.GetFileAsync(App.SettingFileName);
            String fileText = await FileIO.ReadTextAsync(settings);
            XDocument xSettings = XDocument.Parse(fileText);
            XElement root = xSettings.Root;
            XElement from = root.Element("from");
            XElement to = root.Element("to");
            if (from==null)
            {
                from = new XElement("from");
                root.Add(from);
            }
            if (to==null)
            {
                to = new XElement("to");
                root.Add(to);
            }
            from.Value= p1;
            to.Value = p2;
            await FileIO.WriteTextAsync(settings, root.ToString());
            Frame.Navigate(typeof(Translate));
        }

        
     



    }
}