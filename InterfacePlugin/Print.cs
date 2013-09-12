using System;
using System.Collections.Generic;
using System.Linq;

namespace InterfacePlugin
{
    public class Print
    {
        public void ClearConsole()
        {
            Console.Clear();
        }

        public void PrintInterline(bool newLine = false)
        {
            Console.WriteLine(string.Concat(new string(char.Parse("="), 80), (newLine ? "\n" : "")));
        }

        public void PrintString(string arg1)
        {
            Console.WriteLine(string.Format("{0}", arg1));
        }

        public void PrintError(string arg1)
        {
            var tempColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine(string.Format("ERROR: {0}", arg1));

            Console.ForegroundColor = tempColor;
        }

        public void PrintInfo(string arg1)
        {
            var tempColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine(string.Format("INFO: {0}", arg1));

            Console.ForegroundColor = tempColor;
        }

        public void PrintNewLine()
        {
            Console.WriteLine("");
        }

        public void PressKeyToContinue()
        {
            Console.WriteLine("Presione una tecla para continuar...");
            Console.ReadLine();

        }

        public void PrintTitle(string arg1)
        {
            PrintInterline();
            PrintString(arg1);
            PrintInterline();
            PrintNewLine();
        }

        #region Parse Key Input
        public int GetKeyInt()
        {
            string temp = Console.ReadLine();
            int ret = -1;
            int.TryParse(temp, out ret);
            return ret;
        }

        public object[] GetKeyInt(string CharParse)
        {
            string temp = Console.ReadLine();
            string[] ret = temp.Split(char.Parse(CharParse));
            return ret;
        }

        public void GetKeyExample()
        {
            Console.TreatControlCAsInput = true;
            ConsoleKeyInfo cki;
            Console.WriteLine("Press any combination of CTL, ALT, and SHIFT, and a console key.");
            Console.WriteLine("Press the Escape (Esc) key to quit: \n");
            do
            {
                cki = Console.ReadKey();
                Console.Write(" --- You pressed ");
                if ((cki.Modifiers & ConsoleModifiers.Alt) != 0) Console.Write("ALT+");
                if ((cki.Modifiers & ConsoleModifiers.Shift) != 0) Console.Write("SHIFT+");
                if ((cki.Modifiers & ConsoleModifiers.Control) != 0) Console.Write("CTL+");
                Console.WriteLine(cki.Key.ToString());
            } while (cki.Key != ConsoleKey.Escape);
        }
        #endregion
    }
}
