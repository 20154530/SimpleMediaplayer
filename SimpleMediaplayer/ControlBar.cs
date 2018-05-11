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
using System.Threading.Tasks;
using System.Net.Http;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Storage.Pickers;
using Windows.Storage.AccessCache;
using System.Text;
using Newtonsoft.Json;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Media.Imaging;

namespace SimpleMediaplayer
{
    public sealed class ControlBar : Control
    {
        //NOVisial
        private enum SaveType
        {
            config = 0,//存储设置
            video = 1,
            audio = 2,
        }
        private String SaveLocal = null;
        private String SaveLocal_Path = null;
        private bool IsVolumeBarVisible = false;
        private bool IsAdditionSettingVisible = false;
        private bool IsFullWindow = false;

        private DispatcherTimer ProgressTimer;
        private DispatcherTimer VolumeSliderVisiblityTimer;
        private DispatcherTimer AutoHideTimer;


        //UIElement
        private Slider ProgressSlider;
        private Slider VolumeSlider;
        private TextBox URLResourcesSearchTextBox;
        private SymbolIcon VolumeMutePopupIcon;

        //DependenceProperty
        #region 自动隐藏阻止标签
        public bool CanAutoHide
        {
            get { return (bool)GetValue(CanAutoHideProperty); }
            set { SetValue(CanAutoHideProperty, value); }
        }
        public static readonly DependencyProperty CanAutoHideProperty =
            DependencyProperty.Register("CanAutoHide", typeof(bool), typeof(ControlBar), new PropertyMetadata(true));
        #endregion

        #region 强制软件解码
        public bool ForceSoftwareDecode
        {
            get { return (bool)GetValue(ForceSoftwareDecodeProperty); }
            set { SetValue(ForceSoftwareDecodeProperty, value); }
        }
        public static readonly DependencyProperty ForceSoftwareDecodeProperty =
            DependencyProperty.Register("ForceSoftwareDecode", typeof(bool), typeof(ControlBar), new PropertyMetadata(false));
        #endregion

        #region 可见性
        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }
        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.Register("IsVisible", typeof(bool), typeof(ControlBar), new PropertyMetadata(true));
        #endregion

        #region 文字提示
        private static readonly DependencyProperty NotifyWordsProperty = DependencyProperty.RegisterAttached("NotifyWords", typeof(string), typeof(ControlBar),
            new PropertyMetadata(""));
        public string NotifyWords
        {
            get { return (string)this.GetValue(NotifyWordsProperty); }
            set { this.SetValue(NotifyWordsProperty, value); }
        }
        #endregion

        #region 自动隐藏属性
        private static readonly DependencyProperty AutoHideProperty = DependencyProperty.RegisterAttached("AutoHide", typeof(bool), typeof(ControlBar),
            new PropertyMetadata(false));
        public bool AutoHide
        {
            get { return (bool)this.GetValue(AutoHideProperty); }
            set { this.SetValue(AutoHideProperty, value); }
        }
        #endregion

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

        #region 关联播放列表
        public PlayList AttachedPlayList
        {
            get { return (PlayList)GetValue(AttachedPlayListProperty); }
            set { SetValue(AttachedPlayListProperty, value); }
        }
        public static readonly DependencyProperty AttachedPlayListProperty =
            DependencyProperty.Register("AttachedPlayList", typeof(PlayList), typeof(ControlBar), new PropertyMetadata(null));
        #endregion

        //Motheds
        #region 重写
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "PlayListHide", false);
            VisualStateManager.GoToState(this, "SettingButton_Normal", false);
            VisualStateManager.GoToState(this, "URLResources_Search_Hide", false);
            AttachedPlayList.AttachedControlBar = this;
            IsVisible = true;
        }

        protected override void OnApplyTemplate()
        {
            #region onlyEvent
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

            var URLResources_Search_Icon = GetTemplateChild("URLResources_Search_Icon") as Button;
            URLResources_Search_Icon.Click += URLResources_Search_Icon_Click;

            var URLResources_Search_Online = GetTemplateChild("URLResources_Search_Online") as Button;
            URLResources_Search_Online.Click += URLResources_Search_Online_Click;

            var URLResources_Search_PreLoad = GetTemplateChild("URLResources_Search_PreLoad") as Button;
            URLResources_Search_PreLoad.Click += URLResources_Search_PreLoad_CLick;

            var SaveLocal = GetTemplateChild("MediaControlsCommandBar_Additionals_SaveLocal") as DoubleClickButton;
            SaveLocal.Tapped += SaveLocal_Click;
            #endregion

            #region needReference
            VolumeMutePopupIcon = GetTemplateChild("VolumeMutePopup_Icon") as SymbolIcon;

            URLResourcesSearchTextBox = GetTemplateChild("URLResources_Search_TextBox") as TextBox;

            VolumeSlider = GetTemplateChild("VolumeSlider") as Slider;
            VolumeSlider.ValueChanged += VolumeSlider_OnValueChange;
            VolumeSlider.PointerMoved += VolumeSlider_PointerMoved;

            ProgressSlider = GetTemplateChild("ProgressSlider") as Slider;
            ProgressSlider.Loaded += ProgressSlider_Loaded;

            AttachedPlayList = GetTemplateChild("PlayList") as PlayList;
            #endregion

            base.OnApplyTemplate();
        }
        #endregion

        #region 内部控件事件

        //播放、暂停
        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)//PlayPause
        {
            if (AttachedMediaPlayer.Source != null)
                switch (AttachedMediaPlayer.PlaybackSession.PlaybackState)
                {
                    case MediaPlaybackState.Paused:
                        AttachedMediaPlayer.Play();
                        VisualStateManager.GoToState(this, "PauseState", false);
                        ProgressTimer.Start();
                        AutoHideControlBar();
                        break;
                    case MediaPlaybackState.Playing:
                        AttachedMediaPlayer.Pause();
                        VisualStateManager.GoToState(this, "PlayState", false);
                        ProgressTimer.Stop();
                        break;
                }
        }

        //更改文件位置
        private async void SaveLocal_Click(object sender, TappedRoutedEventArgs e)
        {
            FolderPicker pick = new FolderPicker();
            pick.FileTypeFilter.Add(".mp3");
            pick.FileTypeFilter.Add(".mp4");
            pick.FileTypeFilter.Add(".wmv");
            StorageFolder folder = await pick.PickSingleFolderAsync();

            if (folder != null)
            {
                SaveLocal_Path = folder.Path;
                SaveLocal = StorageApplicationPermissions.FutureAccessList.Add(folder);
            }

            if (SaveLocal != null)
                SavePathToLocal();
        }

        //设置按钮
        private void SettingButton_Click(object sender, RoutedEventArgs e)//Setting
        {
            if (IsAdditionSettingVisible)
                VisualStateManager.GoToState(this, "SettingButton_Normal", false);
            else
                VisualStateManager.GoToState(this, "SettingButton_Active", false);

            IsAdditionSettingVisible = !IsAdditionSettingVisible;
        }

        //打开在线播放输入框
        private void OpenUrlRescourse_Click(object sender, RoutedEventArgs e)//OpenUrlRescourse
        {
            ForcenotAutohide();
            VisualStateManager.GoToState(this, "PlayListHide", false);
            VisualStateManager.GoToState(this, "URLResources_Search_Show", false);
        }

        //全屏播放
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

        //打开播放列表
        private void OpenPlayList_Click(object sender, RoutedEventArgs e)//OpenPlayList
        {
            if (AttachedPlayList.IsPlaylistVisible)
            {
                TryAutoHide();
                VisualStateManager.GoToState(this, "PlayListHide", false);
            }
            else
            {
                ForcenotAutohide();
                VisualStateManager.GoToState(this, "PlayListShow", false);
            }
        }

        //收起在线播放栏
        private void URLResources_Search_Icon_Click(object sender, RoutedEventArgs e)//URLResources_Search_Icon
        {
            TryAutoHide();
            VisualStateManager.GoToState(this, "URLResources_Search_Hide", false);
        }

        //音量条值变化
        private void VolumeSlider_OnValueChange(object sender, RangeBaseValueChangedEventArgs e)//VolumeSlider
        {
            if (e.NewValue == 0 || e.OldValue == 0)
            {
                VisualStateManager.GoToState(this, "MutePopupHide", false);
                VolumeMutePopupIcon.Symbol = VolumeSlider.Value == 0 ? Symbol.Mute : Symbol.Volume;
                VisualStateManager.GoToState(this, "MutePopupShow", false);
            }
        }

        //音量条指针行为
        private void VolumeSlider_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            ShowVolumeBar();
        }

        //进度条加载
        private void ProgressSlider_Loaded(object sender, RoutedEventArgs e)//ProgressSlider
        {

        }

        #region 播放事件处理
        //打开文件
        private async void OpenFileButton_Click(object sender, RoutedEventArgs args)
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".mp3");
            picker.FileTypeFilter.Add(".mp4");
            picker.FileTypeFilter.Add(".wmv");
            picker.FileTypeFilter.Add(".flv");
            picker.FileTypeFilter.Add(".mkv");
            IReadOnlyList<StorageFile> file = await picker.PickMultipleFilesAsync();
            if (file.Count != 0)
            {
                NowPlay = file[0].Name;
                var NowPlaySource = MediaSource.CreateFromStorageFile(file[0]);
                CreatList(file);
                NowPlaySource.OpenOperationCompleted += Mediasource_OpenOperationCompleted;
                AttachedMediaPlayer.Source = NowPlaySource;
            }
        }

        //在线播放URL
        private void URLResources_Search_Online_Click(object sender, RoutedEventArgs args)
        {
            try
            {
                var NowPlaySource = MediaSource.CreateFromUri(new Uri(URLResourcesSearchTextBox.Text.ToString()));
                NowPlaySource.OpenOperationCompleted += Mediasource_OpenOperationCompleted;
                AttachedMediaPlayer.Source = NowPlaySource;
            }
            catch (Exception)
            {
                WordsNotifyPopupAni("URL is not a MediaFile,or type is not support!");
            }
        }

        //播放并下载URL
        private void URLResources_Search_PreLoad_CLick(object sender, RoutedEventArgs args)
        {
            try
            {
                var NowPlaySource = MediaSource.CreateFromUri(new Uri(URLResourcesSearchTextBox.Text.ToString()));
                NowPlaySource.OpenOperationCompleted += Mediasource_OpenOperationCompleted;
                DownloadFile(URLResourcesSearchTextBox.Text.ToString());
                AttachedMediaPlayer.Source = NowPlaySource;
            }
            catch (Exception)
            {
                WordsNotifyPopupAni("URL is not a MediaFile,or type is not support!");
            }
        }

        //打开后连带动作
        private async void Mediasource_OpenOperationCompleted(MediaSource sender, MediaSourceOpenOperationCompletedEventArgs args)
        {
            var _Span = sender.Duration.GetValueOrDefault();
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                VisualStateManager.GoToState(this, "PlayState", false);
                ProgressTimer.Stop();
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
            AutoHideControlBar();
            VisualStateManager.GoToState(this, "ControlPanelFadeIn", false);
        }

        public void Hide()
        {
            IsVisible = false;
            VisualStateManager.GoToState(this, "ControlPanelFadeOut", false);
            if (AttachedPlayList.IsPlaylistVisible)
                VisualStateManager.GoToState(this, "PlayListHide", false);
            AutoHideTimer.Stop();
        }

        public void SetSource(MediaSource m)
        {
            m.OpenOperationCompleted += Mediasource_OpenOperationCompleted;
            AttachedMediaPlayer.Source = m;
        }

        public void ShowVolumeBar()
        {
            if (!IsVolumeBarVisible)
            {
                IsVolumeBarVisible = true;
                VisualStateManager.GoToState(this, "VolumeSliderGridShow", false);
            }
            VolumeSliderVisiblityTimer.Start();
        }

        public void HideVolumeBar()
        {
            IsVolumeBarVisible = false;
            VisualStateManager.GoToState(this, "VolumeSliderGridHide", false);
            VolumeSliderVisiblityTimer.Stop();
        }

        public void SetVolumeIncrement(double v)
        {
            VolumeSlider.Value += v;
        }
        #endregion

        #region 私有功能方法
        //改变保存路径
        private async void SavePathToLocal()
        {
            UserdefaultSettings settings = new UserdefaultSettings
            {
                Savelocal = SaveLocal.ToString()
            };

            string s = JsonConvert.SerializeObject(settings);
            await WriteToFile("UserSettings.dat", Encoding.Unicode.GetBytes(s), SaveType.config);
        }

        //下载文件
        private async void DownloadFile(String Uri)
        {
            Task t = new Task(() =>
            {
                try
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        using (HttpResponseMessage response = httpClient.GetAsync(new Uri(Uri)).Result)
                        {
                            var filename = Uri.Split('/');
                            WriteToFile(filename[filename.Length - 1], response.Content.ReadAsByteArrayAsync().Result, SaveType.video).Wait();
                        }
                    }
                }
                catch (Exception)
                {
                    WordsNotifyPopupAni("Please Check Your Web Connection ");
                }
            });
            t.Start();
            await t;
        }

        //检查是否有已存路径
        private async void CheckSaveloc()
        {
            byte[] bt = await ReadfromFile("UserSettings.dat");
            if (bt != null)
            {
                string f = Encoding.Unicode.GetString(bt);
                if (f != null)
                {
                    UserdefaultSettings s = JsonConvert.DeserializeObject<UserdefaultSettings>(f);
                    SaveLocal = s.Savelocal;
                }
            }
        }

        //写入文件
        private async Task WriteToFile(string fileName, byte[] file, SaveType type)
        {
            try
            {
                StorageFolder folder = null;
                switch (type)
                {
                    case SaveType.config:  folder = ApplicationData.Current.LocalFolder;
                        break;
                    case SaveType.audio:
                        folder = KnownFolders.MusicLibrary;
                        if (SaveLocal != null)
                            folder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(SaveLocal);
                        break;
                    case SaveType.video:
                        folder = KnownFolders.VideosLibrary;
                        if (SaveLocal != null)
                            folder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(SaveLocal);
                        break;
                }
                //Debug.WriteLine(folder.Path);
                StorageFile f = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                using (StorageStreamTransaction x = await f.OpenTransactedWriteAsync())
                {
                    using (DataWriter w = new DataWriter(x.Stream))
                    {
                        w.WriteBytes(file);
                        x.Stream.Size = await w.StoreAsync();
                        await x.CommitAsync();
                    }
                }
                switch (type)
                {
                    case SaveType.config:
                        WordsNotifyPopupAni(String.Format("Changed save location to : \"{0} \"", SaveLocal_Path));
                        break;
                    default:
                        WordsNotifyPopupAni(String.Format(" {0} has been saved to : \"{1} \"", fileName, SaveLocal_Path));
                        break;
                }
            }
            catch (Exception)
            {
                switch (type)
                {
                    case SaveType.config:
                        WordsNotifyPopupAni(String.Format("Changed save location to : \"{0} \" failed", SaveLocal_Path)); break;
                    default:
                        WordsNotifyPopupAni(String.Format(" Failed to save {0} to : \"{1} \"", fileName, SaveLocal_Path));
                        break;
                }
            }
        }

        //读取文件
        private async Task<byte[]> ReadfromFile(string fileName)
        {
            StorageFolder folderLocal = ApplicationData.Current.LocalFolder;
            StorageFile file = (StorageFile)await folderLocal.TryGetItemAsync(fileName);
            if (file != null)
            {
                IBuffer ibuffer = await FileIO.ReadBufferAsync(file);
                using (DataReader read = DataReader.FromBuffer(ibuffer))
                {
                    byte[] s = new byte[read.UnconsumedBufferLength];
                    read.ReadBytes(s);
                    return s;
                }
            }
            return null;
        }

        //提示弹框
        private async void WordsNotifyPopupAni(string notify)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
             {                 
                 ForcenotAutohide();
                 NotifyWords = notify;                 
                 VisualStateManager.GoToState(this, "WordsNotifyPopupHide", false);
                 VisualStateManager.GoToState(this, "WordsNotifyPopupShow", false);
             });
        }

        //软解
        private void UseCore()
        {

        }

        //创建播放列表
        private async void CreatList(IReadOnlyList<StorageFile> file)
        {
            List<MediaInfo> infolist = new List<MediaInfo>();

            foreach(StorageFile f in file)
            {
                MediaInfo m = new MediaInfo();
                var thumbnail = await f.GetScaledImageAsThumbnailAsync(ThumbnailMode.VideosView);
                BitmapImage bitmapImage = new BitmapImage();
                using (InMemoryRandomAccessStream randomAccessStream = new InMemoryRandomAccessStream())
                {
                    await RandomAccessStream.CopyAsync(thumbnail, randomAccessStream);
                    randomAccessStream.Seek(0);
                    bitmapImage.SetSource(randomAccessStream);
                    m.FileScaledImage = bitmapImage;
                }
                m.FileAccess = f;
                m.FileName = f.Name;
                AttachedPlayList.FileList.Add(m);
            }
          //  AttachedPlayList.ItemsSource = AttachedPlayList.FileList;
        }
        #endregion

        #region 定时器异步事件
        //进度条更新
        private void ProgressTimer_Tick(object sender, object e)
        {
            ProgressSlider.Value = ((TimeSpan)AttachedMediaPlayer.PlaybackSession.Position).TotalSeconds;
            if (ProgressSlider.Value == ProgressSlider.Maximum)
            {
                ProgressSlider.Value = 0;
                AttachedMediaPlayer.Pause();
                VisualStateManager.GoToState(this, "PlayState", false);
                ProgressTimer.Stop();
            }
        }

        //音量条隐藏
        private void VolumeSliderVisiblityTimer_Tick(object sender, object e)
        {
            HideVolumeBar();
        }

        //控制栏隐藏
        private void AutoHideControlBar()
        {
            AutoHideTimer.Start();
        }
        private void AutoHideTimer_Tick(object sender, object e)
        {
            if (AutoHide && AttachedMediaPlayer.PlaybackSession.PlaybackState.Equals(MediaPlaybackState.Playing) &&
               CanAutoHide)
                Hide();
        }

        //尝试隐藏控制栏
        private void TryAutoHide()
        {
            CanAutoHide = true;
            AutoHideControlBar();
        }

        //尝试停止隐藏控制栏
        private void ForcenotAutohide()
        {
            CanAutoHide = false;
            AutoHideTimer.Stop();
        }
        #endregion

        public ControlBar()
        {
            this.DefaultStyleKey = typeof(ControlBar);
            this.Loaded += OnLoaded;
            CheckSaveloc();

            //进度条更新
            ProgressTimer = new DispatcherTimer{
                Interval = TimeSpan.FromMilliseconds(500)
            };
            ProgressTimer.Tick += ProgressTimer_Tick;

            //音量条隐藏
            VolumeSliderVisiblityTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            VolumeSliderVisiblityTimer.Tick += VolumeSliderVisiblityTimer_Tick;

            //自动隐藏计时
            AutoHideTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
            AutoHideTimer.Tick += AutoHideTimer_Tick;
        }
    }

    public class UserdefaultSettings
    {
        public String Savelocal { get; set; }


        public UserdefaultSettings()
        {

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
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
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
