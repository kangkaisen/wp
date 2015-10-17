using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Luchi.Models
{
   public class PointList
    {
        public PointList()
        {
            Points = new List<Geopoint>();
        }
        public string Name { get; set; }

        public List<Geopoint> Points { get; set; }
    }
}
