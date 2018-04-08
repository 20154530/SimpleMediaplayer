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

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SimpleMediaplayer
{
    public sealed class YCommandBar : CommandBar
    {

        protected override void OnApplyTemplate()
        {
            var PrimaryItemsControl = GetTemplateChild("PrimaryItemsControl") as ItemsControl;
            PrimaryItemsControl.Loaded += PrimaryItemsControl_Loaded;

            base.OnApplyTemplate();
        }

        private void PrimaryItemsControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        public YCommandBar()
        {
            this.DefaultStyleKey = typeof(YCommandBar);
        }
    }
}
