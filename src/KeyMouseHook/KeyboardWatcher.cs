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
        #region Fields
        private readonly object accesslock = new object();
        public event EventHandler<MacroEvent> OnKeyboardInput;
        private MacroEventType macroEventTypes = MacroEventType.KeyDown | MacroEventType.KeyUp;
        #endregion

        #region Properties
        private bool isRunning { get; set; }
        private KeyMouseFactory Factory { get; set; }
        public MacroEventType MacroEventTypes { get => macroEventTypes; set => macroEventTypes = value; }
        #endregion

        #region Ctor
        /// <summary>
        /// Get an instance of mouse watcher.defalt event type is MacroEventType.KeyPress
        /// </summary>
        /// <param name="factory"></param>
        internal KeyboardWatcher(KeyMouseFactory factory)
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
        /// Start watching key events
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
        /// Stop watching key events
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
        /// enable key press event, the default value is MacroEventType.KeyPress
        /// </summary>
        /// <param name="macroEventType">MacroEventType.KeyPress</param>
        /// <returns></returns>
        public KeyboardWatcher Enable(MacroEventType macroEventType)
        {
            this.MacroEventTypes |= macroEventType;
            return this;
        }

        /// <summary>
        /// disable events, the default value is MacroEventType.KeyPress
        /// </summary>
        /// <param name="macroEventType"></param>
        /// <returns></returns>
        public KeyboardWatcher Disable(MacroEventType macroEventType)
        {
            this.MacroEventTypes &= ~macroEventType;
            return this;
        }

        internal void Subscribe(IKeyboardMouseEvents events = null)
        {
            if (events != null) this.Factory.KeyboardMouseEvents = events;

            if ((this.MacroEventTypes & MacroEventType.KeyDown) == MacroEventType.KeyDown)
                this.Factory.KeyboardMouseEvents.KeyDown += OnKeyDown;
            if ((this.MacroEventTypes & MacroEventType.KeyUp) == MacroEventType.KeyUp)
                this.Factory.KeyboardMouseEvents.KeyUp += OnKeyUp;
            if ((this.MacroEventTypes & MacroEventType.KeyPress) == MacroEventType.KeyPress)
                this.Factory.KeyboardMouseEvents.KeyPress += OnKeyPress;
        }

        internal void Unsubscribe()
        {
            if (this.Factory.KeyboardMouseEvents == null) return;

            if ((this.MacroEventTypes & MacroEventType.KeyDown) == MacroEventType.KeyDown)
                this.Factory.KeyboardMouseEvents.KeyDown -= OnKeyDown;
            if ((this.MacroEventTypes & MacroEventType.KeyUp) == MacroEventType.KeyUp)
                this.Factory.KeyboardMouseEvents.KeyUp -= OnKeyUp;
            if ((this.MacroEventTypes & MacroEventType.KeyPress) == MacroEventType.KeyPress)
                this.Factory.KeyboardMouseEvents.KeyPress -= OnKeyPress;
        }
        #endregion

        #region key events
        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (isRunning)
            {
                var time = Environment.TickCount - this.Factory.lastTimeRecorded;
                KListener_KeyEvent(new MacroEvent(MacroEventType.KeyPress, e, time));
                Debug.WriteLine(string.Format("KeyPress  \t\t {0}\n", e.KeyChar));
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (isRunning)
            {
                var time = Environment.TickCount - this.Factory.lastTimeRecorded;
                KListener_KeyEvent(new MacroEvent(MacroEventType.KeyDown, e, time));
                Debug.WriteLine(string.Format("KeyDown  \t\t {0}\n", e.KeyCode));
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (isRunning)
            {
                var time = Environment.TickCount - this.Factory.lastTimeRecorded;
                KListener_KeyEvent(new MacroEvent(MacroEventType.KeyUp, e, time));
                Debug.WriteLine(string.Format("KeyUp  \t\t {0}\n", e.KeyCode));
            }
        }
       
        /// <summary>
        /// Invoke user callbacks with the argument
        /// </summary>
        /// <param name="kd"></param>
        private void KListener_KeyEvent(MacroEvent e)
        {
            this.Factory.lastTimeRecorded = Environment.TickCount;
            OnKeyboardInput?.Invoke(null, e);
        }
        #endregion
    }
}