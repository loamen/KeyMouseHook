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
    public class KeyboardWatcher: IDisposable
    {
        private readonly object accesslock = new object();
        public event EventHandler<MacroEvent> OnKeyboardInput;
        private bool isRunning { get; set; }
        private int lastTimeRecorded = 0;
        private KeyMouseFactory Factory { get; set; }

        internal KeyboardWatcher(KeyMouseFactory factory)
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

        internal void Subscribe(IKeyboardMouseEvents events = null)
        {
            if (events != null) this.Factory.KeyboardMouseEvents = events;

            this.Factory.KeyboardMouseEvents.KeyDown += OnKeyDown;
            this.Factory.KeyboardMouseEvents.KeyUp += OnKeyUp;
            this.Factory.KeyboardMouseEvents.KeyPress += OnKeyPress;
        }

        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (isRunning)
            {
                var time = Environment.TickCount - lastTimeRecorded;
                KListener_KeyDown(new MacroEvent(MacroEventType.KeyPress, e, time));
                Debug.WriteLine(string.Format("KeyPress  \t\t {0}\n", e.KeyChar));
            }
        }

        internal void Unsubscribe()
        {
            if (this.Factory.KeyboardMouseEvents == null) return;

            this.Factory.KeyboardMouseEvents.KeyDown -= OnKeyDown;
            this.Factory.KeyboardMouseEvents.KeyUp -= OnKeyUp;
            this.Factory.KeyboardMouseEvents.KeyPress -= OnKeyPress;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (isRunning)
            {
                var time = Environment.TickCount - lastTimeRecorded;
                KListener_KeyDown(new MacroEvent(MacroEventType.KeyDown, e, time));
                Debug.WriteLine(string.Format("KeyDown  \t\t {0}\n", e.KeyCode));
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (isRunning)
            {
                var time = Environment.TickCount - lastTimeRecorded;
                KListener_KeyDown(new MacroEvent(MacroEventType.KeyUp, e, time));
                Debug.WriteLine(string.Format("KeyUp  \t\t {0}\n", e.KeyCode));
            }
        }

        /// <summary>
        /// Invoke user callbacks with the argument
        /// </summary>
        /// <param name="kd"></param>
        private void KListener_KeyDown(MacroEvent e)
        {
            lastTimeRecorded = Environment.TickCount;
            OnKeyboardInput?.Invoke(null, e);
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