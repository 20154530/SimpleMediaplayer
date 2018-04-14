using System;
using Windows.Foundation;
using Windows.Media.Playback;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Sensors;

using Windows.UI.Core;
using Windows.ApplicationModel.Core;


namespace SimpleMediaplayer
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private StatusBar _Statusbar;
        private SimpleOrientationSensor _Simpleorientationsensor;
        private MediaPlayer _PlayerCore;

        public MainPage()
        {
            CheckStatusBar();
            //SetOrientation();
            this.InitializeComponent();
            this.Loaded += RootPage_Loaded;
            
        }

        private async void CheckStatusBar()
        {
            
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                _Statusbar = StatusBar.GetForCurrentView();
                await _Statusbar.HideAsync();
            }
            else
            {
                CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = false;
            }
         
        }

        //private void SetOrientation()
        //{
        //    _Simpleorientationsensor = SimpleOrientationSensor.GetDefault();
        //    if (_Simpleorientationsensor != null)
        //    {
        //        _Simpleorientationsensor.OrientationChanged += new TypedEventHandler<SimpleOrientationSensor, SimpleOrientationSensorOrientationChangedEventArgs>(OrientationChanged);
        //    }
        //}

        //private async void OrientationChanged(object sender, SimpleOrientationSensorOrientationChangedEventArgs e)
        //{
        //    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
        //    {
        //        SimpleOrientation orientation = e.Orientation;
        //        switch (orientation)
        //        {
        //            case SimpleOrientation.NotRotated:

        //                break;
        //            case SimpleOrientation.Rotated90DegreesCounterclockwise:

        //                break;
        //            case SimpleOrientation.Rotated180DegreesCounterclockwise:

        //                break;
        //            case SimpleOrientation.Rotated270DegreesCounterclockwise:

        //                break;
        //            case SimpleOrientation.Faceup:

        //                break;
        //            case SimpleOrientation.Facedown:

        //                break;
        //            default:

        //                break;
        //        }
        //    });
        //}

        private void RootPage_Loaded(object sender, RoutedEventArgs e)
        {
            SetMediaPlayer();
            _MediaPlayer_ControlBar.SetChildBindings();
        }

        private void SetMediaPlayer()//Init MediaPlayer
        {
            _PlayerCore = new MediaPlayer();
            _PlayerCore.CommandManager.IsEnabled = false;
            _MediaPlayer.SetMediaPlayer(_PlayerCore);
            _MediaPlayer.AttachedControlbar = _MediaPlayer_ControlBar;
            _MediaPlayer_ControlBar.AttachedMediaPlayer = _PlayerCore;
        }

    }
}
