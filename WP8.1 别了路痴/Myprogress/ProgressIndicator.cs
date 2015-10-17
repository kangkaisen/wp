using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Phone.UI.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Myprogress
{
    
        public sealed class ProgressIndicator : ContentControl
        {
           
            private TextBlock textBlockStatus;
      

           
           
            private string labelText;

            public ProgressIndicator()
            {
                DefaultStyleKey = typeof(ProgressIndicator);
            }

           

            protected override void OnApplyTemplate()
            {
                base.OnApplyTemplate();
             
                textBlockStatus = GetTemplateChild("textBlockStatus") as TextBlock;
                InitializeProgressType();
            }

            

            public string Text
            {
                get
                {
                    return labelText;
                }
                set
                {
                    labelText = value;
                }
            }

            

            internal Popup ChildWindowPopup
            {
                get;
                private set;
            }

            private static Frame RootVisual
            {
                get
                {
                    return Window.Current == null ? null : Window.Current.Content as Frame;
                }
            }

            internal Page Page
            {
                get { return RootVisual.GetVisualDescendants().OfType<Page>().FirstOrDefault(); }
            }

            public bool IsOpen
            {
                get
                {
                    return ChildWindowPopup != null && ChildWindowPopup.IsOpen;
                }
            }

            public void Show()
            {
                if (ChildWindowPopup == null)
                {
                    ChildWindowPopup = new Popup();
                    ChildWindowPopup.Child = this;
                    ChildWindowPopup.IsLightDismissEnabled = true;
                   
                }
                
                ChildWindowPopup.IsOpen = true;
               
               

            }

            public void Hide()
            {
                

                ChildWindowPopup.IsOpen = false;
            }


        

            private void InitializeProgressType()
            {
              
          
                      
                       
                        textBlockStatus.Visibility = Visibility.Visible;
                        textBlockStatus.FontSize = 20;
                        
                        textBlockStatus.Text = Text;
                        textBlockStatus.TextWrapping = TextWrapping.Wrap;
                      
              
            }

        }
    
}
