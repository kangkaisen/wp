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
    public class BusRouteViewModel:ViewModelBase
    {
        private string  route;

        public string  Route
        {
            get { return route; }
            set
            { this.SetProperty(ref this.route, value); }
        }

        private List<BusRouteModel.lineModel> lineList;

        public List<BusRouteModel.lineModel> LineList
        {
            get { return lineList; }
            set
            { this.SetProperty(ref this.lineList, value); }
        }
        
        
        public DelegateCommand FindCommand { get; set; }
        public BusRouteViewModel()
        {
            FindCommand = new DelegateCommand(Find);
            LineList = new List<BusRouteModel.lineModel>();
        }

        private async void Find()
        {
            string uri = "http://openapi.aibang.com/bus/lines?alt=json&app_key=" + App.AiBangApi + "&city=" + App.City + "&q=" + Route;
            string response = await HttpGet.Get(uri);
            try
            {
                DataContractJsonSerializer ds = new DataContractJsonSerializer(typeof(BusRouteModel));
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(response));
                BusRouteModel line = ds.ReadObject(ms) as BusRouteModel;
                LineList = line.lines.line;
            }
            catch
            {
                Msg("亲,您输入线路名称可能有误.请您确定后再次尝试");
            }
            
        }

        private static void Msg(string msg)
        {
            HelpShow.Show(msg);
        }
    }
}
