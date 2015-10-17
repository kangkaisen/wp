using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media;
using Windows.Web.Http;

namespace Luchi.Common
{
    public class HttpGet
    {
        private static string responseString;

        public static async Task<string> Get(string uri)
        {
            HttpClient httpClient = new HttpClient();
            Myprogress.ProgressIndicator progressIndicator = new Myprogress.ProgressIndicator();
            progressIndicator.Text = "正在加载...";

            progressIndicator.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x23, 0xAC, 0x5B));
         
            progressIndicator.Width = 400;
            progressIndicator.Height = 60;
            progressIndicator.Show();

            var headers = httpClient.DefaultRequestHeaders;
            headers.UserAgent.ParseAdd("ie");
            headers.UserAgent.ParseAdd("Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                response = await httpClient.GetAsync(new Uri(uri,UriKind.Absolute));

                response.EnsureSuccessStatusCode();

                responseString = await response.Content.ReadAsStringAsync();
               
                
               
            }
            catch (Exception ex)
            {
               
                
            }
            progressIndicator.Hide();
            return responseString;

        }

       
    }
}
