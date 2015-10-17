using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class ShowDrawingsControl : UserControl
    {
        public ShowDrawingsControl()
        {
            this.InitializeComponent();
        }
        public static readonly DependencyProperty DataSourceProperty = DependencyProperty.Register("DataSource", typeof(object), typeof(ShowDrawingsControl), new PropertyMetadata(null, new PropertyChangedCallback(ShowDrawingsControl.DataSourceChanged)));

       

        private static void DataSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ShowDrawingsControl drawings = sender as ShowDrawingsControl;
            if ((drawings.DataSource != null) && (drawings.DataSource is DrawModel))
            {
                drawings.Draw();
            }
        }
        private double Scale = 0.3;
        internal void Draw()
        {
                this.canvas.Children.Clear();
                DrawModel drawModel = DataSource as DrawModel;
                canvas.Height = drawModel.Height * Scale;
                canvas.Width = drawModel.Width * Scale;

                foreach (LineModel line in drawModel.ListLine)
                {
                    if (line.IsVisible)
                    {
                        Line line2 = new Line
                        {
                            X1 = line.X1 * Scale,
                            Y1 = line.Y1 * Scale,
                            X2 = line.X2 * Scale,
                            Y2 = line.Y2 * Scale,
                            Stroke = new SolidColorBrush(Colors.Red),
                            StrokeThickness = 0.4,
                            StrokeLineJoin = PenLineJoin.Round,
                            StrokeStartLineCap = PenLineCap.Round,
                            StrokeEndLineCap = PenLineCap.Round
                        };
                        canvas.Children.Add(line2);
                    }
                }
               

            
        }

        public object DataSource
        {
            get
            {
                return base.GetValue(DataSourceProperty);
            }
            set
            {
                base.SetValue(DataSourceProperty, value);
            }
        }
    }
}
