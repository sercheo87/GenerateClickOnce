using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using GenerateClickOnceBVCmd.tools;

namespace GenerateClickOnceBVCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            Print objPrint = new Print();
            List<Plugin> CollectionPlugins = new List<Plugin>();
            List<object[]> _MENU = new List<object[]>();

            objPrint.ClearConsole();
            objPrint.PrintTitle("Generacion automatica de Click Once");

            ReflectionDll _ReflectionDll = new ReflectionDll(GetDll("UtilsGenerate.dll"));
            CollectionPlugins.Add(_ReflectionDll.itemPlugin);
            ShowMenuDashboard(objPrint, CollectionPlugins, _MENU, _ReflectionDll);
             
            //_Print objPrint = new _Print();
            //List<Plugin> _Plugins = new List<Plugin>();
            //List<MenuCollection> _Menus = new List<MenuCollection>();
            //int idPlugin = 1;

            //objPrint.ClearConsole();
            //objPrint.PrintTitle("Generacion automatica de Click Once");

            //Plugin itemPlugin = new Plugin()
            //   {
            //       Id = idPlugin,
            //       Name = Path.GetFileName("UtilsGenerate.dll"),
            //       LocationFile = "UtilsGenerate.dll"
            //   };

            //List<object> actions = new List<object>();
            //Assembly myDll = Assembly.LoadFrom("UtilsGenerate.dll");
            //Type[] types = myDll.GetTypes();

            //for (int i = 0; i < types.Length; i++)
            //{
            //    Type type = myDll.GetType(types[i].FullName);
            //    if (type.GetInterface("UtilsGenerate.IActions") != null)
            //    {
            //        object obj = Activator.CreateInstance(type);
            //        if (obj != null)
            //            actions.Add(obj);
            //    }
            //}

            //MenuCollection collMenu = new MenuCollection();

            //foreach (var action in actions)
            //{
            //    MethodInfo mi = action.GetType().GetMethod("DoAction");

            //    // get info about property
            //    PropertyInfo numberPropertyInfo = action.GetType().GetProperty("Result");
            //    // get value of property
            //    object[] value = (object[])numberPropertyInfo.GetValue(action, null);
            //    // invoke public instance method:
            //    action.GetType().InvokeMember("SetAction", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, action, new object[] { 20 });

            //    PropertyInfo menuPropertyInfo = action.GetType().GetProperty("MenuActions");
            //    object[] menu = (object[])menuPropertyInfo.GetValue(action, null);
            //    collMenu.ConvertArrayToMenu(menu);
            //}
            //itemPlugin.MenuCollection = collMenu;

            //_Menus.Add(collMenu);
            //_Plugins.Add(itemPlugin);

            //objPrint.PrintMenu(_Plugins);

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
             
            //Console.ReadLine();
            Console.ReadKey();
        }

        static string GetDll(string arg1)
        {
            //return Path.Combine("Pugins", arg1);
            return string.Concat(@"Plugins\", arg1);
        }

        static void ShowMenuDashboard(Print objPrint, List<Plugin> CollectionPlugins, List<object[]> _MENU, ReflectionDll _ReflectionDll)
        {
            object[] ret = objPrint.PrintMenu(CollectionPlugins);
            _MENU = (List<object[]>)ret[0];

            string optionSelected = Console.ReadLine();

            int num;
            bool isNumeric = int.TryParse(optionSelected, out num);
            if (isNumeric)
            {
                for (int i=0 ;i<= ((object[])_MENU[0]).Length-1;i++)
                {
                    if (((object[])_MENU[0])[i].ToString().Equals(num.ToString()))
                    {
                        Plugin _obj1 = (Plugin)((object[])_MENU[0])[1];
                        MenuItem _obj2 = (MenuItem)((object[])_MENU[0])[2];

                         _ReflectionDll.EjectMethod(_obj2.EventItem);
                        object[] Result=_ReflectionDll.GetProperty("Result");
                    }
                }
            }
            else
            {
                objPrint.ClearConsole();
                ShowMenuDashboard(objPrint, CollectionPlugins, _MENU, _ReflectionDll);
            }
        }
    }
}
