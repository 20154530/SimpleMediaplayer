using System;
using System.Collections.Generic;
using System.ComponentModel;
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


namespace SimpleMediaplayer
{

    public sealed class ControlBar : MediaTransportControls
    {
        #region 正在播放的文件名
        private static readonly DependencyProperty FilenameProperty = DependencyProperty.RegisterAttached("Filename", typeof(String), typeof(ControlBar),
            new PropertyMetadata("No File Now !"));
        public String Filename
        {
            get { return (String)this.GetValue(FilenameProperty); }
            set { this.SetValue(FilenameProperty, value); }
        }
        #endregion


        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            base.OnTapped(e);
        //    this.Visibility = this.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        public ControlBar()
        {
            this.DefaultStyleKey = typeof(ControlBar);
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

        private String batteryLevel;//电量 格式 DD.D%
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
