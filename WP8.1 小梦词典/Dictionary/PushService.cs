using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.PushNotifications;

namespace Dictionary
{
    public class PushService
    {
        public static async void CreateChannel()
        {
            PushNotificationChannel channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            //如果本地设置中没有相关键，表明是第一次使用
            // 需要存储URL，并发送给服务器
            if (Windows.Storage.ApplicationData.Current.LocalSettings.Values.ContainsKey("url") == false)
            {
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["url"] = channel.Uri;
                //SendURL(channel.Uri);
            }
            else
            {
                string savedUrl = Windows.Storage.ApplicationData.Current.LocalSettings.Values["url"] as string;
                // 当URL改变了，就重新发给服务器
                if (savedUrl != channel.Uri)
                {
                    // 再次保存本地设置
                    Windows.Storage.ApplicationData.Current.LocalSettings.Values["url"] = channel.Uri;
                    //SendURL(channel.Uri);
                }
            }

            System.Diagnostics.Debug.WriteLine(channel.Uri);
            //SendURL(channel.Uri);
           // channel.PushNotificationReceived += channel_PushNotificationReceived;
        }
    }
}
