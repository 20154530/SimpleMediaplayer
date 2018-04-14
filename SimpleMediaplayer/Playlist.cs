using System;
using System.Collections.Generic;
using Windows.Media.Core;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace SimpleMediaplayer
{
    public sealed class PlayList : ListView
    {
        #region 关联播放控制器
        public ControlBar AttachedControlBar
        {
            get { return (ControlBar)GetValue(AttachedControlBarProperty); }
            set { SetValue(AttachedControlBarProperty, value); }
        }
        public static readonly DependencyProperty AttachedControlBarProperty =
            DependencyProperty.Register("AttachedControlBar", typeof(ControlBar), typeof(PlayList), new PropertyMetadata(null));
        #endregion

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

        #region 文件播放列表
        private static readonly DependencyProperty FileListProperty = DependencyProperty.RegisterAttached("FileList", typeof(List<MediaInfo>), typeof(PlayList),
            new PropertyMetadata(null,new PropertyChangedCallback(OnFileListChanged)));

        private static void OnFileListChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlayList p = (PlayList)d;
            p.ItemsSource = e.NewValue;
        }

        public List<MediaInfo> FileList
        {
            get { return (List<MediaInfo>)this.GetValue(FileListProperty); }
            set { this.SetValue(FileListProperty, value); }
        }
        #endregion

        #region 重写
        protected override void OnApplyTemplate()
        {
            var RepeatButton = GetTemplateChild("RepeatButton") as ToggleButton;
            RepeatButton.Click += RepeatButton_Click;
            base.OnApplyTemplate();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var NowPlaySource = MediaSource.CreateFromStorageFile(((MediaInfo)SelectedItem).FileAccess);
            AttachedControlBar.SetSource(NowPlaySource);
        }
        #endregion

        #region 公共方法

        #endregion

        #region 内部控件方法
        //循环模式
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

        #endregion

        public PlayList()
        {
            this.DefaultStyleKey = typeof(PlayList);
            this.SelectionChanged += OnSelectionChanged;
        }
    }


    public class MediaInfo
    {
        public String FileName { get; set; }       
        public ImageSource FileScaledImage { get; set; }
        public StorageFile FileAccess { get; set; }
    }
}
