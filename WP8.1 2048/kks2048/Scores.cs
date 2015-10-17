using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace kks2048
{
    public class Scores : INotifyPropertyChanged
    {
        public Scores()
        {
            Score = 0;
        }
        private int score;

        public int Score
        {
            get
            {
                return score;
            }
            set
            {
                if (value!=score)
                {
                    score = value;
                    RaisePropertyChanged("Score");
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

  
}
