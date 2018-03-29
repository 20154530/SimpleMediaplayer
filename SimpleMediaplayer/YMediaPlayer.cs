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

namespace SimpleMediaplayer
{
    public sealed class YMediaPlayer : MediaPlayerElement
    {
        #region 正在播放的文件名
        public static readonly DependencyProperty FilenameProperty = DependencyProperty.RegisterAttached("Filename", typeof(String), typeof(ControlBar), new PropertyMetadata(""));

        public String Filename
        {
            get { return (String)this.GetValue(FilenameProperty); }
            set { this.SetValue(FilenameProperty, value); }
        }
        #endregion

        public YMediaPlayer()
        {
            this.DefaultStyleKey = typeof(YMediaPlayer);
            this.TransportControls = new ControlBar();
        }
    }
}
