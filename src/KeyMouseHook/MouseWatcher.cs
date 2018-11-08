using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Loamen.KeyMouseHook
{
    public class MouseWatcher : IDisposable
    {
        private readonly object accesslock = new object();
        public event EventHandler<MacroEvent> OnMouseInput;
        private bool isRunning { get; set; }
        private int lastTimeRecorded = 0;
        private KeyMouseFactory Factory { get; set; }

        internal MouseWatcher(KeyMouseFactory factory)
        {
            this.Factory = factory;
        }

        /// <summary>
        /// Start watching mouse events
        /// </summary>
        public void Start(IKeyboardMouseEvents events = null)
        {
            lock (accesslock)
            {
                if (!isRunning)
                {
                    lastTimeRecorded = Environment.TickCount;
                    Unsubscribe();
                    Subscribe(events);
                    isRunning = true;
                }
            }
        }

        /// <summary>
        /// Stop watching mouse events
        /// </summary>
        public void Stop()
        {
            lock (accesslock)
            {
                if (isRunning)
                {
                    Unsubscribe();
                    isRunning = false;
                }
            }
        }

        private void Subscribe(IKeyboardMouseEvents events = null)
        {
            if (events != null) this.Factory.KeyboardMouseEvents = events;

            this.Factory.KeyboardMouseEvents.MouseUp += OnMouseUp;
            this.Factory.KeyboardMouseEvents.MouseDoubleClick += OnMouseDoubleClick;
            this.Factory.KeyboardMouseEvents.MouseMove += HookManager_MouseMove;
            this.Factory.KeyboardMouseEvents.MouseWheel += HookManager_MouseWheel;
            this.Factory.KeyboardMouseEvents.MouseDown += OnMouseDown;

        }

        private void Unsubscribe()
        {
            if (this.Factory.KeyboardMouseEvents == null) return;

            this.Factory.KeyboardMouseEvents.MouseUp -= OnMouseUp;
            this.Factory.KeyboardMouseEvents.MouseDoubleClick -= OnMouseDoubleClick;
            this.Factory.KeyboardMouseEvents.MouseMove -= HookManager_MouseMove;
            this.Factory.KeyboardMouseEvents.MouseWheel -= HookManager_MouseWheel;
            this.Factory.KeyboardMouseEvents.MouseDown -= OnMouseDown;
        }

        public void SupressMouse(bool isSupress, MacroEventType eventType)
        {
            if (this.Factory.KeyboardMouseEvents == null) return;

            if (isSupress)
            {
                switch (eventType)
                {
                    case MacroEventType.MouseDown:
                        this.Factory.KeyboardMouseEvents.MouseDown -= OnMouseDown;
                        this.Factory.KeyboardMouseEvents.MouseDownExt += KeyboardMouseEvents_MouseDownExt;
                        break;
                    case MacroEventType.MouseUp:
                        this.Factory.KeyboardMouseEvents.MouseUpExt -= OnMouseUp;
                        this.Factory.KeyboardMouseEvents.MouseUpExt += KeyboardMouseEvents_MouseUpExt;
                        break;
                    case MacroEventType.MouseMove:
                        this.Factory.KeyboardMouseEvents.MouseMove -= HookManager_MouseMove;
                        this.Factory.KeyboardMouseEvents.MouseMoveExt += KeyboardMouseEvents_MouseMoveExt;
                        break;
                    case MacroEventType.MouseWheel:
                        this.Factory.KeyboardMouseEvents.MouseWheel -= HookManager_MouseWheel;
                        this.Factory.KeyboardMouseEvents.MouseWheelExt += KeyboardMouseEvents_MouseWheelExt;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (eventType)
                {
                    case MacroEventType.MouseDown:
                        this.Factory.KeyboardMouseEvents.MouseDown += OnMouseDown;
                        this.Factory.KeyboardMouseEvents.MouseDownExt -= KeyboardMouseEvents_MouseDownExt;
                        break;
                    case MacroEventType.MouseUp:
                        this.Factory.KeyboardMouseEvents.MouseUpExt += OnMouseUp;
                        this.Factory.KeyboardMouseEvents.MouseUpExt -= KeyboardMouseEvents_MouseUpExt;
                        break;
                    case MacroEventType.MouseMove:
                        this.Factory.KeyboardMouseEvents.MouseMove += HookManager_MouseMove;
                        this.Factory.KeyboardMouseEvents.MouseMoveExt -= KeyboardMouseEvents_MouseMoveExt;
                        break;
                    case MacroEventType.MouseWheel:
                        this.Factory.KeyboardMouseEvents.MouseWheel += HookManager_MouseWheel;
                        this.Factory.KeyboardMouseEvents.MouseWheelExt -= KeyboardMouseEvents_MouseWheelExt;
                        break;
                    default:
                        break;
                }
            }
        }

        private void KeyboardMouseEvents_MouseMoveExt(object sender, MouseEventExtArgs e)
        {
            if (isRunning)
            {
                var time = Environment.TickCount - lastTimeRecorded;
                KListener_KeyDown(new MacroEvent(MacroEventType.MouseMoveExt, e, time));
                Debug.WriteLine(string.Format("MouseMoveExt \t\t {0}\n", e.Button));
            }
        }

        private void KeyboardMouseEvents_MouseWheelExt(object sender, MouseEventExtArgs e)
        {
            if (isRunning)
            {
                var time = Environment.TickCount - lastTimeRecorded;
                KListener_KeyDown(new MacroEvent(MacroEventType.MouseWheelExt, e, time));
                Debug.WriteLine(string.Format("MouseWheelExt \t\t {0}\n", e.Button));
            }
        }

        private void KeyboardMouseEvents_MouseUpExt(object sender, MouseEventExtArgs e)
        {
            if (isRunning)
            {
                var time = Environment.TickCount - lastTimeRecorded;
                KListener_KeyDown(new MacroEvent(MacroEventType.MouseUpExt, e, time));
                Debug.WriteLine(string.Format("MouseUpExt \t\t {0}\n", e.Button));
            }
        }

        private void KeyboardMouseEvents_MouseDownExt(object sender, MouseEventExtArgs e)
        {
            if (isRunning)
            {
                var time = Environment.TickCount - lastTimeRecorded;
                KListener_KeyDown(new MacroEvent(MacroEventType.MouseDownExt, e, time));
                Debug.WriteLine(string.Format("MouseDownExt \t\t {0}\n", e.Button));
            }
        }

        private void HookManager_MouseMove(object sender, MouseEventArgs e)
        {
            if (isRunning)
            {
                var time = Environment.TickCount - lastTimeRecorded;
                KListener_KeyDown(new MacroEvent(MacroEventType.MouseMove, e, time));
                Debug.WriteLine("x={0:0000}; y={1:0000}", e.X, e.Y);
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (isRunning)
            {
                var time = Environment.TickCount - lastTimeRecorded;
                KListener_KeyDown(new MacroEvent(MacroEventType.MouseDown, e, time));
                Debug.WriteLine(string.Format("MouseDown \t\t {0}\n", e.Button));
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (isRunning)
            {
                var time = Environment.TickCount - lastTimeRecorded;
                KListener_KeyDown(new MacroEvent(MacroEventType.MouseUp, e, time));
                Debug.WriteLine(string.Format("MouseUp \t\t {0}\n", e.Button));
            }
        }

        private void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (isRunning)
            {
                var time = Environment.TickCount - lastTimeRecorded;
                KListener_KeyDown(new MacroEvent(MacroEventType.MouseDoubleClick, e, time));
                Debug.WriteLine(string.Format("MouseDoubleClick \t\t {0}\n", e.Button));
            }
        }

        private void HookManager_MouseWheel(object sender, MouseEventArgs e)
        {
            if (isRunning)
            {
                var time = Environment.TickCount - lastTimeRecorded;
                KListener_KeyDown(new MacroEvent(MacroEventType.MouseWheel, e, time));
                Debug.WriteLine("Wheel={0:000}", e.Delta);
            }
        }

        /// <summary>
        /// Invoke user callbacks with the argument
        /// </summary>
        /// <param name="kd"></param>
        private void KListener_KeyDown(MacroEvent e)
        {
            lastTimeRecorded = Environment.TickCount;
            OnMouseInput?.Invoke(null, e);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            this.Unsubscribe();
        }
    }
}