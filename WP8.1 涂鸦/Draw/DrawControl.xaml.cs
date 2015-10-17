using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234236 上提供

namespace Draw
{
    public sealed partial class DrawControl : UserControl
    {
      
        private SolidColorBrush _brush;
        private DrawModel _currentPage;
        private Line _linetemp;
        private double _strokeThickness = 7.0;
        private Point oldPoint;
        private Point currentPoint;
        private DispatcherTimer _playbackTimer;
        private int i=0;

        public static readonly DependencyProperty CurrentPageCountProperty = DependencyProperty.Register("CurrentPageCount", typeof(int), typeof(DrawControl), new PropertyMetadata(0));

        public int CurrentPageCount
        {
            get
            {
                return (int)base.GetValue(CurrentPageCountProperty);
            }
            set
            {
                base.SetValue(CurrentPageCountProperty, value);
            }
        }
        private List<DrawModel> ListPage { get; set; }

        public DrawControl()
        {
            this.InitializeComponent();
            this._brush = new SolidColorBrush(Colors.Red);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.InizializeFirstPage();
        
        }

      

        public void InizializeFirstPage()
        {
            if (this.ListPage == null)
            {
                this.ListPage = new List<DrawModel>();
                this._currentPage = new DrawModel(this.ListPage.Count, this.panel.ActualWidth, this.panel.ActualHeight);
                this.ListPage.Add(this._currentPage);
                this.panel.Children.Clear();
                this.CurrentPageCount = 1;
            }
            else if (this.ListPage.Count > 0)
            {
                this._currentPage = this.ListPage[0];
                this.CurrentPageCount = 1;
                this.ReloadData();
            }
            else if (this.ListPage.Count == 0)
            {
                this._currentPage = new DrawModel(this.ListPage.Count, this.panel.ActualWidth, this.panel.ActualHeight);
                this.ListPage.Add(this._currentPage);
                this.panel.Children.Clear();
                this.CurrentPageCount = 1;
            }
        }

        private void ReloadData()
        {
            try
            {
                if (this._currentPage != null)
                {
                    foreach (LineModel line in this._currentPage.ListLine)
                    {
                        if (line.IsVisible)
                        {
                            Line line2 = new Line
                            {
                                X1 = line.X1,
                                Y1 = line.Y1,
                                X2 = line.X2,
                                Y2 = line.Y2,
                                Stroke = this._brush,
                                StrokeThickness = this._strokeThickness,
                                StrokeLineJoin = PenLineJoin.Round,
                                StrokeStartLineCap = PenLineCap.Round,
                                StrokeEndLineCap = PenLineCap.Round
                            };
                            this.panel.Children.Add(line2);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }



     

        public IEnumerable<DrawModel> GetListPage()
        {
            return this.ListPage.Where<DrawModel>(delegate(DrawModel item)
            {
                item.ResetInvisiblePoint();
                return (item.ListLine.Count > 0);
            });
        }

        public void SetListPage(List<DrawModel> lst)
        {
            if (lst != null)
            {
                if (this.ListPage != null)
                {
                    this.ListPage.Clear();
                }
                this.ListPage = new List<DrawModel>();
                this.ListPage.AddRange(from item in lst select item.Clone());
                this.InizializeFirstPage();
            }
        }

        public void NextPage()
        {
            if ((this._currentPage != null) && (this._currentPage.ListLine.Count != 0))
            {
                int index = this.ListPage.IndexOf(this._currentPage);
                this._linetemp = null;
                if (index < (this.ListPage.Count - 1))
                {
                    this._currentPage = this.ListPage[index + 1];
                    this.panel.Children.Clear();
                    this.ReloadData();
                }
                else if (index >= (this.ListPage.Count - 1))
                {
                    this._currentPage = new DrawModel(this.ListPage.Count, this.panel.ActualWidth, this.panel.ActualHeight);
                    this.ListPage.Add(this._currentPage);
                    this.panel.Children.Clear();
                    this.ReloadData();
                }
                if (this._currentPage != null)
                {
                    this.CurrentPageCount = this.ListPage.IndexOf(this._currentPage) + 1;
                }
            }
        }

        public void PreviousPage()
        {
            if (this._currentPage != null)
            {
                int index = this.ListPage.IndexOf(this._currentPage);
                this._linetemp = null;
                if (index > 0)
                {
                    this._currentPage = this.ListPage[index - 1];
                    this.panel.Children.Clear();
                    this.ReloadData();
                }
                if (this._currentPage != null)
                {
                    this.CurrentPageCount = this.ListPage.IndexOf(this._currentPage) + 1;
                }
            }
        }

        public void Clear()
        {
           
                this._currentPage.ListLine.Clear();
                this.panel.Children.Clear();
            
           
        }



        public void Rendo()
        {
            if (!this._isManipulation)
            {
                LineModel firstInvisible = this._currentPage.GetFirstInvisible();
                if (firstInvisible != null)
                {
                    foreach (LineModel line2 in this._currentPage.GetByIndex(firstInvisible.Index))
                    {
                        line2.IsVisible = true;
                        Line line3 = new Line
                        {
                            X1 = line2.X1,
                            Y1 = line2.Y1,
                            X2 = line2.X2,
                            Y2 = line2.Y2,
                            Stroke = this._brush,
                            StrokeThickness = this._strokeThickness,
                            StrokeLineJoin = PenLineJoin.Round,
                            StrokeStartLineCap = PenLineCap.Round,
                            StrokeEndLineCap = PenLineCap.Round
                        };
                        this.panel.Children.Insert(this._currentPage.ListLine.IndexOf(line2), line3);
                    }
                }
            }
        }

        public void Undo()
        {
            if (!this._isManipulation)
            {
                LineModel lastVisible = this._currentPage.GetLastVisible();
                if (lastVisible != null)
                {
                    IEnumerable<LineModel> byIndex = this._currentPage.GetByIndex(lastVisible.Index);
                    if (byIndex.Count<LineModel>() > 0)
                    {
                        LineModel[] lineArray = byIndex.ToArray<LineModel>();
                        for (int i = lineArray.Length - 1; i >= 0; i--)
                        {
                            int index = this._currentPage.ListLine.IndexOf(lineArray[i]);
                            lineArray[i].IsVisible = false;
                            this.panel.Children.RemoveAt(index);
                        }
                    }
                }
            }
        }

        //画图
        private bool _isManipulation=false;



      



    

        private void UserControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
          
            this._linetemp = null;
            currentPoint = e.GetCurrentPoint(panel).Position;
            oldPoint = currentPoint;
            this._currentPage.ResetInvisiblePoint();
            this._currentPage.ProgressIndex++;  

        }

        private void UserControl_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            currentPoint = e.GetCurrentPoint(panel).Position;
            this._linetemp = new Line
            {
                X1 = oldPoint.X,
                Y1 = oldPoint.Y,
                X2 = currentPoint.X,
                Y2 = currentPoint.Y,
                Stroke = this._brush,
                StrokeThickness = this._strokeThickness,
                StrokeLineJoin = PenLineJoin.Round,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round
            };

            this._currentPage.ListLine.Add(new LineModel(this._linetemp.X1, this._linetemp.Y1, this._linetemp.X2, this._linetemp.Y2, this._currentPage.ProgressIndex));
            this.panel.Children.Add(this._linetemp);

            oldPoint = currentPoint;
        }

      
        public void palyBack()
        {
            if ((_currentPage != null))
            {
                this.panel.Children.Clear();
                _playbackTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };
                _playbackTimer.Tick += _playbackTimer_Tick;
                _playbackTimer.Start();
            }
        }

        void _playbackTimer_Tick(object sender, object e)
        {
           if (i<_currentPage.ListLine.Count)
	       {
		       this._linetemp = new Line
                {
                    X1 = _currentPage.ListLine[i].X1,
                    Y1 = _currentPage.ListLine[i].Y1,
                    X2 = _currentPage.ListLine[i].X2,
                    Y2 = _currentPage.ListLine[i].Y2,
                    Stroke = this._brush,
                    StrokeThickness = this._strokeThickness,
                    StrokeLineJoin = PenLineJoin.Round,
                    StrokeStartLineCap = PenLineCap.Round,
                    StrokeEndLineCap = PenLineCap.Round
                };
               this.panel.Children.Add(this._linetemp);
               i++;
	        }
           else if (i==_currentPage.ListLine.Count)
           {
               _playbackTimer.Stop();
               i = 0;
           }
               
              
           
        }

        public void ShowBody()
        {

            if ((_currentPage != null))
            {
                ShowDrawingsControl drawings2 = new ShowDrawingsControl
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    DataSource = _currentPage
                };
                ShowDrawingsControl drawings = drawings2;
                this.body.Items.Add(drawings);
            }


        }

       
       

      
    }
}
