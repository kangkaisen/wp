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
    public class BusTranferViewModel:ViewModelBase
    {
        public class buslist:ViewModelBase
        {

            private string  start;

            public string  Start
            {
                get { return start; }
                set
                { this.SetProperty(ref this.start, value); }
            }

            private string end;

            public string End
            {
                get { return end; }
                set
                { this.SetProperty(ref this.end, value); }
            }
            


            private string linename;

            public string Line_name
            {
                get { return linename; }
                set
                { this.SetProperty(ref this.linename, value); }
            }
            





            private string stats;

            public string Stats
            {
                get { return stats; }
                set
                { this.SetProperty(ref this.stats, value); }
            }
            

            
            private string dist;

            public string Dist
            {
                get { return dist; }
                set
                { this.SetProperty(ref this.dist, value); }
            }



            private string time;

            public string Time
            {
                get { return time; }
                set
                { this.SetProperty(ref this.time, value); }
            }
         }

        private List<buslist> busList;

        public List<buslist> Buslist
        {
            get { return busList; }
            set
            { this.SetProperty(ref this.busList, value); }
        }


        public List<buslist> BusDemoList { get; set; }

        
        private string startStop;

        public string StartStop
        {
            get { return startStop; }
            set
            { this.SetProperty(ref this.startStop, value); }
        }

        private string endStop;

        public string EndStop
        {
            get { return endStop; }
            set
            { this.SetProperty(ref this.endStop, value); }
        }
        
        
        public DelegateCommand FindCommand { get; set; }
        public BusTranferViewModel()
        {
            FindCommand = new DelegateCommand(Find);
            BusDemoList = new List<buslist>();
        }


        private async void Find()
        {
            if (StartStop != null && EndStop != null)
            {
                string uri = "http://openapi.aibang.com/bus/transfer?alt=json&app_key=" + App.AiBangApi + "&city=" + App.City + "&start_addr=" + StartStop + "&end_addr=" + EndStop;
                string response=await HttpGet.Get(uri);
                try
                {
                    DataContractJsonSerializer ds = new DataContractJsonSerializer(typeof(BusTransferModel));
                    MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(response));
                    BusTransferModel bus = ds.ReadObject(ms) as BusTransferModel;


                    foreach (var item in bus.buses.bus)
                    {
                        buslist busdemo = new buslist();
                        busdemo.Start = "起点站:" + item.segments.segment.FirstOrDefault().start_stat;
                        busdemo.End = "终点站:" + item.segments.segment.FirstOrDefault().end_stat;
                        busdemo.Dist = "距离: " + item.dist.ToString() + "米";
                        busdemo.Time = "预计耗时: " + item.time.ToString() + "分";
                        busdemo.Line_name = "线路名称: " + item.segments.segment.FirstOrDefault().line_name;
                        busdemo.Stats = "沿途站点: " + item.segments.segment.FirstOrDefault().stats;
                        BusDemoList.Add(busdemo);
                    }
                    ms.Dispose();
                    Buslist = BusDemoList;
                    BusDemoList = new List<buslist>();
                }
                catch
                {
                    Msg("亲:请检查您输入的起点或终点名称是否有误.请尝试输入相似或者相近的站点名称");
                }
                
            }
            else
            {
                Msg("请您输入起点和终点！");
            }
        }

        private static void Msg(string msg)
        {
            HelpShow.Show(msg);
        }
    }
}
