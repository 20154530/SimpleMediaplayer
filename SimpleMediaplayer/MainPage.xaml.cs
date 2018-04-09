using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Media.Playback;
using System.Diagnostics;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Sensors;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

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
            SetOrientation();     
            this.InitializeComponent();
            setMediaPlayer();
        }

        private void setMediaPlayer()
        {
            _PlayerCore = new MediaPlayer();
            _PlayerCore.CommandManager.IsEnabled = false;
            _PlayerCore.TimelineController = new Windows.Media.MediaTimelineController();
            MediaPlayer.SetMediaPlayer(_PlayerCore);
            MediaPlayer_ControlBar.SetMediaPlayer(_PlayerCore);
        }

        private async void CheckStatusBar()
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                _Statusbar = StatusBar.GetForCurrentView();
                await _Statusbar.HideAsync();
            }
        }

        private void SetOrientation()
        {
            _Simpleorientationsensor = SimpleOrientationSensor.GetDefault();
            if (_Simpleorientationsensor != null)
            {
                _Simpleorientationsensor.OrientationChanged += new TypedEventHandler<SimpleOrientationSensor, SimpleOrientationSensorOrientationChangedEventArgs>(OrientationChanged);
            }
        }

        private async void OrientationChanged(object sender, SimpleOrientationSensorOrientationChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                SimpleOrientation orientation = e.Orientation;
                switch (orientation)
                {
                    case SimpleOrientation.NotRotated:

                        break;
                    case SimpleOrientation.Rotated90DegreesCounterclockwise:

                        break;
                    case SimpleOrientation.Rotated180DegreesCounterclockwise:

                        break;
                    case SimpleOrientation.Rotated270DegreesCounterclockwise:

                        break;
                    case SimpleOrientation.Faceup:

                        break;
                    case SimpleOrientation.Facedown:

                        break;
                    default:

                        break;
                }
            });
        }

    }
}
