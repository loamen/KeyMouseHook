using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Loamen.KeyMouseHook.Simulators
{
    public class KeyMouseSimulator
    {
        #region Properties
        public GlobalMouseSimulator Mouse { get; set; }
        public GlobalKeySimulator Keyboard { get; set; }
        #endregion

        public KeyMouseSimulator()
        {
            Mouse = new GlobalMouseSimulator();
            Keyboard = new GlobalKeySimulator();
        }
        public void PlayBack(List<MacroEvent> mouseKeyEventList)
        {
            if (mouseKeyEventList == null || mouseKeyEventList.Count == 0) return;

            if (mouseKeyEventList.Count > 0)
            {
                foreach (MacroEvent mouseKeyEvent in mouseKeyEventList)
                {
                    #region Mouse simulator

                    switch (mouseKeyEvent.KeyMouseEventType)
                    {
                        case MacroEventType.MouseMove:
                            {
                                MouseEventArgs e = (MouseEventArgs)mouseKeyEvent.EventArgs;
                                this.Mouse.Sleep(mouseKeyEvent.TimeSinceLastEvent).MouseMove(e.X, e.Y);
                            }
                            break;
                        case MacroEventType.MouseDown:
                            {
                                MouseEventArgs e = (MouseEventArgs)mouseKeyEvent.EventArgs;
                                this.Mouse.Sleep(mouseKeyEvent.TimeSinceLastEvent).MouseDown(e.Button);
                            }
                            break;
                        case MacroEventType.MouseUp:
                            {
                                MouseEventArgs e = (MouseEventArgs)mouseKeyEvent.EventArgs;
                                this.Mouse.Sleep(mouseKeyEvent.TimeSinceLastEvent).MouseUp(e.Button);
                            }
                            break;
                        case MacroEventType.MouseWheel:
                            {
                                MouseEventArgs e = (MouseEventArgs)mouseKeyEvent.EventArgs;
                                this.Mouse.Sleep(mouseKeyEvent.TimeSinceLastEvent).MouseWheel(e.Delta);
                            }
                            break;
                        case MacroEventType.KeyDown:
                            {
                                KeyEventArgs ergs = (KeyEventArgs)mouseKeyEvent.EventArgs;
                                this.Keyboard.Sleep(mouseKeyEvent.TimeSinceLastEvent).KeyDown(ergs.KeyCode);
                                Debug.WriteLine("Key {0} event of key {1}={2}={3}", mouseKeyEvent.KeyMouseEventType, ergs.KeyData, ergs.KeyCode, ergs.KeyValue);
                            }
                            break;
                        case MacroEventType.KeyUp:
                            {
                                KeyEventArgs ergs = (KeyEventArgs)mouseKeyEvent.EventArgs;
                                this.Keyboard.Sleep(mouseKeyEvent.TimeSinceLastEvent).KeyUp(ergs.KeyCode);
                                Debug.WriteLine("Key {0} event of key {1}={2}={3}", mouseKeyEvent.KeyMouseEventType, ergs.KeyData, ergs.KeyCode, ergs.KeyValue);
                            }
                            break;
                        default:
                            break;
                    }
                    #endregion
                }
            }
        }
    }
}
