using System;
using System.Collections.Generic;
using System.Threading;

namespace Loamen.KeyMouseHook
{
    /// <summary>
    /// The contract for a service that simulates Keyboard and Mouse input and Hardware Input Device state detection for the Windows Platform.
    /// </summary>
    public interface IInputSimulator
    {
        /// <summary>
        /// Get or set enable events
        /// </summary>
        MacroEventType MacroEventTypes { get; set; }
        CancellationTokenSource CancelTokenSource { get; set; }
        /// <summary>
        /// Callback event
        /// </summary>
        event EventHandler<MacroEvent> OnPlayback;
        /// <summary>
        /// Gets the <see cref="IKeyboardSimulator"/> instance for simulating Keyboard input.
        /// </summary>
        /// <value>The <see cref="IKeyboardSimulator"/> instance.</value>
        IKeyboardSimulator Keyboard { get; }

        /// <summary>
        /// Gets the <see cref="IMouseSimulator"/> instance for simulating Mouse input.
        /// </summary>
        /// <value>The <see cref="IMouseSimulator"/> instance.</value>
        IMouseSimulator Mouse { get; }

        /// <summary>
        /// Gets the <see cref="IInputDeviceStateAdaptor"/> instance for determining the state of the various input devices.
        /// </summary>
        /// <value>The <see cref="IInputDeviceStateAdaptor"/> instance.</value>
        IInputDeviceStateAdaptor InputDeviceState { get; }

        /// <summary>
        /// Set which events can be palyed back.The default value is MacroEventType.KeyDown | MacroEventType.KeyUp | MacroEventType.MouseDown | MacroEventType.MouseUp | MacroEventType.MouseMove | MacroEventType.MouseWheel
        /// </summary>
        /// <param name="macroEventType"></param>
        /// <returns></returns>
        IInputSimulator Enable(MacroEventType macroEventType);

        /// <summary>
        /// disable events
        /// </summary>
        /// <param name="macroEventType"></param>
        /// <returns></returns>
        IInputSimulator Disable(MacroEventType macroEventType);

        /// <summary>
        /// Play back
        /// </summary>
        /// <param name="mouseKeyEventList"></param>
        /// <param name="windowTitle"></param>
        /// <param name="windowClassName"></param>
        void PlayBack(IList<MacroEvent> mouseKeyEventList, string windowTitle = null, string windowClassName = null);
    }
}