using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luchi.Models
{
   public  class BusTransferModel
    {
        public int result_num { get; set; }
        public string web_url { get; set; }
        public string  wap_url { get; set; }
        public busesObj buses { get; set; }


        public class busesObj
        {
            public List<busObj> bus { get; set; }
        }

        public class busObj
       {
           public int dist { get; set; }
           public int time { get; set; }
           public int foot_dist { get; set; }
           public int last_foot_dist { get; set; }
           public segmentsObj segments { get; set; }

       }

       public class segmentsObj
       {
           public List<segmentObj> segment { get; set; }
       }

       public class segmentObj
       {
           public string start_stat { get; set; }
           public string end_stat { get; set; }
           public string line_name { get; set; }
           public string stats { get; set; }
           public string stat_xys { get; set; }
           public string line_xys { get; set; }
           public int line_dist { get; set; }
           public int foot_dist { get; set; }
       }
    }
}
