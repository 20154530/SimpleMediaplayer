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
    public sealed class PlayList : ListView
    {
        #region 播放列表可见性
        private static readonly DependencyProperty IsPlaylistVisibleProperty = DependencyProperty.RegisterAttached("IsPlaylistVisible", typeof(bool), typeof(PlayList),
            new PropertyMetadata(false));
        public bool IsPlaylistVisible
        {
            get { return (bool)this.GetValue(IsPlaylistVisibleProperty); }
            set { this.SetValue(IsPlaylistVisibleProperty, value); }
        }
        #endregion

        public PlayList()
        {
            this.DefaultStyleKey = typeof(PlayList);
        }
    }
}
