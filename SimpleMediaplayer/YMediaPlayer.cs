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
        private static readonly DependencyProperty NowPlayProperty = DependencyProperty.RegisterAttached("NowPlay", typeof(String), typeof(YMediaPlayer),
            new PropertyMetadata("No File Now !"));
        public String NowPlay
        {
            get { return (String)this.GetValue(NowPlayProperty); }
            set { this.SetValue(NowPlayProperty, value); }
        }
        #endregion

        #region 文件播放列表
        private static readonly DependencyProperty PalyListProperty = DependencyProperty.RegisterAttached("PlayList", typeof(List<String>), typeof(YMediaPlayer),
            new PropertyMetadata(null));
        public List<String> PlayList
        {
            get { return (List<String>)this.GetValue(PalyListProperty); }
            set { this.SetValue(PalyListProperty, value); }
        }
        #endregion



        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);
        }



        public YMediaPlayer()
        {
            this.DefaultStyleKey = typeof(YMediaPlayer);
            this.TransportControls = new ControlBar();
        }
    }
}
