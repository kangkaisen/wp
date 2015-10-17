using Luchi.Command;
using Luchi.Common;
using Luchi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Luchi.ViewModels
{
   public class BusAroundViewModel:ViewModelBase
    {
        private string  number;

        public string  Number
        {
            get { return number; }
            set
            { this.SetProperty(ref this.number, value); }
        }

        private List<BusAroundModel.statModel> stationList;

        public List<BusAroundModel.statModel> StationList
        {
            get { return stationList; }
            set
            { this.SetProperty(ref this.stationList, value); }
        }
       public DelegateCommand FindCommand { get; set; }
       
       public BusAroundViewModel()
       {
           FindCommand = new DelegateCommand(Find);
       }

       private async void Find()
       {
          
          
           
           string uri = "http://openapi.aibang.com/bus/stats_xy?alt=json&app_key=" + App.AiBangApi + "&city=" + App.City + "&lng=" + App.longitude + "&lat=" + App.latitude + "&dist=" + Number;
           
           string response = await HttpGet.Get(uri);
           
           
           try
           {
               DataContractJsonSerializer ds = new DataContractJsonSerializer(typeof(BusAroundModel));
               MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(response));
               BusAroundModel stationDemo = ds.ReadObject(ms) as BusAroundModel;
               StationList = stationDemo.stats.stat;
               
           }
           catch
           {
               Msg(string.Format("亲,周边{0}米内无公交站点.请您扩大查询范围再试下!",Number));
           }
          
       }

       private static void Msg(string msg)
       {
           HelpShow.Show(msg);
       }
    }
}
