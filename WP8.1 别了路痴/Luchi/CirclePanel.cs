using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Luchi
{
    public class CirclePanel : Panel
    {
        private double _radius = 0;
        public double X;
        public double Y;

        public CirclePanel()
        {

        }
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.RegisterAttached
            ("Radius",
            typeof(double),
            typeof(CirclePanel),
            new PropertyMetadata(0.0, OnRadiusPropertyChanged));

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        private static void OnRadiusPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CirclePanel target = (CirclePanel)obj;
            target._radius = (double)e.NewValue;
            target.InvalidateArrange();
        }

        protected override Size MeasureOverride(Size availableSize)
        {

            double maxElementWidth = 0;

            foreach (UIElement child in Children)
            {
                if (child == Children.FirstOrDefault())
                {

                }
                else
                {
                    child.Measure(availableSize);
                    maxElementWidth = Math.Max(child.DesiredSize.Width, maxElementWidth);
                }

            }

            double panelWidth = 2 * this.Radius + 2 * maxElementWidth;
            double width = Math.Min(panelWidth, availableSize.Width);
            double heigh = Math.Min(panelWidth, availableSize.Height);
            return new Size(width, heigh);
        }


        protected override Size ArrangeOverride(Size finalSize)
        {
            double degree = 0;
            double degreeStep = (double)360 / (this.Children.Count - 1);

            double mX = this.DesiredSize.Width / 2;
            double mY = this.DesiredSize.Height / 2;
            X = mX;
            Y = mY;

            foreach (UIElement child in Children)
            {
                if (child == Children.FirstOrDefault())
                {
                    child.Arrange(new Rect(mX - this._radius, mY - this._radius, this._radius * 2, this._radius * 2));

                }
                else
                {
                    double angle = Math.PI * degree / 180.0;
                    double x = Math.Cos(angle) * this._radius;
                    double y = Math.Sin(angle) * this._radius;
                    RotateTransform rotateTransform = new RotateTransform();
                    rotateTransform.Angle = degree;
                    rotateTransform.CenterX = 0;
                    rotateTransform.CenterY = 0;
                    child.RenderTransform = rotateTransform;


                    child.Arrange(new Rect(mX + x, mY + y, child.DesiredSize.Width, child.DesiredSize.Height));

                    degree += degreeStep;
                }

            }

            return finalSize;
        }
    }
}
