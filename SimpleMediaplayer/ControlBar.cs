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

namespace SimpleMediaplayer
{

    public sealed class ControlBar : Control
    {
        private PlayList PlayListControl;
        private CommandBar MediaControlsCommandBar;

        #region 正在播放的文件名
        private static readonly DependencyProperty NowPlayProperty = DependencyProperty.RegisterAttached("NowPlay", typeof(String), typeof(ControlBar),
            new PropertyMetadata("No File Now !"));
        public String NowPlay
        {
            get { return (String)this.GetValue(NowPlayProperty); }
            set { this.SetValue(NowPlayProperty, value); }
        }
        #endregion

        #region 关联播放器
        private static readonly DependencyProperty AttachedMediaPlayerProperty = DependencyProperty.RegisterAttached("AttachedMediaPlayer", typeof(MediaPlayer), typeof(ControlBar),
            new PropertyMetadata(null));
        public MediaPlayer AttachedMediaPlayer
        {
            get { return (MediaPlayer)this.GetValue(AttachedMediaPlayerProperty); }
            set { this.SetValue(AttachedMediaPlayerProperty, value); }
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
           
        }

        protected override void OnApplyTemplate()
        {
            var OpenFileButton = GetTemplateChild("OpenFile") as Button;
            OpenFileButton.Click += OpenFileButton_Click;

            var OpenPlayList = GetTemplateChild("OpenPlayList") as Button;
            OpenPlayList.Click += OpenPlayList_Click;

            var VolumSlider = GetTemplateChild("VolumeSlider") as Slider;
            VolumSlider.ValueChanged += VolumSlider_OnValueChange;
            VolumSlider.Loaded += VolumSlider_OnLoaded;

            var PlayPauseButton = GetTemplateChild("PlayPauseButton") as AppBarButton;
            PlayPauseButton.Click += PlayPauseButton_Click;

            MediaControlsCommandBar = GetTemplateChild("MediaControlsCommandBar") as CommandBar;

            PlayListControl = GetTemplateChild("PlayList") as PlayList;

            base.OnApplyTemplate();
        }

        protected override void OnTapped(TappedRoutedEventArgs e)
        {


        }
        #endregion

        #region PlayPause
        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if(AttachedMediaPlayer.TimelineController.State.Equals(Windows.Media.MediaTimelineControllerState.Paused))
            {
                
            }
            
        }
        #endregion

        #region ControlBlank

        #endregion

        #region PlayList
        private void OpenPlayList_Click(object sender, RoutedEventArgs e)
        {
            if (PlayListControl.IsPlaylistVisible)
                VisualStateManager.GoToState(this, "PlayListHide", false);
            else
                VisualStateManager.GoToState(this, "PlayListShow", false);

            PlayListControl.IsPlaylistVisible = !PlayListControl.IsPlaylistVisible;
        }
        #endregion

        #region VolumSlider
        private void VolumSlider_OnLoaded(object sender, RoutedEventArgs e)
        {
            (sender as Slider).Value = 50;
        }

        private void VolumSlider_OnValueChange(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (e.NewValue == 0)
                VisualStateManager.GoToState(this, "MuteState", false);
            else
                VisualStateManager.GoToState(this, "VolumeState", false);
        }
        #endregion

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
               
            }
        }

        public void SetMediaPlayer (MediaPlayer player)
        {
            AttachedMediaPlayer = player;
        }

        public ControlBar()
        {
            this.DefaultStyleKey = typeof(ControlBar);
            this.Loaded += OnLoaded;
        }

    }

    public class MediaInfo
    {
        public String FileName { get; set; }
        public DateTime FileLength { get; set; }
        public Image FileScaledImage { get; set; }
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
