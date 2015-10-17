using Luchi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luchi.Models
{
    public class BusRouteModel
    {
        public int result_num { get; set; }
        public string web_url { get; set; }
        public string wap_url { get; set; }
        public linesModel lines { get; set; }
        
        public struct linesModel
        {
            public List<lineModel> line { get; set; }
        }

        public struct lineModel
        {
            public string name { get; set; }
            public string info { get; set; }
            public string stats { get; set; }
            public string stat_xys { get; set; }
            public string xys { get; set; }
        }
    }
}
