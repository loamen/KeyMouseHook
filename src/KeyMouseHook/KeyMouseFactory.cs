using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loamen.KeyMouseHook
{
    public class KeyMouseFactory : IDisposable
    {
        public IKeyboardMouseEvents KeyboardMouseEvents;
        private MouseWatcher _mouseWatcher;
        private KeyboardWatcher _keyboardWatcher;
        internal int lastTimeRecorded = 0;

        public KeyMouseFactory(IKeyboardMouseEvents events)
        {
            KeyboardMouseEvents = events ?? throw new ArgumentNullException("events can not be null.");
        }
        /// <summary>
        /// Get an instance of mouse watcher.defalt event type is MacroEventType.MouseClick | MacroEventType.MouseMove | MacroEventType.MouseWheel
        /// </summary>
        /// <returns></returns>
        public MouseWatcher GetMouseWatcher()
        {
            if (_mouseWatcher == null) _mouseWatcher = new MouseWatcher(this);
            return _mouseWatcher;
        }

        /// <summary>
        /// Get an instance of keystroke watcher.
        /// </summary>
        /// <returns></returns>
        public KeyboardWatcher GetKeyboardWatcher()
        {
            if (_keyboardWatcher == null) _keyboardWatcher = new KeyboardWatcher(this);
            return _keyboardWatcher;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (KeyboardMouseEvents == null) return;
            KeyboardMouseEvents.Dispose();
            KeyboardMouseEvents = null;
            if (_mouseWatcher != null)
            {
                _mouseWatcher.Dispose();
                _mouseWatcher = null;
            }
            if (_keyboardWatcher != null)
            {
                _keyboardWatcher.Dispose();
                _keyboardWatcher = null;
            }
        }
    }
}