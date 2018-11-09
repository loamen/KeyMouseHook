using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Loamen.KeyMouseHook
{
    /// <summary>
    /// Series of events that can be recorded any played back
    /// </summary>
    [Serializable]
    public class MacroEvent : EventArgs
    {
        public MacroEventType KeyMouseEventType;
        public EventArgs EventArgs;
        public int TimeSinceLastEvent;

        public MacroEvent(MacroEventType eventType, EventArgs eventArgs, int timeSinceLastEvent)
        {
            KeyMouseEventType = eventType;
            EventArgs = eventArgs;
            TimeSinceLastEvent = timeSinceLastEvent;
        }
    }

    [Serializable]
    [FlagsAttribute]
    public enum MacroEventType
    {
        MouseMove = 0,
        MouseMoveExt = 1,
        MouseDown = 2,
        MouseDownExt = 4,
        MouseUp = 8,
        MouseUpExt = 16,
        MouseWheel = 32,
        MouseWheelExt = 64,
        MouseDragStarted = 128,
        MouseDragFinished = 256,
        MouseDoubleClick = 512,
        KeyDown = 1024,
        KeyUp = 2048,
        KeyPress = 4096
    }

    public static class PointConvertor
    {
        /// <summary>
        /// 将像素坐标转化为绝对坐标，API中MouseInput结构中的dx，dy含义是绝对坐标，是相对屏幕的而言的，屏幕左上角的坐标为（0,0），右下角的坐标为（65535,65535）。而在C#中获得的对象（Frame，button，flash等）的坐标都是像素坐标，是跟当前屏幕的分辨率相关的。
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Point ToAbsolutePoint(this Point point)
        {
            var CurrentScreenWidth = 65535.0d / Screen.PrimaryScreen.WorkingArea.Width;
            var CurrentScreenHeight = 65535.0d / Screen.PrimaryScreen.WorkingArea.Height;
            var X = CurrentScreenWidth * point.X;
            var Y = CurrentScreenHeight * point.Y;
            return new Point((int)X, (int)Y);
        }
    }
}
