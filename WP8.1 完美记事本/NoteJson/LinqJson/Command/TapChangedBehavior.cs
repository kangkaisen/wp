using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace LinqJson.Command
{
   public static class  TapChangedBehavior
    {
       public static readonly DependencyProperty TapChangedCommandProperty =
           DependencyProperty.RegisterAttached(
           "TapChangedCommand",
            typeof(ICommand),
            typeof(TapChangedBehavior),
            new PropertyMetadata(null, new PropertyChangedCallback(TapChangedPropertyChangedCallback)));

        public static ICommand GetTapChangedCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(TapChangedCommandProperty);
        }

        public static void SetTapChangedCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(TapChangedCommandProperty, value);
        }
        private static void TapChangedPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Image image = (Image)d;
            image.Tapped+=image_Tapped;
            
        }

        static void image_Tapped(object sender, TappedRoutedEventArgs e)
        {
             Image image = (Image)sender;
             GetTapChangedCommand(image).Execute(image);
        }
       

      
      
    }
}
