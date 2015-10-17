using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace LinqJson.Model
{
    public class Note:ModelBase
    {
      

        private Guid id = Guid.NewGuid();
                
        public Guid ID
        {
            get { return id; }
            set { id= value; }
        }
        
        

        private string name;

        public string Name
        {
            get { return name; }
            set
            { this.SetProperty(ref this.name, value); }
        }

        private string content;

        public string Content
        {
            get { return content; }
            set
            { this.SetProperty(ref this.content, value); }
        }

        private string imagePath;

        public string ImagePath
        {
            get { return imagePath; }
            set
            { this.SetProperty(ref this.imagePath, value); }
        }

        
        
      
        
       
    }
}
