using Gma.System.MouseKeyHook;
using Loamen.KeyMouseHook;
using Loamen.KeyMouseHook.Simulators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinformExample
{
    public partial class FormMain : Form
    {
        private readonly KeyMouseFactory eventHookFactory = new KeyMouseFactory(Hook.GlobalEvents());
        private readonly KeyboardWatcher keyboardWatcher;
        private readonly MouseWatcher mouseWatcher;
        private List<MacroEvent> _macroEvents;
        private Hotkey hotkey;
        private bool isRecording = false;
        private bool isPlaying = false;

        private int hotkeyRecordId;
        private int hotkeyPlaybackId;

        public FormMain()
        {
            InitializeComponent();

            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, Screen.PrimaryScreen.WorkingArea.Height - this.Height);

            keyboardWatcher = eventHookFactory.GetKeyboardWatcher().Enable(MacroEventType.KeyPress);
            keyboardWatcher.OnKeyboardInput += (s, e) =>
            {
                if (_macroEvents != null)
                    _macroEvents.Add(e);

                if (e.KeyMouseEventType == MacroEventType.KeyPress)
                {
                    var keyEvent = (KeyPressEventArgs)e.EventArgs;
                    Keys key = (Keys)Enum.Parse(typeof(Keys), ((int)keyEvent.KeyChar).ToString());
                    Log(string.Format("Key {0}\t\t{1}\n", key, e.KeyMouseEventType));
                }
                else
                {
                    var keyEvent = (KeyEventArgs)e.EventArgs;
                    Log(string.Format("Key {0}\t\t{1}\n", keyEvent.KeyCode, e.KeyMouseEventType));
                }
            };

            mouseWatcher = eventHookFactory.GetMouseWatcher().Enable(MacroEventType.MouseDragStarted | MacroEventType.MouseDoubleClick);
            mouseWatcher.OnMouseInput += (s, e) =>
            {
                if (_macroEvents != null)
                    _macroEvents.Add(e);

                switch (e.KeyMouseEventType)
                {
                    case MacroEventType.MouseMove:
                        var mouseEvent = (MouseEventArgs)e.EventArgs;
                        LogMouseLocation(mouseEvent.X, mouseEvent.Y);
                        break;
                    case MacroEventType.MouseWheel:
                        mouseEvent = (MouseEventArgs)e.EventArgs;
                        LogMouseWheel(mouseEvent.Delta);
                        break;
                    case MacroEventType.MouseDown:
                    case MacroEventType.MouseUp:
                        mouseEvent = (MouseEventArgs)e.EventArgs;
                        Log(string.Format("Mouse {0}\t\t{1}\n", mouseEvent.Button, e.KeyMouseEventType));
                        break;
                    case MacroEventType.MouseDownExt:
                        MouseEventExtArgs downExtEvent = (MouseEventExtArgs)e.EventArgs;
                        if (downExtEvent.Button != MouseButtons.Right)
                        {
                            Log(string.Format("Mouse Down \t {0}\n", downExtEvent.Button));
                            return;
                        }
                        Log(string.Format("Mouse Down \t {0} Suppressed\n", downExtEvent.Button));
                        downExtEvent.Handled = true;
                        break;
                    case MacroEventType.MouseWheelExt:
                        MouseEventExtArgs wheelEvent = (MouseEventExtArgs)e.EventArgs;
                        labelWheel.Text = string.Format("Wheel={0:000}", wheelEvent.Delta);
                        Log("Mouse Wheel Move Suppressed.\n");
                        wheelEvent.Handled = true;
                        break;
                    case MacroEventType.MouseDragStarted:
                        Log("MouseDragStarted\n");
                        break;
                    case MacroEventType.MouseDragFinished:
                        Log("MouseDragFinished\n");
                        break;
                    case MacroEventType.MouseDoubleClick:
                        mouseEvent = (MouseEventArgs)e.EventArgs;
                        Log(string.Format("Mouse {0}\t\t{1}\n", mouseEvent.Button, e.KeyMouseEventType));
                        break;
                    default:
                        break;
                }
            };
        }

        private void Log(string text)
        {
            if (IsDisposed) return;
            textBoxLog.AppendText(text);
            textBoxLog.ScrollToCaret();
        }

        private void LogMouseWheel(int Delta)
        {
            if (IsDisposed) return;
            labelWheel.Text = string.Format("Wheel={0:000}", Delta);
        }
        private void LogMouseLocation(int X, int Y)
        {
            if (IsDisposed) return;
            labelMousePosition.Text = string.Format("x={0:0000}; y={1:0000}", X, Y);
        }

        public void StartWatch(IKeyboardMouseEvents events = null)
        {
            Thread.Sleep(1000);
            _macroEvents = new List<MacroEvent>();
            keyboardWatcher.Start(events);
            mouseWatcher.Start(events);
        }

        public void StopWatch()
        {
            keyboardWatcher.Stop();
            mouseWatcher.Stop();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            InitHotkey();
        }

        private void InitHotkey()
        {
            hotkey = new Hotkey(this.Handle);
            hotkey.OnHotkey += Hotkey_OnHotkey;
            this.hotkeyRecordId = hotkey.RegisterHotkey(Keys.F10, Hotkey.KeyFlags.MOD_CONTROL);
            this.hotkeyPlaybackId = hotkey.RegisterHotkey(Keys.F12, Hotkey.KeyFlags.MOD_CONTROL);

            #region Combination
            //var record = Combination.TriggeredBy(Keys.F10).With(Keys.Control);
            //var playback = Combination.TriggeredBy(Keys.F12).With(Keys.Control);

            //var assignment = new Dictionary<Combination, Action>
            //{
            //    {record, ()=>{this.Record(); Debug.WriteLine("Control+F10"); } },
            //    {playback,  ()=>{this.Playback(); Debug.WriteLine("Control+F12"); }}
            //};

            //Hook.GlobalEvents().OnCombination(assignment);
            #endregion
        }

        private void Hotkey_OnHotkey(int HotKeyID)
        {
            if (HotKeyID == hotkeyRecordId)
            {
                if (isPlaying) return;
                this.Record();
            }
            else if (HotKeyID == hotkeyPlaybackId)
                this.Playback();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (eventHookFactory != null)
                eventHookFactory.Dispose();
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            Record();
        }

        private void btnPlayback_Click(object sender, EventArgs e)
        {
            Playback();
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            textBoxLog.Clear();
        }

        private void radioNone_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                StopWatch();
                isRecording = false;
                btnRecord.Text = "Record";
                if (_macroEvents != null && _macroEvents.Count > 0)
                {
                    btnPlayback.Enabled = true;
                }
            }
        }

        private void checkBoxSuppressMouse_CheckedChanged(object sender, EventArgs e)
        {
            if (eventHookFactory.KeyboardMouseEvents == null) return;

            mouseWatcher.SupressMouse(((CheckBox)sender).Checked, MacroEventType.MouseDown);
        }

        private void checkBoxSupressMouseWheel_CheckedChanged(object sender, EventArgs e)
        {
            if (eventHookFactory.KeyboardMouseEvents == null) return;

            mouseWatcher.SupressMouse(((CheckBox)sender).Checked, MacroEventType.MouseWheel);
        }

        private void Record()
        {
            if (!isRecording)
            {
                if (radioApplication.Checked)
                    StartWatch(Hook.AppEvents());
                else if (radioGlobal.Checked)
                    StartWatch(Hook.GlobalEvents());
                isRecording = true;
                btnRecord.Text = "Stop(Ctrl+F10)";
            }
            else
            {
                StopWatch();
                isRecording = false;
                btnRecord.Text = "Record(Ctrl+F10)";
                if (_macroEvents != null && _macroEvents.Count > 0)
                {
                    btnPlayback.Enabled = true;
                }
            }
        }

        private void Playback()
        {
            this.isPlaying = true;
            btnPlayback.Enabled = false;
            var sim = new InputSimulator().Enable(MacroEventType.KeyPress | MacroEventType.MouseDoubleClick);
            //var sim = new KeyMouseSimulator();
            sim.OnPlayback += OnPlayback;
            sim.PlayBack(_macroEvents);
            btnPlayback.Enabled = true;
            var timer = new System.Threading.Timer(new TimerCallback(SetPlaying), false, 2000, 2000);
        }

        private void OnPlayback(object sender, MacroEvent e)
        {
            switch (e.KeyMouseEventType)
            {
                case MacroEventType.MouseMove:
                    var mouseEvent = (MouseEventArgs)e.EventArgs;
                    LogMouseLocation(mouseEvent.X, mouseEvent.Y);
                    break;
                case MacroEventType.MouseWheel:
                    mouseEvent = (MouseEventArgs)e.EventArgs;
                    LogMouseWheel(mouseEvent.Delta);
                    break;
                case MacroEventType.MouseDown:
                case MacroEventType.MouseUp:
                    mouseEvent = (MouseEventArgs)e.EventArgs;
                    Log(string.Format("Mouse {0}\t\t{1}\t\tSimulator\n", mouseEvent.Button, e.KeyMouseEventType));
                    break;
                case MacroEventType.MouseDownExt:
                    MouseEventExtArgs downExtEvent = (MouseEventExtArgs)e.EventArgs;
                    if (downExtEvent.Button != MouseButtons.Right)
                    {
                        Log(string.Format("Mouse Down \t {0}\t\t\tSimulator\n", downExtEvent.Button));
                        return;
                    }
                    Log(string.Format("Mouse Down \t {0} Suppressed.\t\tSimulator\n", downExtEvent.Button));
                    downExtEvent.Handled = true;
                    break;
                case MacroEventType.MouseWheelExt:
                    MouseEventExtArgs wheelEvent = (MouseEventExtArgs)e.EventArgs;
                    labelWheel.Text = string.Format("Wheel={0:000}", wheelEvent.Delta);
                    Log("Mouse Wheel Move Suppressed.\t\tSimulator\n");
                    wheelEvent.Handled = true;
                    break;
                case MacroEventType.MouseDragStarted:
                    Log("MouseDragStarted\t\tSimulator\n");
                    break;
                case MacroEventType.MouseDragFinished:
                    Log("MouseDragFinished\t\tSimulator\n");
                    break;
                case MacroEventType.MouseDoubleClick:
                    mouseEvent = (MouseEventArgs)e.EventArgs;
                    Log(string.Format("Mouse {0}\t\t{1}\t\tSimulator\n", mouseEvent.Button, e.KeyMouseEventType));
                    break;
                case MacroEventType.KeyPress:
                    var keyEvent = (KeyPressEventArgs)e.EventArgs;
                    Keys key = (Keys)Enum.Parse(typeof(Keys), ((int)keyEvent.KeyChar).ToString());
                    Log(string.Format("Key {0}\t\t{1}\t\tSimulator\n", key, e.KeyMouseEventType));
                    break;
                case MacroEventType.KeyDown:
                case MacroEventType.KeyUp:
                    var kEvent = (KeyEventArgs)e.EventArgs;
                    Log(string.Format("Key {0}\t\t{1}\t\tSimulator\n", kEvent.KeyCode, e.KeyMouseEventType));
                    break;
                default:
                    break;
            }
        }

        private void SetPlaying(object state)
        {
            isPlaying = (bool)state;
        }
    }
}
