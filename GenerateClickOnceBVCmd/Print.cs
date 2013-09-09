using System;
using System.Collections.Generic;
using System.Linq;
using GenerateClickOnceBVCmd.tools;

namespace GenerateClickOnceBVCmd
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

        public void PrintNewLine()
        {
            Console.WriteLine("\n");
        }

        public void PrintTitle(string arg1)
        {
            PrintInterline();
            PrintString(arg1);
            PrintInterline();
            PrintNewLine();
        }

        public void PrintMenu(object[] arg1)
        {
            int id = 0;
            foreach (string item in arg1)
            {
                id++;
                Console.WriteLine(string.Format("[{0}] {1}", id, item.Split(char.Parse("|")).ToArray()[0]));
            }
        }

        public object[] PrintMenu(List<Plugin> arg1)
        {
            List<object[]> res = new List<object[]>() { };
            int id = 1;
            foreach (Plugin _plugin in arg1)
            {
                foreach (MenuItem _menuItem in _plugin.MenuCollection.Items)
                {
                    res.Add(new object[] { id, _plugin, _menuItem });
                    Console.WriteLine(string.Format("[{0}] {1}", id, _menuItem.Name));
                    id++;
                }
            }

            Console.WriteLine("Seleccione una Opcion...");
            return new object[] { res, true };
        }
    }
}
