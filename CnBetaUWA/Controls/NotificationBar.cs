using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace CnBetaUWA.Controls
{
    /// <summary>
    /// 通知提示控件
    /// </summary>
    [TemplatePart(Name = "mainGrid", Type = typeof(Grid))]
    [TemplatePart(Name = "notifyTextblock", Type = typeof(TextBlock))]
   public sealed class NotificationBar : Control
    {
        private static TextBlock notifyTextblock;
        private static Grid mainGrid;
        private static Storyboard storyBoard;
        public NotificationBar()
        {
            this.DefaultStyleKey = typeof(NotificationBar);
        }

        protected override void OnApplyTemplate()
        {
            mainGrid = GetTemplateChild("mainGrid") as Grid;
            notifyTextblock=GetTemplateChild("notifyTextblock") as TextBlock;
            storyBoard=GetTemplateChild("storyBoard") as Storyboard;
        }


        private static void ShowMessage(string message)
        {
            if (notifyTextblock == null || storyBoard == null) return;
            notifyTextblock.Text = message;
            storyBoard.Begin();
        }


        /// <summary>
        /// Message
        /// </summary>
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(NotificationBar), new PropertyMetadata(string.Empty, OnMessageChanged));

        private static void  OnMessageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var message = e.NewValue as string;
           
            if (message!=string.Empty)
            {
              ShowMessage(message);
            }
        }
    }
}
