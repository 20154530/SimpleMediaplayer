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
    public sealed class StackButton : Button
    {
        #region Textcontent
        public String Textcontent
        {
            get { return (String)GetValue(TextcontentProperty); }
            set { SetValue(TextcontentProperty, value); }
        }
        public static readonly DependencyProperty TextcontentProperty =
            DependencyProperty.Register("Textcontent", typeof(String), typeof(StackButton), new PropertyMetadata(0));
        #endregion

        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "PointerOver", false);

            base.OnPointerEntered(e);
        }


        public StackButton()
        {
            this.DefaultStyleKey = typeof(StackButton);
        }
    }
}
