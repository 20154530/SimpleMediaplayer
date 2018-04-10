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

        public YMediaPlayer()
        {
            this.DefaultStyleKey = typeof(YMediaPlayer);
        }
    }
}
