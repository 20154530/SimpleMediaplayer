using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Power;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Core;
using Windows.ApplicationModel.Core;
using Windows.Media.Core;
using Windows.UI.ViewManagement;
using Windows.Graphics.Display;
using Windows.Devices.Input;
using System.Diagnostics;
using Windows.UI.Xaml.Media.Animation;

namespace SimpleMediaplayer
{
    public sealed class ControlBar : Control
    {
        private DispatcherTimer ProgressTimer;
        private DispatcherTimer VolumeSliderVisiblityTimer;

        private PlayList PlayListControl;
        private Slider ProgressSlider;
        private Slider VolumeSlider;

        private bool IsURLResourcesSearchVisible = false;
        private bool IsAdditionSettingVisible = false;
        private bool IsFullWindow = false;
        public bool IsVisible { get; set; }

        #region 正在播放的文件名
        private static readonly DependencyProperty NowPlayProperty = DependencyProperty.RegisterAttached("NowPlay", typeof(String), typeof(ControlBar),
            new PropertyMetadata("No File Now !"));
        public String NowPlay
        {
            get { return (String)this.GetValue(NowPlayProperty); }
            set { this.SetValue(NowPlayProperty, value); }
        }
        #endregion

        #region 关联播放核心
        private static readonly DependencyProperty AttachedMediaPlayerProperty = DependencyProperty.RegisterAttached("AttachedMediaPlayer", typeof(MediaPlayer), typeof(ControlBar),
            new PropertyMetadata(null));
        public MediaPlayer AttachedMediaPlayer
        {
            get { return (MediaPlayer)GetValue(AttachedMediaPlayerProperty); }
            set { SetValue(AttachedMediaPlayerProperty, value); }
        }
        #endregion

        #region 文件播放列表
        private static readonly DependencyProperty PalyListProperty = DependencyProperty.RegisterAttached("PlayList", typeof(List<String>), typeof(ControlBar),
            new PropertyMetadata(null));
        public List<String> PlayList
        {
            get { return (List<String>)this.GetValue(PalyListProperty); }
            set { this.SetValue(PalyListProperty, value); }
        }
        #endregion

        #region 重写

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "PlayListHide", false);
            VisualStateManager.GoToState(this, "URLResources_Search_Hide", false);
            IsVisible = true;
        }

        protected override void OnApplyTemplate()
        {
            var OpenUrlRescourse = GetTemplateChild("OpenUrlRescourse") as Button;
            OpenUrlRescourse.Click += OpenUrlRescourse_Click;

            var OpenFileButton = GetTemplateChild("OpenFile") as Button;
            OpenFileButton.Click += OpenFileButton_Click;

            var OpenPlayList = GetTemplateChild("OpenPlayList") as AppBarButton;
            OpenPlayList.Click += OpenPlayList_Click;

            var PlayPauseButton = GetTemplateChild("PlayPauseButton") as AppBarButton;
            PlayPauseButton.Click += PlayPauseButton_Click;

            var FullWindowButton = GetTemplateChild("FullWindowButton") as AppBarButton;
            FullWindowButton.Click += FullWindowButton_Click;

            var SettingButton = GetTemplateChild("SettingButton") as AppBarButton;
            SettingButton.Click += SettingButton_Click;

            var VolumeMuteButton = GetTemplateChild("VolumeMuteButton") as AppBarButton;

            VolumeSlider = GetTemplateChild("VolumeSlider") as Slider;
            VolumeSlider.ValueChanged += VolumeSlider_OnValueChange;
            VolumeSlider.PointerWheelChanged += VolumeSlider_PointerWheelChanged;

            ProgressSlider = GetTemplateChild("ProgressSlider") as Slider;
            ProgressSlider.Loaded += ProgressSlider_Loaded;

            PlayListControl = GetTemplateChild("PlayList") as PlayList;

            base.OnApplyTemplate();
        }

        protected override void OnTapped(TappedRoutedEventArgs e)
        {


        }
        #endregion

        #region 内部控件事件

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)//PlayPause
        {
            if (AttachedMediaPlayer.PlaybackSession.PlaybackState.Equals(MediaPlaybackState.Paused))
            {
                AttachedMediaPlayer.Play();

                VisualStateManager.GoToState(this, "PauseState", false);
                ProgressTimer.Start();
            }
            else if (AttachedMediaPlayer.PlaybackSession.PlaybackState.Equals(MediaPlaybackState.Playing))
            {
                AttachedMediaPlayer.Pause();

                VisualStateManager.GoToState(this, "PlayState", false);
                ProgressTimer.Stop();
            }

        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)//Setting
        {
            if (IsAdditionSettingVisible)
                VisualStateManager.GoToState(this, "SettingButton_Normal", false);

            else
                VisualStateManager.GoToState(this, "SettingButton_Active", false);

            IsAdditionSettingVisible = !IsAdditionSettingVisible;
        }

        private void OpenUrlRescourse_Click(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "PlayListHide", false);
            if (IsURLResourcesSearchVisible)
                VisualStateManager.GoToState(this, "URLResources_Search_Hide", false);
            else
                VisualStateManager.GoToState(this, "URLResources_Search_Show", false);

            IsURLResourcesSearchVisible = !IsURLResourcesSearchVisible;

        }

        private void FullWindowButton_Click(object sender, RoutedEventArgs e)//FullWindow
        {
            if (IsFullWindow)
            {
                ApplicationView.GetForCurrentView().ExitFullScreenMode();
                VisualStateManager.GoToState(this, "NonFullWindowState", false);
            }
            else
            {
                ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
                VisualStateManager.GoToState(this, "FullWindowState", false);
            }

            IsFullWindow = !IsFullWindow;
        }

        private void OpenPlayList_Click(object sender, RoutedEventArgs e)//OpenPlayList
        {
            if (PlayListControl.IsPlaylistVisible)
                VisualStateManager.GoToState(this, "PlayListHide", false);
            else
                VisualStateManager.GoToState(this, "PlayListShow", false);
        }

        private void VolumeSlider_OnValueChange(object sender, RangeBaseValueChangedEventArgs e)//VolumeSlider
        {
            if (e.NewValue == 0)
                VisualStateManager.GoToState(this, "MuteState", false);
            else
                VisualStateManager.GoToState(this, "VolumeState", false);
        }


        private void VolumeSlider_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
                ((Slider)sender).Value += e.GetCurrentPoint(null).Properties.MouseWheelDelta / 120;
        }

        private void ProgressSlider_Loaded(object sender, RoutedEventArgs e)//ProgressSlider
        {

        }

        #region 播放事件处理
        private async void OpenFileButton_Click(object sender, RoutedEventArgs args)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.MusicLibrary;
            picker.FileTypeFilter.Add(".mp3");
            picker.FileTypeFilter.Add(".mp4");
            IReadOnlyList<Windows.Storage.StorageFile> file = await picker.PickMultipleFilesAsync();
            if (file.Count != 0)
            {
                NowPlay = file[0].Name;
                var mediasource = MediaSource.CreateFromStorageFile(file[0]);
                mediasource.OpenOperationCompleted += mediasource_OpenOperationCompleted;
                AttachedMediaPlayer.Source = mediasource;
            }
        }

        private async void mediasource_OpenOperationCompleted(MediaSource sender, MediaSourceOpenOperationCompletedEventArgs args)
        {
            var _Span = sender.Duration.GetValueOrDefault();
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                VolumeSlider.Value = 50;
                ProgressSlider.Minimum = 0;
                ProgressSlider.Maximum = _Span.TotalSeconds;
                ProgressSlider.StepFrequency = 1;
            });

        }
        #endregion

        #endregion

        #region 公共方法
        public void SetChildBindings()
        {

            ProgressSlider.SetBinding(Slider.ValueProperty, new Binding()
            {
                Path = new PropertyPath("Position"),
                Source = AttachedMediaPlayer.PlaybackSession,
                Mode = BindingMode.TwoWay,
                Converter = new TimeLineConverter()
            });

            VolumeSlider.SetBinding(Slider.ValueProperty, new Binding()
            {
                Path = new PropertyPath("Volume"),
                Source = AttachedMediaPlayer,
                Mode = BindingMode.TwoWay,
                Converter = new VolumeConverter()
            });
        }

        public void Show()
        {
            IsVisible = true;
            VisualStateManager.GoToState(this, "ControlPanelFadeIn", false);
        }

        public void Hide()
        {
            IsVisible = false;
            VisualStateManager.GoToState(this, "ControlPanelFadeOut", false);
            if (PlayListControl.IsPlaylistVisible)
                VisualStateManager.GoToState(this, "PlayListHide", false);
        }

        public void ShowVolumeBar()
        {
            VisualStateManager.GoToState(this, "VolumeSliderGridShow", false);
            VolumeSliderVisiblityTimer.Start();
        }

        public void HideVolumeBar()
        {
            VisualStateManager.GoToState(this, "VolumeSliderGridHide", false);
            VolumeSliderVisiblityTimer.Stop();
        }

        public void SetVolumeIncrement(double v)
        {
            VolumeSlider.Value += v;
        }
        #endregion

        #region 定时器异步事件
        private void ProgressTimer_Tick(object sender, object e)
        {
            //Debug.WriteLine((double)AttachedMediaPlayer.Volume);
            ProgressSlider.Value = ((TimeSpan)AttachedMediaPlayer.PlaybackSession.Position).TotalSeconds;
            if (ProgressSlider.Value == ProgressSlider.Maximum)
            {
                ProgressSlider.Value = 0;
                AttachedMediaPlayer.Pause();
                VisualStateManager.GoToState(this, "PlayState", false);
                ProgressTimer.Stop();
            }
        }

        private void VolumeSliderVisiblityTimer_Tick(object sender, object e)
        {
            HideVolumeBar();
        }
        #endregion

        public ControlBar()
        {
            this.DefaultStyleKey = typeof(ControlBar);
            this.Loaded += OnLoaded;

            //进度条更新
            ProgressTimer = new DispatcherTimer();
            ProgressTimer.Interval = TimeSpan.FromMilliseconds(500);
            ProgressTimer.Tick += ProgressTimer_Tick;

            VolumeSliderVisiblityTimer = new DispatcherTimer();
            VolumeSliderVisiblityTimer.Interval = TimeSpan.FromSeconds(3);
            VolumeSliderVisiblityTimer.Tick += VolumeSliderVisiblityTimer_Tick;
        }
    }

    public class MediaInfo
    {
        public String FileName { get; set; }
        public DateTime FileLength { get; set; }
        public Image FileScaledImage { get; set; }
    }

    #region Converters
    public class VolumeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return Math.Round((double)value * 100);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return ((double)value / 100.00);
        }
    }

    public class WidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return -(double)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class TimeLineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ((TimeSpan)value).TotalSeconds;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return TimeSpan.FromSeconds((double)value);
        }
    }

    public class StoHMSConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            var hour = (double)value / 3600;
            var min = (double)value % 3600 / 60;
            var sec = (double)value % 3600 % 60;
            return String.Format("{0:00}:{1:00}:{2:00}", hour, min, sec);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    public class SystemInfo : INotifyPropertyChanged //系统信息
    {
        private DateTime time;

        #region 时间元素
        private int hour, min, sec;
        public int Hour
        {
            get { return hour; }
            set
            {
                hour = value;
                OnPropertyChanged("Hour");
            }
        }

        public int Min
        {
            get { return min; }
            set
            {
                min = value;
                OnPropertyChanged("Min");
            }
        }

        public int Sec
        {
            get { return sec; }
            set
            {
                sec = value;
                OnPropertyChanged("Sec");
            }
        }
        #endregion

        private String hourMin;//时间 格式 HH:MM
        public String HourMin
        {
            get { return hourMin; }
            set
            {
                hourMin = value;
                OnPropertyChanged("HourMin");
            }
        }

        private String batteryLevel;//电量 格式 DD%
        public String BatteryLevel
        {
            get { return batteryLevel; }
            set
            {
                batteryLevel = value;
                OnPropertyChanged("BatteryLevel");
            }
        }

        public SystemInfo()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            Battery.AggregateBattery.ReportUpdated += GetBatteryUpdate;
            timer.Tick += UpdateTime;
            timer.Start();
            GetBatteryUpdate(null, null);
            UpdateTime(null, null);
        }

        private void UpdateTime(object sender, object args)
        {
            time = DateTime.Now;
            Hour = time.Hour;
            Min = time.Minute;
            Sec = time.Second;
            HourMin = String.Format("{0:00} : {1:00}", Hour, Min);
        }

        #region 电池数据更新
        private void UpdateBattery(BatteryReport report, string DeviceID)
        {
            double valuem = Convert.ToDouble(report.FullChargeCapacityInMilliwattHours);
            double valuen = Convert.ToDouble(report.RemainingCapacityInMilliwattHours);
            BatteryLevel = String.Format("{0:F0} %", ((valuen / valuem) * 100));
        }

        private void RequestAggregateBatteryReport()
        {
            var aggBattery = Battery.AggregateBattery;
            var report = aggBattery.GetReport();
            UpdateBattery(report, aggBattery.DeviceId);
        }

        private async void GetBatteryUpdate(object sender, object args)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, RequestAggregateBatteryReport);
        }
        #endregion

        #region 属性变化通知
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
