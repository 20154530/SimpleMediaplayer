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
using Windows.Devices.Input;

namespace SimpleMediaplayer
{
    public sealed class YMediaPlayer : MediaPlayerElement
    {
        #region 关联的控制器
        private ControlBar attachedControlbar;
        public ControlBar AttachedControlbar
        {
            get { return attachedControlbar; }
            set { attachedControlbar = value; }
        }
        #endregion

        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            if (attachedControlbar.IsVisible)
                attachedControlbar.Hide();
            else
                attachedControlbar.Show();
            base.OnTapped(e);
        }

        protected override void OnPointerWheelChanged(PointerRoutedEventArgs e)
        {
            attachedControlbar.ShowVolumeBar();
            attachedControlbar.SetVolumeIncrement(e.GetCurrentPoint(null).Properties.MouseWheelDelta / 120 );
            base.OnPointerWheelChanged(e);
        }

        public YMediaPlayer()
        {
            this.DefaultStyleKey = typeof(YMediaPlayer);
        }
    }
}
