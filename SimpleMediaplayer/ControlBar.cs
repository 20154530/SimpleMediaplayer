using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml.Controls.Primitives;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Power;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Core;
using Windows.ApplicationModel.Core;
using Windows.Media.Core;

namespace SimpleMediaplayer
{

    public sealed class ControlBar : MediaTransportControls
    {
        private ListView PlayListControl;

        #region 正在播放的文件名
        private static readonly DependencyProperty NowPlayProperty = DependencyProperty.RegisterAttached("NowPlay", typeof(String), typeof(ControlBar),
            new PropertyMetadata("No File Now !"));
        public String NowPlay
        {
            get { return (String)this.GetValue(NowPlayProperty); }
            set { this.SetValue(NowPlayProperty, value); }
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

        protected override void OnApplyTemplate()
        {
            var OpenFileButton = GetTemplateChild("OpenFile") as Button;
            OpenFileButton.Click += OpenFileButton_Click;
            var OpenPlayList = GetTemplateChild("OpenPlayList") as Button;
            OpenPlayList.Click += OpenPlayList_Click;
            var VolumSlider = GetTemplateChild("VolumeSlider") as Slider;
            VolumSlider.ValueChanged += VolumSlider_OnValueChange;
            PlayListControl = GetTemplateChild("PlayList") as ListView;
            base.OnApplyTemplate();
        }

        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            base.OnTapped(e);
        }

        private void OpenPlayList_Click(object sender, RoutedEventArgs args)
        {

        }

        private void VolumSlider_OnValueChange(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (e.NewValue == 0)
                VisualStateManager.GoToState(this, "MuteState", true);
            else
                VisualStateManager.GoToState(this, "VolumeState", false);
        }

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
                SystemInfo.MediaRes.MediaPlayer.Source = mediasource;
            }
        }



        public ControlBar()
        {
            this.DefaultStyleKey = typeof(ControlBar);
        }

    }

    public class MediaInfo
    {
        public String FileName { get; set; }
        public DateTime FileLength { get; set; }
        public Image FileScaledImage { get; set; }
    }

    public class SystemInfo : INotifyPropertyChanged //系统信息
    {
        private DateTime time;

        public static SystemInfo MediaRes = new SystemInfo();

        private MediaPlayerElement mediaPlayer;
        public MediaPlayerElement MediaPlayer
        {
            get { return mediaPlayer; }
            set { mediaPlayer = value; }
        }

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
