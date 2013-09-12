using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using GenerateClickOnceBVCmd.tools;
using InterfacePlugin;

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
            _ReflectionDll.LoadMethods();
            CollectionPlugins.Add(_ReflectionDll.itemPlugin);
            ShowMenuDashboard(objPrint, CollectionPlugins, _MENU, _ReflectionDll);

            ////
            //List<Plugin> _Plugins1 = new List<Plugin>();
            //List<MenuCollection> _Menus1 = new List<MenuCollection>();
            //int idPlugin1 = 1;
            
            //List<object> actions1 = new List<object>();
            //Assembly myDll1 = Assembly.LoadFrom("Plugins/UtilsGenerate.dll");
            //Type[] types1 = myDll1.GetTypes();

            //for (int i = 0; i < types1.Length; i++)
            //{
            //    Type type2 = myDll1.GetType(types1[i].FullName);
            //    if (type2.GetInterface("UtilsGenerate.IActions") != null)
            //    {
            //        object obj1 = Activator.CreateInstance(type2);
            //        if (obj1 != null)
            //            actions1.Add(obj1);
            //    }
            //}


            //foreach (var action in actions1)
            //{
            //    object[] valueobj = (object[])action.GetType().InvokeMember("GenerateClickOnce", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, action, null);
            //    MethodInfo mi = action.GetType().GetMethod("sumar");

            //    // get info about property
            //    PropertyInfo numberPropertyInfo = action.GetType().GetProperty("xx");
            //    // get value of property
            //    int value = (int)numberPropertyInfo.GetValue(action, null);
            //    // invoke public instance method:
            //    action.GetType().InvokeMember("SetAction", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, action, new object[] { 20 });

            //    PropertyInfo menuPropertyInfo = action.GetType().GetProperty("MenuActions");
            //    object[] menu = (object[])menuPropertyInfo.GetValue(action, null);
            //}
            ///////////////////////////////////////////

                 
             
            //Console.ReadLine();
            Console.ReadKey();
        }

        static string GetDll(string arg1)
        {
            return string.Concat(@"E:\GenerateClickOnce\GenerateClickOnceBVCmd\bin\Debug\Plugins\", arg1);
        }

        static void ShowMenuDashboard(Print objPrint, List<Plugin> CollectionPlugins, List<object[]> _MENU, ReflectionDll _ReflectionDll)
        {
            object[] ret = PrintMenu(CollectionPlugins);
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
                        _obj1.Action.DoAction(_obj2.EventItem);
                    }
                }
            }
            else
            {
                objPrint.ClearConsole();
                ShowMenuDashboard(objPrint, CollectionPlugins, _MENU, _ReflectionDll);
            }
        }

        static object[] PrintMenu(List<Plugin> arg1)
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
