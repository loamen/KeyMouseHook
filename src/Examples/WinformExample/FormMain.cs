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

        private bool isRecording = false;

        public FormMain()
        {
            InitializeComponent();

            keyboardWatcher = eventHookFactory.GetKeyboardWatcher();
            keyboardWatcher.OnKeyboardInput += (s, e) =>
            {
                var keyEvent = (KeyEventArgs)e.EventArgs;
                Log(string.Format("Key {0}\t\t{1}\n", keyEvent.KeyCode, e.KeyMouseEventType));

                if (_macroEvents != null)
                {
                    _macroEvents.Add(e);
                }
            };

            mouseWatcher = eventHookFactory.GetMouseWatcher();
            mouseWatcher.OnMouseInput += (s, e) =>
            {
                if (_macroEvents != null)
                {
                    _macroEvents.Add(e);
                }
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
                            Log(string.Format("Mouse Down \t\t {0}\n", downExtEvent.Button));
                            return;
                        }
                        Log(string.Format("Mouse Down \t\t {0} Suppressed\n", downExtEvent.Button));
                        downExtEvent.Handled = true;
                        break;
                    case MacroEventType.MouseWheelExt:
                        MouseEventExtArgs wheelEvent = (MouseEventExtArgs)e.EventArgs;
                        labelWheel.Text = string.Format("Wheel={0:000}", wheelEvent.Delta);
                        Log("Mouse Wheel Move Suppressed.\n");
                        wheelEvent.Handled = true;
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
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (eventHookFactory != null)
                eventHookFactory.Dispose();
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            if (!isRecording)
            {
                if (radioApplication.Checked)
                    StartWatch(Hook.AppEvents());
                else if (radioGlobal.Checked)
                    StartWatch(Hook.GlobalEvents());
                isRecording = true;
                btnRecord.Text = "Stop";
            }
            else
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

        private void btnPlayback_Click(object sender, EventArgs e)
        {
            var sim = new InputSimulator();
            //var sim = new KeyMouseSimulator();
            sim.PlayBack(_macroEvents);
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
    }
}
