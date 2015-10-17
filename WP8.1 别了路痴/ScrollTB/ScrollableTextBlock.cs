using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ScrollTB
{
    public class ScrollableTextBlock : ContentControl
    {
        private StackPanel stackPanel;

        public ScrollableTextBlock()
        {
            DefaultStyleKey = typeof(ScrollableTextBlock);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(ScrollableTextBlock),
                new PropertyMetadata("ScrollableTextBlock", OnTextPropertyChanged));
     
        public string Text 
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }
     
        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScrollableTextBlock source = (ScrollableTextBlock)d;
            string value = (string)e.NewValue;
            source.ParseText(value);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.stackPanel = this.GetTemplateChild("StackPanel") as StackPanel;
            this.ParseText(this.Text);
        }

        private void ParseText(string value)//处理文字数据,依据空格折行
        {
            string[] textBlockTexts = value.Split(';');

            if (this.stackPanel == null)
            {
                return;
            }

            this.stackPanel.Children.Clear();

            for (int i = 0; i < textBlockTexts.Length; i++)
            {
                TextBlock textBlock = this.GetTextBlock();
                textBlock.Text = textBlockTexts[i].ToString();
                this.stackPanel.Children.Add(textBlock);
            }
        }


        private TextBlock GetTextBlock()//生成一个TextBlock控件
        {
            TextBlock textBlock = new TextBlock();
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.FontSize = this.FontSize;
            textBlock.FontFamily = this.FontFamily;
            textBlock.FontWeight = this.FontWeight;
            textBlock.Foreground = this.Foreground;
            textBlock.Margin = new Thickness(0, 0, 0, 0);
            return textBlock;
        }
    }
}
