using Loamen.KeyMouseHook.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace Loamen.KeyMouseHook
{
    /// <summary>
    /// Implements the <see cref="IInputSimulator"/> interface to simulate Keyboard and Mouse input and provide the state of those input devices.
    /// </summary>
    public class InputSimulator : IInputSimulator
    {
        public event EventHandler<MacroEvent> OnPlayback;
        public CancellationTokenSource CancelTokenSource { get; set; }
        private MacroEventType macroEventTypes = MacroEventType.KeyDown | MacroEventType.KeyUp | MacroEventType.MouseDown | MacroEventType.MouseUp | MacroEventType.MouseMove | MacroEventType.MouseWheel;
        /// <summary>
        /// The <see cref="IKeyboardSimulator"/> instance to use for simulating keyboard input.
        /// </summary>
        private readonly IKeyboardSimulator _keyboardSimulator;

        /// <summary>
        /// The <see cref="IMouseSimulator"/> instance to use for simulating mouse input.
        /// </summary>
        private readonly IMouseSimulator _mouseSimulator;

        /// <summary>
        /// The <see cref="IInputDeviceStateAdaptor"/> instance to use for interpreting the state of the input devices.
        /// </summary>
        private readonly IInputDeviceStateAdaptor _inputDeviceState;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputSimulator"/> class using the specified <see cref="IKeyboardSimulator"/>, <see cref="IMouseSimulator"/> and <see cref="IInputDeviceStateAdaptor"/> instances.
        /// </summary>
        /// <param name="keyboardSimulator">The <see cref="IKeyboardSimulator"/> instance to use for simulating keyboard input.</param>
        /// <param name="mouseSimulator">The <see cref="IMouseSimulator"/> instance to use for simulating mouse input.</param>
        /// <param name="inputDeviceStateAdaptor">The <see cref="IInputDeviceStateAdaptor"/> instance to use for interpreting the state of input devices.</param>
        public InputSimulator(IKeyboardSimulator keyboardSimulator, IMouseSimulator mouseSimulator, IInputDeviceStateAdaptor inputDeviceStateAdaptor)
        {
            _keyboardSimulator = keyboardSimulator;
            _mouseSimulator = mouseSimulator;
            _inputDeviceState = inputDeviceStateAdaptor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputSimulator"/> class using the default <see cref="KeyboardSimulator"/>, <see cref="MouseSimulator"/> and <see cref="WindowsInputDeviceStateAdaptor"/> instances.
        /// </summary>
        public InputSimulator()
        {
            _keyboardSimulator = new KeyboardSimulator(this);
            _mouseSimulator = new MouseSimulator(this);
            _inputDeviceState = new WindowsInputDeviceStateAdaptor();
        }

        /// <summary>
        /// Gets the <see cref="IKeyboardSimulator"/> instance for simulating Keyboard input.
        /// </summary>
        /// <value>The <see cref="IKeyboardSimulator"/> instance.</value>
        public IKeyboardSimulator Keyboard
        {
            get { return _keyboardSimulator; }
        }

        /// <summary>
        /// Gets the <see cref="IMouseSimulator"/> instance for simulating Mouse input.
        /// </summary>
        /// <value>The <see cref="IMouseSimulator"/> instance.</value>
        public IMouseSimulator Mouse
        {
            get { return _mouseSimulator; }
        }

        /// <summary>
        /// Gets the <see cref="IInputDeviceStateAdaptor"/> instance for determining the state of the various input devices.
        /// </summary>
        /// <value>The <see cref="IInputDeviceStateAdaptor"/> instance.</value>
        public IInputDeviceStateAdaptor InputDeviceState
        {
            get { return _inputDeviceState; }
        }

        /// <summary>
        /// Get or set enable events
        /// </summary>
        public MacroEventType MacroEventTypes { get => macroEventTypes; set => macroEventTypes = value; }

        /// <summary>
        /// Set which events can be palyed back.The default value is MacroEventType.KeyDown | MacroEventType.KeyUp | MacroEventType.MouseDown | MacroEventType.MouseUp | MacroEventType.MouseMove | MacroEventType.MouseWheel
        /// </summary>
        /// <param name="macroEventType"></param>
        /// <returns></returns>
        public IInputSimulator Enable(MacroEventType macroEventType)
        {
            this.MacroEventTypes |= macroEventType;
            return this;
        }

        /// <summary>
        /// disable events
        /// </summary>
        /// <param name="macroEventType"></param>
        /// <returns></returns>
        public IInputSimulator Disable(MacroEventType macroEventType)
        {
            this.MacroEventTypes &= ~macroEventType;
            return this;
        }

        /// <summary>
        /// simulate keyboard and mouse events
        /// </summary>
        /// <param name="mouseKeyEventList"></param>
        public void PlayBack(IList<MacroEvent> mouseKeyEventList, string windowTitle = null, string windowClassName = null)
        {
            if (mouseKeyEventList == null || mouseKeyEventList.Count == 0) return;
            var WaitTime = 1000;

            if (mouseKeyEventList.Count > 0)
            {
                foreach (MacroEvent mouseKeyEvent in mouseKeyEventList)
                {
                    if (this.CancelTokenSource != null && this.CancelTokenSource.IsCancellationRequested)
                    {
                        this.CancelTokenSource.Token.ThrowIfCancellationRequested();
                        Debug.WriteLine("Thead IsCancellationRequested....");
                        break;
                    }
                    #region Mouse simulator
                    if ((mouseKeyEvent.KeyMouseEventType & this.MacroEventTypes) != mouseKeyEvent.KeyMouseEventType) break;
                    switch (mouseKeyEvent.KeyMouseEventType)
                    {
                        case MacroEventType.MouseMove:
                            {
                                MouseEventArgs e = (MouseEventArgs)mouseKeyEvent.EventArgs;
                                var point = new Point(e.X, e.Y).ToAbsolutePoint();
                                this.Mouse.Sleep(mouseKeyEvent.TimeSinceLastEvent).MoveMouseTo(point.X, point.Y);
                                KListener_PlayBack(mouseKeyEvent);
                            }
                            break;
                        case MacroEventType.MouseClick:
                            {
                                MouseEventArgs e = (MouseEventArgs)mouseKeyEvent.EventArgs;

                                if (e.Button == MouseButtons.Left)
                                {
                                    if ((this.MacroEventTypes & MacroEventType.MouseMove) == MacroEventType.MouseMove)
                                        this.Mouse.Sleep(mouseKeyEvent.TimeSinceLastEvent).LeftButtonClick();
                                    else
                                    {
                                        this.Mouse.Sleep(WaitTime).MoveMouseTo(new Point(e.X, e.Y).ToAbsolutePoint()).Sleep(mouseKeyEvent.TimeSinceLastEvent).LeftButtonClick();
                                    }
                                }
                                else if (e.Button == MouseButtons.Right)
                                {
                                    if ((this.MacroEventTypes & MacroEventType.MouseMove) == MacroEventType.MouseMove)
                                        this.Mouse.Sleep(mouseKeyEvent.TimeSinceLastEvent).RightButtonClick();
                                    else
                                    {
                                        this.Mouse.Sleep(WaitTime).MoveMouseTo(new Point(e.X, e.Y).ToAbsolutePoint()).Sleep(mouseKeyEvent.TimeSinceLastEvent).RightButtonClick();
                                    }
                                }
                                else if (e.Button == MouseButtons.Middle)
                                {
                                    if ((this.MacroEventTypes & MacroEventType.MouseMove) == MacroEventType.MouseMove)
                                        this.Mouse.Sleep(mouseKeyEvent.TimeSinceLastEvent).MiddleButtonClick();
                                    else
                                    {
                                        this.Mouse.Sleep(WaitTime).MoveMouseTo(new Point(e.X, e.Y).ToAbsolutePoint()).Sleep(mouseKeyEvent.TimeSinceLastEvent).MiddleButtonClick();
                                    }
                                }
                                KListener_PlayBack(mouseKeyEvent);
                            }
                            break;
                        case MacroEventType.MouseDown:
                            {
                                MouseEventArgs e = (MouseEventArgs)mouseKeyEvent.EventArgs;
                                SetWindow(windowTitle, windowClassName);
                                if (e.Button == MouseButtons.Left)
                                {
                                    if ((this.MacroEventTypes & MacroEventType.MouseMove) == MacroEventType.MouseMove)
                                        this.Mouse.Sleep(mouseKeyEvent.TimeSinceLastEvent).LeftButtonDown();
                                    else
                                    {
                                        this.Mouse.Sleep(mouseKeyEvent.TimeSinceLastEvent > 500 ? WaitTime : 0).MoveMouseTo(new Point(e.X, e.Y).ToAbsolutePoint()).Sleep(mouseKeyEvent.TimeSinceLastEvent).LeftButtonDown();
                                    }
                                }
                                else if (e.Button == MouseButtons.Right)
                                {
                                    if ((this.MacroEventTypes & MacroEventType.MouseMove) == MacroEventType.MouseMove)
                                        this.Mouse.Sleep(mouseKeyEvent.TimeSinceLastEvent).RightButtonDown();
                                    else
                                    {
                                        this.Mouse.Sleep(WaitTime).MoveMouseTo(new Point(e.X, e.Y).ToAbsolutePoint()).Sleep(mouseKeyEvent.TimeSinceLastEvent).RightButtonDown();
                                    }
                                }
                                else if (e.Button == MouseButtons.Middle)
                                {
                                    if ((this.MacroEventTypes & MacroEventType.MouseMove) == MacroEventType.MouseMove)
                                        this.Mouse.Sleep(mouseKeyEvent.TimeSinceLastEvent).MiddleButtonDown();
                                    else
                                    {
                                        this.Mouse.Sleep(WaitTime).MoveMouseTo(new Point(e.X, e.Y).ToAbsolutePoint()).Sleep(mouseKeyEvent.TimeSinceLastEvent).MiddleButtonDown();
                                    }
                                }
                                KListener_PlayBack(mouseKeyEvent);
                            }
                            break;
                        case MacroEventType.MouseUp:
                            {
                                MouseEventArgs e = (MouseEventArgs)mouseKeyEvent.EventArgs;
                                if (e.Button == MouseButtons.Left)
                                {
                                    this.Mouse.Sleep(mouseKeyEvent.TimeSinceLastEvent).LeftButtonUp();
                                }
                                else if (e.Button == MouseButtons.Right)
                                {
                                    this.Mouse.Sleep(mouseKeyEvent.TimeSinceLastEvent).RightButtonUp();
                                }
                                else if (e.Button == MouseButtons.Middle)
                                {
                                    this.Mouse.Sleep(mouseKeyEvent.TimeSinceLastEvent).MiddleButtonUp();
                                }
                                ReleaseWindow(windowTitle, windowClassName);
                                KListener_PlayBack(mouseKeyEvent);
                            }
                            break;
                        case MacroEventType.MouseDoubleClick:
                            {
                                MouseEventArgs e = (MouseEventArgs)mouseKeyEvent.EventArgs;
                                if (e.Button == MouseButtons.Left)
                                {
                                    this.Mouse.Sleep(mouseKeyEvent.TimeSinceLastEvent).LeftButtonDoubleClick();
                                }
                                else if (e.Button == MouseButtons.Right)
                                {
                                    this.Mouse.Sleep(mouseKeyEvent.TimeSinceLastEvent).RightButtonDoubleClick();
                                }
                                else if (e.Button == MouseButtons.Middle)
                                {
                                    this.Mouse.Sleep(mouseKeyEvent.TimeSinceLastEvent).MiddleButtonDoubleClick();
                                }
                                KListener_PlayBack(mouseKeyEvent);
                            }
                            break;
                        case MacroEventType.MouseWheel:
                            {
                                MouseEventArgs e = (MouseEventArgs)mouseKeyEvent.EventArgs;
                                this.Mouse.Sleep(mouseKeyEvent.TimeSinceLastEvent).VerticalScroll(e.Delta);
                                KListener_PlayBack(mouseKeyEvent);
                            }
                            break;
                        case MacroEventType.KeyDown:
                            {
                                KeyEventArgs ergs = (KeyEventArgs)mouseKeyEvent.EventArgs;
                                SetWindow(windowTitle, windowClassName);
                                this.Keyboard.Sleep(mouseKeyEvent.TimeSinceLastEvent).KeyDown((VirtualKeyCode)((int)ergs.KeyCode));
                                KListener_PlayBack(mouseKeyEvent);
                            }
                            break;
                        case MacroEventType.KeyUp:
                            {
                                KeyEventArgs ergs = (KeyEventArgs)mouseKeyEvent.EventArgs;
                                this.Keyboard.Sleep(mouseKeyEvent.TimeSinceLastEvent).KeyUp((VirtualKeyCode)((int)ergs.KeyCode));
                                ReleaseWindow(windowTitle, windowClassName);
                                KListener_PlayBack(mouseKeyEvent);
                            }
                            break;
                        case MacroEventType.KeyPress:
                            {
                                KeyPressEventArgs ergs = (KeyPressEventArgs)mouseKeyEvent.EventArgs;
                                Keys key = (Keys)Enum.Parse(typeof(Keys), ((int)Char.ToUpper(ergs.KeyChar)).ToString());
                                this.Keyboard.Sleep(mouseKeyEvent.TimeSinceLastEvent).KeyPress((VirtualKeyCode)((int)key));
                                KListener_PlayBack(mouseKeyEvent);
                            }
                            break;
                        default:
                            break;
                    }
                    #endregion
                }
            }
        }

        private void KListener_PlayBack(MacroEvent e)
        {
            OnPlayback?.Invoke(null, e);
        }

        /// <summary>
        /// 设置窗体置前和键鼠事件总是响应
        /// </summary>
        /// <param name="windowTitle"></param>
        /// <param name="windowClassName"></param>
        private void SetWindow(string windowTitle, string windowClassName = null)
        {
            if (string.IsNullOrEmpty(windowTitle)) return;
            IntPtr wndFx = WinApi.FindWindow(windowClassName, windowTitle);
            if (wndFx != null && wndFx.ToInt32() > 0)
            {
                WinApi.SetForegroundWindow(wndFx);
                WinApi.SetCapture(wndFx);
                Thread.Sleep(300);
            }
        }

        /// <summary>
        /// 释放键鼠
        /// </summary>
        /// <param name="windowTitle"></param>
        /// <param name="windowClassName"></param>
        private void ReleaseWindow(string windowTitle, string windowClassName = null)
        {
            if (string.IsNullOrEmpty(windowTitle)) return;
            IntPtr wndFx = WinApi.FindWindow(windowClassName, windowTitle);
            if (wndFx != null && wndFx.ToInt32() > 0)
            {
                WinApi.ReleaseCapture();
            }
        }
    }
}