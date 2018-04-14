using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace SimpleMediaplayer
{
    public class VolumeMuteStateTrigger : StateTriggerBase
    {
        private static readonly DependencyProperty VloumemuteProperty = DependencyProperty.RegisterAttached("VloumeMute", typeof(int), typeof(VolumeMuteStateTrigger),
            new PropertyMetadata(1, new PropertyChangedCallback(VolumeChanged)));
        public int VloumeMute
        {
            get { return (int)GetValue(VloumemuteProperty); }
            set { SetValue(VloumemuteProperty, value); }
        }
        private static void VolumeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VolumeMuteStateTrigger dat = (VolumeMuteStateTrigger)d;
            dat.SetActive( (int)e.OldValue == 0);
        }
    }


}
