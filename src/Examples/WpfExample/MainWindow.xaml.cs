using Gma.System.MouseKeyHook;
using Loamen.KeyMouseHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfExample
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly KeyMouseFactory eventHookFactory = new KeyMouseFactory(Hook.GlobalEvents());
        private readonly KeyboardWatcher keyboardWatcher;
        private readonly MouseWatcher mouseWatcher;
        private List<MacroEvent> _macroEvents;

        private bool isRecording = false;
        public MainWindow()
        {
            InitializeComponent();
            keyboardWatcher = eventHookFactory.GetKeyboardWatcher();
            keyboardWatcher.OnKeyboardInput += (s, e) =>
            {
                var keyEvent = (System.Windows.Forms.KeyEventArgs)e.EventArgs;
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
                        var mouseEvent = (System.Windows.Forms.MouseEventArgs)e.EventArgs;
                        LogMouseLocation(mouseEvent.X, mouseEvent.Y);
                        break;
                    case MacroEventType.MouseWheel:
                        mouseEvent = (System.Windows.Forms.MouseEventArgs)e.EventArgs;
                        LogMouseWheel(mouseEvent.Delta);
                        break;
                    case MacroEventType.MouseDown:
                    case MacroEventType.MouseUp:
                        mouseEvent = (System.Windows.Forms.MouseEventArgs)e.EventArgs;
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
                        labelWheel.Content = string.Format("Wheel={0:000}", wheelEvent.Delta);
                        Log("Mouse Wheel Move Suppressed.\n");
                        wheelEvent.Handled = true;
                        break;

                }
            };
        }

        private void btnRecord_Click(object sender, RoutedEventArgs e)
        {
            if (!isRecording)
            {
                if (radioApplication.IsChecked ?? false)
                    StartWatch(Hook.AppEvents());
                else if (radioGlobal.IsChecked ?? false)
                    StartWatch(Hook.GlobalEvents());
                isRecording = true;
                btnRecord.Content = "Stop";
            }
            else
            {
                StopWatch();
                isRecording = false;
                btnRecord.Content = "Record";
                if (_macroEvents != null && _macroEvents.Count > 0)
                {
                    btnPlayback.IsEnabled = true;
                }
            }
        }

        private void btnPlayback_Click(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            //var sim = new KeyMouseSimulator();
            sim.PlayBack(_macroEvents);
        }

        private void btnClearLog_Click(object sender, RoutedEventArgs e)
        {
            textBoxLog.Clear();
        }

        private void Log(string text)
        {
            textBoxLog.AppendText(text);
            textBoxLog.ScrollToEnd();
        }

        private void LogMouseWheel(int Delta)
        {
            labelWheel.Content = string.Format("Wheel={0:000}", Delta);
        }
        private void LogMouseLocation(int X, int Y)
        {
            labelMousePosition.Content = string.Format("x={0:0000}; y={1:0000}", X, Y);
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

        private void radioNone_Checked(object sender, RoutedEventArgs e)
        {
            var chkButton = (System.Windows.Controls.RadioButton)sender;
            if (chkButton == null ? false : (bool)chkButton.IsChecked)
            {
                StopWatch();
                isRecording = false;
                btnRecord.Content = "Record";
                if (_macroEvents != null && _macroEvents.Count > 0)
                {
                    btnPlayback.IsEnabled = true;
                }
            }
        }

        private void checkBoxSuppressMouse_Checked(object sender, RoutedEventArgs e)
        {
            if (eventHookFactory.KeyboardMouseEvents == null) return;

            mouseWatcher.SupressMouse((bool)((System.Windows.Controls.CheckBox)sender).IsChecked, MacroEventType.MouseDown);

        }

        private void checkBoxSupressMouseWheel_Checked(object sender, RoutedEventArgs e)
        {
            if (eventHookFactory.KeyboardMouseEvents == null) return;

            mouseWatcher.SupressMouse((bool)((System.Windows.Controls.CheckBox)sender).IsChecked, MacroEventType.MouseWheel);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (eventHookFactory != null)
                eventHookFactory.Dispose();
        }
    }
}
