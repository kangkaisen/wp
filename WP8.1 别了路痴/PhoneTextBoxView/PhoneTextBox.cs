using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PhoneTextBoxView
{
   public class PhoneTextBox:TextBox
    {
        ContentControl WatermarkContent;
        public static readonly DependencyProperty WatermarkProperty =
      DependencyProperty.Register("Watermark", typeof(object), typeof(PhoneTextBox), new PropertyMetadata(null, OnWatermarkPropertyChanged));

        public static readonly DependencyProperty WatermarkStyleProperty =
      DependencyProperty.Register("WatermarkStyle", typeof(Style), typeof(PhoneTextBox), null);

        public Style WatermarkStyle
        {
            get { return base.GetValue(WatermarkStyleProperty) as Style; }
            set { base.SetValue(WatermarkStyleProperty, value); }
        }

        public object Watermark
        {
            get { return base.GetValue(WatermarkProperty) as object; }
            set { base.SetValue(WatermarkProperty, value); }
        }

        public PhoneTextBox()
        {
            DefaultStyleKey = typeof(PhoneTextBox);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.WatermarkContent = this.GetTemplateChild("watermarkContent") as ContentControl;
            if (WatermarkContent != null)
            {
                DetermineWatermarkContentVisibility();
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            if (WatermarkContent != null && string.IsNullOrEmpty(this.Text))
            {
                this.WatermarkContent.Visibility = Visibility.Collapsed;
            }
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if (WatermarkContent != null && string.IsNullOrEmpty(this.Text))
            {
                this.WatermarkContent.Visibility = Visibility.Visible;
            }
            base.OnLostFocus(e);
        }

        private static void OnWatermarkPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            PhoneTextBox watermarkTextBox = sender as PhoneTextBox;
            if (watermarkTextBox != null && watermarkTextBox.WatermarkContent != null)
            {
                watermarkTextBox.DetermineWatermarkContentVisibility();
            }
        }

        private void DetermineWatermarkContentVisibility()
        {
            if (string.IsNullOrEmpty(this.Text))
            {
                this.WatermarkContent.Visibility = Visibility.Visible;
            }
            else
            {
                this.WatermarkContent.Visibility = Visibility.Collapsed;
            }
        }
    }
}
