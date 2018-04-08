using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Media.Playback;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace SimpleMediaplayer
{
    public sealed class YMediaPlayer : MediaPlayerElement
    {
        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            base.OnTapped(e);
            
        }

        protected override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            
        }

        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        public YMediaPlayer()
        {
            this.DefaultStyleKey = typeof(YMediaPlayer);
        }
    }
}
