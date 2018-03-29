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

        #region 旋转角度
        private static readonly DependencyProperty RotateAngleProperty = DependencyProperty.RegisterAttached("RotateAngle", typeof(double), typeof(PathButton),
        new PropertyMetadata(0));
        public Double RotateAngle
        { 
            get { return (Double)GetValue(RotateAngleProperty); }
            set { SetValue(RotateAngleProperty, value); }
        }
        #endregion

        #region 正常图标
        private static readonly DependencyProperty ShapeNProperty = DependencyProperty.RegisterAttached("ShapeN", typeof(String), typeof(PathButton),
            new PropertyMetadata(""));
        public String ShapeN
        {
            get { return (String)GetValue(ShapeNProperty); }
            set { SetValue(ShapeNProperty, value);
                SetValue(ShapeRProperty, value);
                SetValue(ShapePProperty, value);
                SetValue(ShapeDProperty, value);
            }
        }
        #endregion

        #region 正常填充
        private static readonly DependencyProperty FillNProperty = DependencyProperty.RegisterAttached("FillN", typeof(Brush), typeof(PathButton),
            new PropertyMetadata(""));
        public Brush FillN
        {
            get { return (Brush)GetValue(FillNProperty); }
            set { SetValue(FillNProperty, value);
                SetValue(FillRProperty, value);
                SetValue(FillPProperty, value);
                SetValue(FillDProperty, value);
            }
        }
        #endregion

        #region 正常描边
        private static readonly DependencyProperty StrokeNProperty = DependencyProperty.RegisterAttached("StrokeN", typeof(Brush), typeof(PathButton),
        new PropertyMetadata(null));
        public Brush StrokeN
        {
            get { return (Brush)GetValue(StrokeNProperty); }
            set { SetValue(StrokeNProperty, value); }
        }
        #endregion

        #region 悬停图标
        private static readonly DependencyProperty ShapeRProperty = DependencyProperty.RegisterAttached("ShapeR", typeof(String), typeof(PathButton),
            new PropertyMetadata("M 30 30 L 70 70 M 70 30 L 30 70 "));
        public String ShapeR
        {
            get { return (String)GetValue(ShapeRProperty); }
            set { SetValue(ShapeRProperty, value); }
        }
        #endregion

        #region 悬停填充
        private static readonly DependencyProperty FillRProperty = DependencyProperty.RegisterAttached("FillR", typeof(Brush), typeof(PathButton),
            new PropertyMetadata(null));
        public Brush FillR
        {
            get { return (Brush)GetValue(FillRProperty); }
            set { SetValue(FillRProperty, value); }
        }
        #endregion

        #region 悬停描边
        private static readonly DependencyProperty StrokeRProperty = DependencyProperty.RegisterAttached("StrokeR", typeof(Brush), typeof(PathButton),
        new PropertyMetadata(null));
        public Brush StrokeR
        {
            get { return (Brush)GetValue(StrokeRProperty); }
            set { SetValue(StrokeRProperty, value); }
        }
        #endregion

        #region 按下图标
        private static readonly DependencyProperty ShapePProperty = DependencyProperty.RegisterAttached("ShapeP", typeof(String), typeof(PathButton),
            new PropertyMetadata("M 30 30 L 70 70 M 70 30 L 30 70 "));
        public String ShapeP
        {
            get { return (String)GetValue(ShapePProperty); }
            set { SetValue(ShapePProperty, value); }
        }
        #endregion

        #region 按下填充
        private static readonly DependencyProperty FillPProperty = DependencyProperty.RegisterAttached("FillP", typeof(Brush), typeof(PathButton),
            new PropertyMetadata(null));
        public Brush FillP
        {
            get { return (Brush)GetValue(FillPProperty); }
            set { SetValue(FillPProperty, value); }
        }
        #endregion

        #region 按下描边
        private static readonly DependencyProperty StrokePProperty = DependencyProperty.RegisterAttached("StrokeP", typeof(Brush), typeof(PathButton),
        new PropertyMetadata(null));
        public Brush StrokeP
        {
            get { return (Brush)GetValue(StrokePProperty); }
            set { SetValue(StrokePProperty, value); }
        }
        #endregion

        #region 不可用图标
        private static readonly DependencyProperty ShapeDProperty = DependencyProperty.RegisterAttached("ShapeD", typeof(String), typeof(PathButton),
            new PropertyMetadata("M 30 30 L 70 70 M 70 30 L 30 70 "));
        public String ShapeD
        {
            get { return (String)GetValue(ShapeDProperty); }
            set { SetValue(ShapeDProperty, value); }
        }
        #endregion

        #region 不可用填充
        private static readonly DependencyProperty FillDProperty = DependencyProperty.RegisterAttached("FillD", typeof(Brush), typeof(PathButton),
            new PropertyMetadata(null));
        public Brush FillD
        {
            get { return (Brush)GetValue(FillDProperty); }
            set { SetValue(FillDProperty, value); }
        }
        #endregion

        #region 不可用描边
        private static readonly DependencyProperty StrokeDProperty = DependencyProperty.RegisterAttached("StrokeD", typeof(Brush), typeof(PathButton),
        new PropertyMetadata(null));
        public Brush StrokeD
        {
            get { return (Brush)GetValue(StrokeDProperty); }
            set { SetValue(StrokeDProperty, value); }
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
