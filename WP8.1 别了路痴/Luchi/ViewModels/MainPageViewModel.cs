using Luchi.Common;
using Luchi.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Devices.Geolocation;
using Windows.Devices.Sensors;
using Windows.Networking.Connectivity;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Luchi.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private string  city;

        public string  City
        {
            get { return city; }
            set
            { this.SetProperty(ref this.city, value); }
        }

        private string keyWord;

        public string KeyWord
        {
            get { return keyWord; }
            set
            { this.SetProperty(ref this.keyWord, value); }
        }
        
        
        private string latitude = null;
        private string longitude = null;
        Geolocator geolocator;
        private string response = null;
        public MainPageViewModel()
        {
            
            City = "正在定位...";
            Location();
            Hind();
            
        }

    

     

        
        private async void Init()
        {

            await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                //await new MessageDialog("亲,首次启动应用.请确定您已经开启定位功能.谢谢!").ShowAsync();
                bool success = await Launcher.LaunchUriAsync(new Uri("ms-settings-location:"));

            });


        }

        private async void Location()
        {
           
           try
            {
                geolocator = new Geolocator();
                geolocator.DesiredAccuracyInMeters = 50;
                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10));
                
                latitude = geoposition.Coordinate.Point.Position.Latitude.ToString();
                App.latitude = latitude;
                longitude = geoposition.Coordinate.Point.Position.Longitude.ToString();
                App.longitude = longitude;
            }
            catch (UnauthorizedAccessException)
            {
                Msg("获取位置失败!");
            }
            catch(Exception ex)
            {
                Msg("亲,麻烦您开启定位功能后重新进入本应用!"+ex.Message.ToString());
                App.InitFlag = true;
                Init();
            }
           if (CheckNetwork())
           {
               GetCity();    
           }
            
        }
        bool CheckNetwork()
        {
            bool isOnline = false;
            ConnectionProfile InternetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();
            if (InternetConnectionProfile == null)
            {
                
                Msg ("亲：断网情况下无法获知您的城市信息.请您联网后重启本应用");

            }
            else
            {
                isOnline = true;
            }
            return isOnline;
        }


        private async void GetCity()
        {
            string uri = "http://api.map.baidu.com/telematics/v3/reverseGeocoding?location=" + longitude + "," + latitude + "&output=json&ak=" +App.BaiDuApi;
            response= await HttpGet.Get(uri);
            JsonObject cityInfo= JsonObject.Parse(response);
            string cityString = cityInfo.GetNamedString("city").ToString();
            if (cityString=="")
            {
                Msg("抱歉,无法获取您的城市信息.这将导致您无法使用本应用的服务,恳请您向我反馈此信息");
                City = "未知";
                App.City = "北京";
            }
            else
            {
                int i = 0;
              
                City = cityString.Substring(0, 2);
                App.City = City;
            }
            
        }

        private async void Hind()
        {
            await Windows.UI.ViewManagement.StatusBar.GetForCurrentView().HideAsync();
           
        }
        private async void Msg(string msg)
        {
            await new MessageDialog(msg).ShowAsync();
        }
    }
}
