using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Notifications;

namespace Dictionary
{
    public  static class Help
    {
        public static void UpdateBadge(int wordCount)
             
        {
            XmlDocument xdoc = BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeNumber);
            XmlElement xBadge = (XmlElement)xdoc.SelectSingleNode("/badge");
            xBadge.SetAttribute("value", wordCount.ToString());
            BadgeNotification notifi = new BadgeNotification(xdoc);
            BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(notifi);

        }
      
      
    }
}
