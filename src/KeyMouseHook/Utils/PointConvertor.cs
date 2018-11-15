using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Loamen.KeyMouseHook
{
    public static class PointConvertor
    {
        /// <summary>
        /// 将像素坐标转化为绝对坐标，API中MouseInput结构中的dx，dy含义是绝对坐标，是相对屏幕的而言的，屏幕左上角的坐标为（0,0），右下角的坐标为（65535,65535）。而在C#中获得的对象（Frame，button，flash等）的坐标都是像素坐标，是跟当前屏幕的分辨率相关的。
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Point ToAbsolutePoint(this Point point)
        {
            var CurrentScreenWidth = 65535.0d / Screen.PrimaryScreen.Bounds.Width;
            var CurrentScreenHeight = 65535.0d / Screen.PrimaryScreen.Bounds.Height;
            var X = CurrentScreenWidth * point.X;
            var Y = CurrentScreenHeight * point.Y;
            return new Point((int)X, (int)Y);
        }
    }
}
