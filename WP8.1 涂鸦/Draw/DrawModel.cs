using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draw
{
   public  class DrawModel
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public int Page { get; set; }
        public int ProgressIndex { get; set; }

        private List<LineModel> _listline;
        public List<LineModel> ListLine
        {
            get
            {
                return _listline;
            }
        }

        public string Serialize()
        {
            StringBuilder str = new StringBuilder();
            string format = "{0}:";
            str.AppendFormat(format, new object[] { Page });
            str.AppendFormat(format, new object[] { ProgressIndex });
            str.AppendFormat(format, new object[] { Width });
            str.AppendFormat(format, new object[] { Height });
          foreach (var item in ListLine)
          {
               str.AppendFormat(format, new object[] { item.Index });
                str.AppendFormat(format, new object[] { item.IsVisible });
                str.AppendFormat(format, new object[] { item.X1 });
                str.AppendFormat(format, new object[] { item.Y1 });
                str.AppendFormat(format, new object[] { item.X2 });
                str.AppendFormat(format, new object[] { item.Y2 });
          }
             str.Append(";");
            return str.ToString();
        }

        public void Deserialize(string value)
        {
            try
            {
                if (value != "")
                {
                    string[] strArray = value.Split(':');
                    if (strArray != null && strArray.Length > 0)
                    {
                        if (_listline == null)
                        {
                            _listline = new List<LineModel>();
                        }
                        else
                        {
                            _listline.Clear();
                        }
                        Page = Convert.ToInt32(strArray[0]);
                        ProgressIndex = Convert.ToInt32(strArray[1]);
                        Width = Convert.ToDouble(strArray[2]);
                        Height = Convert.ToDouble(strArray[3]);
                        for (int i = 4; i < strArray.Length; i += 6)
                        {
                            if (strArray[i].ToString() == ";")
                            {
                                return;
                            }
                            LineModel item = new LineModel(0, 0, 0, 0, 0)
                            {
                                Index = Convert.ToInt32(strArray[i]),
                                IsVisible = Convert.ToBoolean(strArray[i + 1]),
                                X1 = Convert.ToDouble(strArray[i + 2]),
                                Y1 = Convert.ToDouble(strArray[i + 3]),
                                X2 = Convert.ToDouble(strArray[i + 4]),
                                Y2 = Convert.ToDouble(strArray[i + 5])
                            };
                            ListLine.Add(item);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public DrawModel(string value)
        {
            Deserialize(value);
        }

        public DrawModel(int page, double width, double height)
        {
            Page = page;
            Width = width;
            Height = height;
            _listline = new List<LineModel>();
        }

        public DrawModel Clone()
        {
            DrawModel drawModel = new DrawModel(Page, Width, Height)
            {
                ProgressIndex = this.ProgressIndex
            };

         foreach (var item in ListLine)
            {
                LineModel line = new LineModel()
                {
                    Index = item.Index,
                    IsVisible = item.IsVisible,
                    X1 = item.X1,
                    Y1 = item.Y1,
                    X2 = item.X2,
                    Y2 = item.Y2
                };
                drawModel.ListLine.Add(line);
            }
            return drawModel;
        }

        public IEnumerable<LineModel> GetByIndex(int index)
        {
            return from item in this.ListLine where item.Index == index select item;
        }

        public LineModel GetFirstInvisible()
        {
            foreach (var item in ListLine)
            {
                if (!item.IsVisible) return item;
            }
            return null;
        }

        public LineModel GetLastVisible()
        {
            for (int i = ListLine.Count - 1; i >= 0; i--)
            {
                if (ListLine[i].IsVisible) return ListLine[i];
            }
            return null;
        }

        public void ResetInvisiblePoint()
        {
            var items = from item in ListLine where item.IsVisible select item;

            if (items != null)
            {
                var listItems = items.ToList();
                ListLine.Clear();
                ListLine.AddRange(listItems);
            }

        }
    }
}
