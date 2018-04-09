using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using System.Diagnostics;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SimpleMediaplayer
{
    public sealed class PlayList : ListView
    {
        #region 播放列表可见性
        private static readonly DependencyProperty IsPlaylistVisibleProperty = DependencyProperty.RegisterAttached("IsPlaylistVisible", typeof(bool), typeof(PlayList),
            new PropertyMetadata(false,new PropertyChangedCallback(IsPlaylistVisibleChanged)));
        public bool IsPlaylistVisible
        {
            get { return (bool)this.GetValue(IsPlaylistVisibleProperty); }
            set { this.SetValue(IsPlaylistVisibleProperty, value); }
        }
        private static void IsPlaylistVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion

        #region 循环模式
        private static readonly DependencyProperty RepeatModeProperty = DependencyProperty.RegisterAttached("RepeatMode", typeof(int), typeof(PlayList),
            new PropertyMetadata(0));
        public int RepeatMode
        {
            get { return (int)this.GetValue(RepeatModeProperty); }
            set { this.SetValue(RepeatModeProperty, value); }
        }

        public static DependencyProperty IsPlaylistVisibleProperty1 => IsPlaylistVisibleProperty;

        public static DependencyProperty IsPlaylistVisibleProperty2 => IsPlaylistVisibleProperty;
        #endregion

        protected override void OnApplyTemplate()
        {
            var RepeatButton = GetTemplateChild("RepeatButton") as ToggleButton;
            RepeatButton.Click += RepeatButton_Click;
            base.OnApplyTemplate();
        }

        private void RepeatButton_Click(object sender, RoutedEventArgs args)
        {
            switch (RepeatMode)
            {
                case 0:
                    RepeatMode = 1;
                    break;
                case 1:
                    RepeatMode = 2;
                    VisualStateManager.GoToState(this, "RepeatOne", false);
                    break;
                case 2:
                    RepeatMode = 0;
                    VisualStateManager.GoToState(this, "RepeatAll", false);
                    break;
            }
        }

        public PlayList()
        {
            this.DefaultStyleKey = typeof(PlayList);
        }
    }
}
