using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draw
{
    public class LineModel
    {
        public LineModel()
        {
        }
        public LineModel(double x1, double y1, double x2, double y2, int index)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            Index = index;
        }
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }

        private bool _isvisible = true;
        public bool IsVisible
        {
            get
            {
                return _isvisible;
            }
            set
            {
                _isvisible = value;
            }
        }

        public int Index { get; set; }
    }
}
