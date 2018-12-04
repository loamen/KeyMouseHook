using Gma.System.MouseKeyHook;
using Loamen.KeyMouseHook;
using Loamen.KeyMouseHook.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleExample
{
    internal class LogKeys
    {
        public static void Record(Action action)
        {
            Console.WriteLine("Press " + Program.exitChar + " to quit.");
            KeyMouseFactory eventHookFactory = new KeyMouseFactory(Hook.GlobalEvents());
            KeyboardWatcher keyboardWatcher = eventHookFactory.GetKeyboardWatcher().Disable(MacroEventType.KeyDown| MacroEventType.KeyUp).Enable(MacroEventType.KeyPress);
            Program._macroEvents = new List<MacroEvent>();
            keyboardWatcher.OnKeyboardInput += (s, e) =>
            {
                Program._macroEvents.Add(e);

                if (e.KeyMouseEventType == MacroEventType.KeyPress)
                {
                    var keyEvent = (KeyPressEventArgs)e.EventArgs;
                    Console.Write(string.Format("Key {0}\t\t{1}\n", keyEvent.KeyChar, e.KeyMouseEventType));
                    //var ch = Console.ReadKey(true).KeyChar;
                    if (keyEvent.KeyChar == Program.exitChar)
                    {
                        keyboardWatcher.Stop();
                        eventHookFactory.Dispose();
                        Console.Clear();
                        Console.WriteLine("Record stopped");
                        Program.ConsoleLine();
                        action();
                        return;
                    }
                }
            };
            keyboardWatcher.Start();
        }

        public static void Playback(Action quit)
        {
            PlayBack(Program._macroEvents, quit);
        }

        private static void PlayBack(List<MacroEvent> mouseKeyEventList,Action quit)
        {
            if (mouseKeyEventList == null || mouseKeyEventList.Count == 0)
            {
                Console.Write("Exiting");
                Wait(3);
                Environment.Exit(0);
            }
            else
            {
                if (mouseKeyEventList.Count > 0)
                {
                    foreach (MacroEvent mouseKeyEvent in mouseKeyEventList)
                    {
                        #region Mouse simulator
                        Thread.Sleep(mouseKeyEvent.TimeSinceLastEvent);

                        switch (mouseKeyEvent.KeyMouseEventType)
                        {
                            case MacroEventType.KeyPress:
                                {
                                    KeyPressEventArgs ergs = (KeyPressEventArgs)mouseKeyEvent.EventArgs;
                                   
                                    Console.WriteLine(string.Format("Input {0}\t\t{1}", ergs.KeyChar, mouseKeyEvent.KeyMouseEventType));
                                    if (ergs.KeyChar == Program.exitChar)
                                    {
                                        Program.ConsoleLine();
                                        Console.Write("Playback completed");
                                        Wait(3);
                                        quit();
                                    }
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

        private static void Wait(int seconds)
        {
            for (int i = 0; i < seconds; i++)
            {
                Console.Write(".");
                Thread.Sleep(1000);
            }
        }
    }
}