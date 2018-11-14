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
        #region Fields
        private readonly object accesslock = new object();
        public event EventHandler<MacroEvent> OnMouseInput;
        private bool enableMouseDrag = false;
        private bool enableMouseDoubleClick = false;
        private bool enableMouseMove = false;
        #endregion

        #region Properties
        private bool isRunning { get; set; }
        private KeyMouseFactory Factory { get; set; }
        #endregion

        #region Ctor
        internal MouseWatcher(KeyMouseFactory factory)
        {
            this.Factory = factory;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            this.Unsubscribe();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Start watching mouse events
        /// </summary>
        public void Start(IKeyboardMouseEvents events = null)
        {
            lock (accesslock)
            {
                if (!isRunning)
                {
                    this.Factory.lastTimeRecorded = Environment.TickCount;
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

        /// <summary>
        /// Enable mouse drag started and finised event or double click event
        /// </summary>
        /// <param name="macroEventType">MacroEventType.MouseDragStarted | MacroEventType.MouseDoubleClick</param>
        /// <returns></returns>
        public MouseWatcher Enable(MacroEventType macroEventType)
        {
            if ((macroEventType & MacroEventType.MouseDoubleClick) == MacroEventType.MouseDoubleClick)
                this.enableMouseDoubleClick = true;
            if ((macroEventType & MacroEventType.MouseDragFinished) == MacroEventType.MouseDragFinished)
                this.enableMouseDrag = true;
            if ((macroEventType & MacroEventType.MouseDragStarted) == MacroEventType.MouseDragStarted)
                this.enableMouseDrag = true;
            if ((macroEventType & MacroEventType.MouseMove) == MacroEventType.MouseMove)
                this.enableMouseMove = true;
            return this;
        }

        private void Subscribe(IKeyboardMouseEvents events = null)
        {
            if (events != null) this.Factory.KeyboardMouseEvents = events;

            this.Factory.KeyboardMouseEvents.MouseUp += OnMouseUp;

            this.Factory.KeyboardMouseEvents.MouseWheel += OnMouseWheel;
            this.Factory.KeyboardMouseEvents.MouseDown += OnMouseDown;

            if (enableMouseMove)
                this.Factory.KeyboardMouseEvents.MouseMove += OnMouseMove;

            if (enableMouseDrag)
            {
                this.Factory.KeyboardMouseEvents.MouseDragStarted += OnMouseDragStarted;
                this.Factory.KeyboardMouseEvents.MouseDragFinished += OnMouseDragFinished;
            }
            if (enableMouseDoubleClick)
                this.Factory.KeyboardMouseEvents.MouseDoubleClick += OnMouseDoubleClick;
        }

        private void Unsubscribe()
        {
            if (this.Factory.KeyboardMouseEvents == null) return;

            this.Factory.KeyboardMouseEvents.MouseUp -= OnMouseUp;

            this.Factory.KeyboardMouseEvents.MouseWheel -= OnMouseWheel;
            this.Factory.KeyboardMouseEvents.MouseDown -= OnMouseDown;

            if (enableMouseMove)
                this.Factory.KeyboardMouseEvents.MouseMove -= OnMouseMove;

            if (enableMouseDrag)
            {
                this.Factory.KeyboardMouseEvents.MouseDragStarted -= OnMouseDragStarted;
                this.Factory.KeyboardMouseEvents.MouseDragFinished -= OnMouseDragFinished;
            }
            if (enableMouseDoubleClick)
                this.Factory.KeyboardMouseEvents.MouseDoubleClick -= OnMouseDoubleClick;
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
                        this.Factory.KeyboardMouseEvents.MouseDownExt += OnMouseDownExt;
                        break;
                    case MacroEventType.MouseUp:
                        this.Factory.KeyboardMouseEvents.MouseUpExt -= OnMouseUp;
                        this.Factory.KeyboardMouseEvents.MouseUpExt += OnMouseUpExt;
                        break;
                    case MacroEventType.MouseMove:
                        this.Factory.KeyboardMouseEvents.MouseMove -= OnMouseMove;
                        this.Factory.KeyboardMouseEvents.MouseMoveExt += OnMouseMoveExt;
                        break;
                    case MacroEventType.MouseWheel:
                        this.Factory.KeyboardMouseEvents.MouseWheel -= OnMouseWheel;
                        this.Factory.KeyboardMouseEvents.MouseWheelExt += OnMouseWheelExt;
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
                        this.Factory.KeyboardMouseEvents.MouseDownExt -= OnMouseDownExt;
                        break;
                    case MacroEventType.MouseUp:
                        this.Factory.KeyboardMouseEvents.MouseUpExt += OnMouseUp;
                        this.Factory.KeyboardMouseEvents.MouseUpExt -= OnMouseUpExt;
                        break;
                    case MacroEventType.MouseMove:
                        this.Factory.KeyboardMouseEvents.MouseMove += OnMouseMove;
                        this.Factory.KeyboardMouseEvents.MouseMoveExt -= OnMouseMoveExt;
                        break;
                    case MacroEventType.MouseWheel:
                        this.Factory.KeyboardMouseEvents.MouseWheel += OnMouseWheel;
                        this.Factory.KeyboardMouseEvents.MouseWheelExt -= OnMouseWheelExt;
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion

        #region Mouse Events
        private void OnMouseDragFinished(object sender, MouseEventArgs e)
        {
            if (isRunning)
            {
                var time = Environment.TickCount - this.Factory.lastTimeRecorded;
                KListener_MouseEvent(new MacroEvent(MacroEventType.MouseDragFinished, e, time));
                Debug.WriteLine("MouseDragStarted");
            }
        }

        private void OnMouseDragStarted(object sender, MouseEventArgs e)
        {
            if (isRunning)
            {
                var time = Environment.TickCount - this.Factory.lastTimeRecorded;
                KListener_MouseEvent(new MacroEvent(MacroEventType.MouseDragStarted, e, time));
                Debug.WriteLine("MouseDragFinished");
            }
        }

        private void OnMouseMoveExt(object sender, MouseEventExtArgs e)
        {
            if (isRunning)
            {
                var time = Environment.TickCount - this.Factory.lastTimeRecorded;
                KListener_MouseEvent(new MacroEvent(MacroEventType.MouseMoveExt, e, time));
                Debug.WriteLine(string.Format("MouseMoveExt \t\t {0}\n", e.Button));
            }
        }

        private void OnMouseWheelExt(object sender, MouseEventExtArgs e)
        {
            if (isRunning)
            {
                var time = Environment.TickCount - this.Factory.lastTimeRecorded;
                KListener_MouseEvent(new MacroEvent(MacroEventType.MouseWheelExt, e, time));
                Debug.WriteLine(string.Format("MouseWheelExt \t\t {0}\n", e.Button));
            }
        }

        private void OnMouseUpExt(object sender, MouseEventExtArgs e)
        {
            if (isRunning)
            {
                var time = Environment.TickCount - this.Factory.lastTimeRecorded;
                KListener_MouseEvent(new MacroEvent(MacroEventType.MouseUpExt, e, time));
                Debug.WriteLine(string.Format("MouseUpExt \t\t {0}\n", e.Button));
            }
        }

        private void OnMouseDownExt(object sender, MouseEventExtArgs e)
        {
            if (isRunning)
            {
                var time = Environment.TickCount - this.Factory.lastTimeRecorded;
                KListener_MouseEvent(new MacroEvent(MacroEventType.MouseDownExt, e, time));
                Debug.WriteLine(string.Format("MouseDownExt \t\t {0}\n", e.Button));
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (isRunning)
            {
                var time = Environment.TickCount - this.Factory.lastTimeRecorded;
                KListener_MouseEvent(new MacroEvent(MacroEventType.MouseMove, e, time));
                Debug.WriteLine("x={0:0000}; y={1:0000}", e.X, e.Y);
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (isRunning)
            {
                var time = Environment.TickCount - this.Factory.lastTimeRecorded;
                KListener_MouseEvent(new MacroEvent(MacroEventType.MouseDown, e, time));
                Debug.WriteLine(string.Format("MouseDown \t\t {0}\n", e.Button));
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (isRunning)
            {
                var time = Environment.TickCount - this.Factory.lastTimeRecorded;
                KListener_MouseEvent(new MacroEvent(MacroEventType.MouseUp, e, time));
                Debug.WriteLine(string.Format("MouseUp \t\t {0}\n", e.Button));
            }
        }

        private void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (isRunning)
            {
                var time = Environment.TickCount - this.Factory.lastTimeRecorded;
                KListener_MouseEvent(new MacroEvent(MacroEventType.MouseDoubleClick, e, time));
                Debug.WriteLine(string.Format("MouseDoubleClick \t\t {0}\n", e.Button));
            }
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (isRunning)
            {
                var time = Environment.TickCount - this.Factory.lastTimeRecorded;
                KListener_MouseEvent(new MacroEvent(MacroEventType.MouseWheel, e, time));
                Debug.WriteLine("Wheel={0:000}", e.Delta);
            }
        }

        /// <summary>
        /// Invoke user callbacks with the argument
        /// </summary>
        /// <param name="kd"></param>
        private void KListener_MouseEvent(MacroEvent e)
        {
            this.Factory.lastTimeRecorded = Environment.TickCount;
            OnMouseInput?.Invoke(null, e);
        }
        #endregion
    }
}