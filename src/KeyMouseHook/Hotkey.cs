using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Loamen.KeyMouseHook
{
    public delegate void HotkeyEventHandler(int HotKeyID);

    public class Hotkey : IMessageFilter
    {
        public event HotkeyEventHandler OnHotkey;
        private readonly IntPtr hWnd;
        private readonly Hashtable keyIDs = new Hashtable();

        public enum KeyFlags
        {
            NONE = 0x0,
            MOD_ALT = 0x1,
            MOD_CONTROL = 0x2,
            MOD_SHIFT = 0x4,
            MOD_WIN = 0x8,
            MOD_ALT_CTRL = 0x3,
            MOD_ALT_SHIFT = 0x5,
            MOD_CTRL_SHIFT = 0x6,
            MOD_ALT_CTRL_SHIFT = 0x7
        }

        #region Windows Api Code

        [DllImport("user32.dll")]
        public static extern UInt32 RegisterHotKey(IntPtr hWnd, UInt32 id, UInt32 fsModifiers, UInt32 vk);

        [DllImport("user32.dll")]
        public static extern UInt32 UnregisterHotKey(IntPtr hWnd, UInt32 id);

        [DllImport("kernel32.dll")]
        public static extern UInt32 GlobalAddAtom(String lpString);

        [DllImport("kernel32.dll")]
        public static extern UInt32 GlobalDeleteAtom(UInt32 nAtom);
        #endregion

        #region Methods

        public Hotkey(IntPtr hWnd)
        {
            this.hWnd = hWnd;
            Application.AddMessageFilter(this);
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 0x312)
            {
                if (OnHotkey != null)
                {
                    foreach (UInt32 key in keyIDs.Values)
                    {
                        if ((UInt32)m.WParam == key)
                        {
                            OnHotkey((int)m.WParam);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public int RegisterHotkey(Keys Key, KeyFlags keyflags)
        {
            UInt32 hotkeyid = GlobalAddAtom(Guid.NewGuid().ToString());
            RegisterHotKey(hWnd, hotkeyid, (UInt32)keyflags, (UInt32)Key);
            if (!keyIDs.ContainsKey(hotkeyid))
                keyIDs.Add(hotkeyid, hotkeyid);
            return (int)hotkeyid;
        }

        public int RegisterHotkey(Keys Key, UInt32 keyflags)
        {
            UInt32 hotkeyid = GlobalAddAtom(Guid.NewGuid().ToString());
            RegisterHotKey(hWnd, hotkeyid, keyflags, (UInt32)Key);
            if (!keyIDs.ContainsKey(hotkeyid))
                keyIDs.Add(hotkeyid, hotkeyid);
            return (int)hotkeyid;
        }

        public void UnregisterHotkeys()
        {
            Application.RemoveMessageFilter(this);
            foreach (UInt32 key in keyIDs.Values)
            {
                UnregisterHotKey(hWnd, key);
                GlobalDeleteAtom(key);
            }
        }
        #endregion
    }
}