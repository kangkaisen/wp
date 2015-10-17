using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luchi.Models
{
    public class BusStationModel
    {
        public int result_num { get; set; }
        public string web_url { get; set; }
        public string wap_url { get; set; }
        public statsModel stats { get; set;}
        public struct statsModel
        {
            public List<statModel> stat { get; set; }
        }

        public struct statModel
        {
            public string name { get; set; }
            public string xy { get; set; }
            public string line_names { get; set; }
        }
    }
}
