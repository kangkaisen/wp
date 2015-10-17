using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draw
{
    public class Note:ModelBase
    {

        private Guid id = Guid.NewGuid();

        public Guid ID
        {
            get { return id; }
            set { id = value; }
        }

      
        private string title;

        public string Title
        {
            get { return title; }
            set
            { this.SetProperty(ref this.title, value); }
        }

       



        private string draw ="";

        public string Draw
        {
            get { return draw; }
            set
            { this.SetProperty(ref this.draw, value); }
        }
        
    
    }
}
