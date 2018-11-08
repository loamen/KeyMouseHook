using Loamen.KeyMouseHook.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;

namespace Loamen.KeyMouseHook
{
    /// <summary>
    /// Implements the <see cref="IInputSimulator"/> interface to simulate Keyboard and Mouse input and provide the state of those input devices.
    /// </summary>
    public class InputSimulator : IInputSimulator
    {
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

        public void PlayBack(List<MacroEvent> mouseKeyEventList)
        {
            if (mouseKeyEventList == null || mouseKeyEventList.Count == 0) return;

            if (mouseKeyEventList.Count > 0)
            {
                foreach (MacroEvent mouseKeyEvent in mouseKeyEventList)
                {
                    #region Mouse simulator

                    switch (mouseKeyEvent.KeyMouseEventType)
                    {
                        case MacroEventType.MouseMove:
                            {
                                MouseEventArgs e = (MouseEventArgs)mouseKeyEvent.EventArgs;
                                var point = new Point(e.X, e.Y).ToAbsolutePoint();
                                this.Mouse.Sleep(mouseKeyEvent.TimeSinceLastEvent).MoveMouseTo(point.X, point.Y);
                            }
                            break;
                        case MacroEventType.MouseDown:
                            {
                                MouseEventArgs e = (MouseEventArgs)mouseKeyEvent.EventArgs;
                                if (e.Button == MouseButtons.Left)
                                {
                                    this.Mouse.Sleep(mouseKeyEvent.TimeSinceLastEvent).LeftButtonDown();
                                }
                                else if (e.Button == MouseButtons.Right)
                                {
                                    this.Mouse.Sleep(mouseKeyEvent.TimeSinceLastEvent).RightButtonDown();
                                }
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
                            }
                            break;
                        case MacroEventType.MouseWheel:
                            {
                                MouseEventArgs e = (MouseEventArgs)mouseKeyEvent.EventArgs;
                                this.Mouse.Sleep(mouseKeyEvent.TimeSinceLastEvent).VerticalScroll(e.Delta);
                            }
                            break;
                        case MacroEventType.KeyDown:
                            {
                                KeyEventArgs ergs = (KeyEventArgs)mouseKeyEvent.EventArgs;
                             
                                this.Keyboard.Sleep(mouseKeyEvent.TimeSinceLastEvent).KeyDown((VirtualKeyCode)((int)ergs.KeyCode));
                            }
                            break;
                        case MacroEventType.KeyUp:
                            {
                                KeyEventArgs ergs = (KeyEventArgs)mouseKeyEvent.EventArgs;
                                this.Keyboard.Sleep(mouseKeyEvent.TimeSinceLastEvent).KeyUp((VirtualKeyCode)((int)ergs.KeyCode));
                            }
                            break;
                        default:
                            break;
                    }
                    #endregion
                }
            }
        }
    }
}