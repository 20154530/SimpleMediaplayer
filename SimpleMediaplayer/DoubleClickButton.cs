using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SimpleMediaplayer
{
    public sealed class DoubleClickButton : ContentControl
    {
        public enum State
        {
            normal = 0,
            cover = 1,
        }

        #region Textcontent
        public String Textcontent
        {
            get { return (String)GetValue(TextcontentProperty); }
            set { SetValue(TextcontentProperty, value); }
        }
        public static readonly DependencyProperty TextcontentProperty =
            DependencyProperty.Register("Textcontent", typeof(String), typeof(DoubleClickButton), new PropertyMetadata(0));
        #endregion
      
        #region State
        public State BState
        {
            get { return (State)GetValue(BStateProperty); }
            set { SetValue(BStateProperty, value); }
        }
        public static readonly DependencyProperty BStateProperty =
            DependencyProperty.Register("BState", typeof(State), typeof(DoubleClickButton), new PropertyMetadata(State.normal));
        #endregion

        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            switch (BState)
            {
                case State.normal:
                    VisualStateManager.GoToState(this, "Cover", false);
                    BState = State.cover;
                    e.Handled = true;
                    break;
                case State.cover:
                    VisualStateManager.GoToState(this, "Normal", false);
                    BState = State.normal;
                    break;
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        public DoubleClickButton()
        {
            this.DefaultStyleKey = typeof(DoubleClickButton);
        }
    }
}
