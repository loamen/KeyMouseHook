using Loamen.KeyMouseHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ConsoleExample
{
    class Program
    {
        internal static List<MacroEvent> _macroEvents = new List<MacroEvent>();
        internal const char exitChar = 'Q';

        internal static void Main(string[] args)
        {
            ShowMenu();
            Application.Run(new ApplicationContext());
        }

        private static void ShowMenu()
        {
            var selector = new Dictionary<string, Action<Action>>
            {
                {"1. Record keys", LogKeys.Record},
                {"2. Playback", LogKeys.Playback},
                {exitChar + ". Quit", Exit}
            };

            Console.WriteLine("Please select one of these:");
            foreach (var selectorKey in selector.Keys)
                Console.WriteLine(selectorKey);

            Action<Action> action = null;

            while (action == null)
            {
                var ch = Console.ReadKey(true).KeyChar;
                action = selector
                    .Where(p => p.Key.StartsWith(ch.ToString()))
                    .Select(p => p.Value).FirstOrDefault();
            }
            ConsoleLine();
            if (action == LogKeys.Record)
                action(ShowMenu);
            else
                action(Application.Exit);
        }

        private static void Exit(Action quit)
        {
            Environment.Exit(0);
        }

        internal static void ConsoleLine()
        {
            Console.WriteLine("--------------------------------------------------");
        }
    }
}
