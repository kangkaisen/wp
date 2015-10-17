using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary
{
    public class WordGroup :INotifyPropertyChanged
   {
        private string timeTitle;
        public string TimeTitle
        {
            get { return timeTitle; }
            set
            {
                if (value!=timeTitle)
                {
                    timeTitle = value;
                    RaisePropertyChanged("TimeTitle");
                    
                }
            }
        }
        private ObservableCollection<WordItem> wordItemContent;
        public ObservableCollection<WordItem> WordItemContent
        {
            get { return wordItemContent; }
            set
            {
                if (value!=null)
                {
                    wordItemContent = value;
                    RaisePropertyChanged("WordItemContent");
                }
            }
        }


       public event PropertyChangedEventHandler PropertyChanged;
       protected void RaisePropertyChanged(string propertyName)
       {
           if (PropertyChanged != null)
           {
               PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
           }
       }
   }

   public class WordItem : INotifyPropertyChanged
    {
       private string key;
       public string Key
       {
           get { return key; }
           set
           {
               if (value!=key)
               {
                   key = value;
                   RaisePropertyChanged("Key");
               }
           }
       }
       private string ps;
       public string Ps
       {
           get { return ps; }
           set
           {
               if (value!=ps)
               {
                   ps = value;
                   RaisePropertyChanged("Ps");
               }
           }
       }

       private string acception;
       public string Acception
       {
           get { return acception; }
           set
           {
               if (value!=acception)
               {
                   acception = value;
                   RaisePropertyChanged("Acception");
               }
           }
       }
       private string time;
       public string Time
       {
           get { return time; }
           set
           {
               if (value!=time)
               {
                   time = value;
                   RaisePropertyChanged("Time");
               }
           }
       }
        public string Pos { get; set; }

        public string Trans { get; set; }
        public string Orig { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

     

    }
}
