using Luchi.Command;
using Luchi.Common;
using Luchi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Luchi.ViewModels
{
   public  class BusinessViewModel:ViewModelBase
    {
       public string  parameter { get; set; }
       private List<BusinessModel.bizModel> businessList;

       public List<BusinessModel.bizModel> BusinessList
       {
           get { return businessList; }
           set
           { this.SetProperty(ref this.businessList, value); }
       }
       public BusinessViewModel(string para)
       {
           parameter = para;
           Find();
       }

       private async void Find()
       {
           string uri = "http://openapi.aibang.com/search?alt=json&to=200&rc=2&app_key=" + App.AiBangApi + "&city=" + App.City + "&lng="+App.longitude+"&lat="+App.latitude+"&q="+parameter;
           string response = await HttpGet.Get(uri);
           Debug.WriteLine(response);
              try
            {
               DataContractJsonSerializer ds = new DataContractJsonSerializer(typeof(BusinessModel));
               MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(response));
              
                   BusinessModel businessDemo = (BusinessModel)ds.ReadObject(ms);
               
                
                    BusinessList = businessDemo.bizs.biz;
                
               }
            catch
           {
               Msg("亲,请您换个关键词试试");
           }
       }

       private static void Msg(string msg)
       {
           HelpShow.Show(msg);
       }
    }
}
