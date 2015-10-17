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
using Windows.UI.Popups;

namespace Luchi.ViewModels
{
   public  class BusStationViewModel:ViewModelBase
    {

        private string station;

        public string Station
        {
            get { return station; }
            set
            { this.SetProperty(ref this.station, value); }
        }

        private List<BusStationModel.statModel> stationList;

        public List<BusStationModel.statModel> StationList
        {
            get { return stationList; }
            set
            { this.SetProperty(ref this.stationList, value); }
        }
        
       public DelegateCommand FindCommand { get; set; }
       public BusStationViewModel()
       {
           FindCommand = new DelegateCommand(Find);
       }

       private async void Find()
       {
           string uri = "http://openapi.aibang.com/bus/stats?alt=json&app_key=" + App.AiBangApi + "&city=" + App.City + "&q=" + Station;
           string  response = await HttpGet.Get(uri);
           try
           {
               DataContractJsonSerializer ds = new DataContractJsonSerializer(typeof(BusStationModel));
               MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(response));
               BusStationModel stationDemo = ds.ReadObject(ms) as BusStationModel;
               StationList = stationDemo.stats.stat;
           }
           catch(InvalidCastException ex)
           {
               Msg("亲,您输入站点名称有误.");
           }
          
       }

       private static void Msg(string msg)
       {
           HelpShow.Show(msg);
       }
    }
}
