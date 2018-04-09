using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SimpleMediaplayer
{
    public class SidePanel : Panel //依据控制栏分隔符将组件均匀分散开布局
    {
        private Size SetSize;
        private int Sets;
        private int InnerElements;

        protected override Size MeasureOverride(Size availableSize)
        {
            Sets = 0;
            InnerElements = 0;
            SetSize = new Size(0, 0);
            foreach (var child in Children)
            {
                if (child is AppBarSeparator)
                    Sets++;
                else if (Sets == 1 && child.Visibility.Equals(Visibility))
                    InnerElements++;

                child.Measure(availableSize);
                SetSize.Height = child.DesiredSize.Height > SetSize.Height ? child.DesiredSize.Height : SetSize.Height;
            }

           
            if (availableSize.Width == Double.PositiveInfinity)
                return SetSize;
            else
                SetSize.Width = availableSize.Width / (Sets + 1);

            return new Size(availableSize.Width, SetSize.Height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double x = 0;
            double Sepx = SetSize.Height;
            int Setn = 0;
            foreach (var child in Children)
            {
                if (child is AppBarSeparator)
                {
                    child.Arrange(new Rect(Sepx, 0, 0, 0));
                    Sepx += Sepx;
                    Setn++;
                    if (Setn == Sets)
                        x = finalSize.Width - SetSize.Height;
                    else
                    {
                        x = (finalSize.Width - InnerElements * SetSize.Height) / 2;
                        Debug.WriteLine(finalSize);
                        Debug.WriteLine(x);
                    }
                }
                else
                {
                    if (Setn == 0)
                    {
                        child.Arrange(new Rect(x, 0, child.DesiredSize.Width, child.DesiredSize.Height));
                        x += child.DesiredSize.Width;
                    }
                    else if (Setn == Sets)
                    {
                        child.Arrange(new Rect(x, 0, child.DesiredSize.Width, child.DesiredSize.Height));
                        x -= child.DesiredSize.Width;
                    }
                    else
                    {
                        child.Arrange(new Rect(x, 0, child.DesiredSize.Width, child.DesiredSize.Height));
                        x += child.DesiredSize.Width;
                    }

                }
            }
            return finalSize;
        }
    }
}
