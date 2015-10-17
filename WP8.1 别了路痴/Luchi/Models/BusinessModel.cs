using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luchi.Models
{
   public  class BusinessModel
    {
        public string index_num { get; set; }
        public int total { get; set; }
        public int result_num { get; set; }
        public string web_url { get; set; }
        public string wap_url { get; set; }
        public bizsModel bizs { get; set; }
        public struct bizsModel
        {
            public List<bizModel> biz { get; set; }
        }
        public struct bizModel
        {
            public string id { get; set; }
            public string name { get; set; }
            public string addr { get; set; }
            public string tel { get; set; }
            public string cate { get; set; }
            public float rate { get; set; }
            public int cost { get; set; }
            public string desc { get; set; }
            public string  dist { get; set; }
            public string  lng { get; set; }
            public string  lat { get; set; }
            public string img_url { get; set; }
           
            

        }
    }
}
