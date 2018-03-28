using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace SimpleMediaplayer
{
    public sealed class PathButton : Button //路径按钮 使用path路径 或 point 绘制的矢量按钮，不失真
    {

        #region 描边粗细
        private static readonly DependencyProperty StrokeSizeProperty = DependencyProperty.RegisterAttached("StrokeSize", typeof(double), typeof(PathButton),
            new PropertyMetadata(2));
        public Double StrokeSize
        {
            get { return (Double)GetValue(StrokeSizeProperty); }
            set { SetValue(StrokeSizeProperty, value); }
        }
        #endregion

        #region 正常图标
        private static readonly DependencyProperty ShapeNProperty = DependencyProperty.RegisterAttached("ShapeN", typeof(String), typeof(PathButton),
            new PropertyMetadata(""));
        public String ShapeN
        {
            get { return (String)GetValue(ShapeNProperty); }
            set { SetValue(ShapeNProperty, value); }
        }
        #endregion

        #region 正常填充
        private static readonly DependencyProperty FillNProperty = DependencyProperty.RegisterAttached("ShapeN", typeof(Brush), typeof(PathButton),
            new PropertyMetadata(new SolidColorBrush()));
        public Brush FillN
        {
            get { return (Brush)GetValue(FillNProperty); }
            set { SetValue(FillNProperty, value); }
        }
        #endregion

        #region 正常描边
        private static readonly DependencyProperty StrokeNProperty = DependencyProperty.RegisterAttached("ShapeN", typeof(Brush), typeof(PathButton),
            new PropertyMetadata(""));
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeNProperty); }
            set { SetValue(StrokeNProperty, value); }
        }
        #endregion

        #region 悬停图标
        private static readonly DependencyProperty ShapeRProperty = DependencyProperty.RegisterAttached("ShapeR", typeof(String), typeof(PathButton),
            new PropertyMetadata(""));
        public String ShapeR
        {
            get { return (String)GetValue(ShapeRProperty); }
            set { SetValue(ShapeRProperty, value); }
        }
        #endregion

        #region 按下图标
        private static readonly DependencyProperty ShapePProperty = DependencyProperty.RegisterAttached("ShapeP", typeof(String), typeof(PathButton),
            new PropertyMetadata(""));
        public String ShapeP
        {
            get { return (String)GetValue(ShapePProperty); }
            set { SetValue(ShapePProperty, value); }
        }
        #endregion



        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            base.OnTapped(e);
            e.Handled = true;
        }

        public PathButton()
        {
            this.DefaultStyleKey = typeof(PathButton);
        }
    }
}
