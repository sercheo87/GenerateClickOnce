using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace GenerateClickOnceBVCmd.tools
{
    public class ReflectionDll
    {
        private static string dllFile { get; set; }
        private List<object> actions { get; set; }
        public Plugin itemPlugin { get; set; }

        public ReflectionDll()
        {
            LoadMethods();
        }

        public ReflectionDll(string file)
        {
            dllFile = file;
            LoadMethods();
        }

        public Plugin LoadMethods()
        {
            int idPlugin = 1;
            actions = new List<object>();

            itemPlugin = new Plugin()
            {
                Id = idPlugin,
                Name = Path.GetFileName(dllFile),
                LocationFile = dllFile
            };

            Assembly myDll = Assembly.LoadFrom(dllFile);
            Type[] types = myDll.GetTypes();

            for (int i = 0; i < types.Length; i++)
            {
                Type type = myDll.GetType(types[i].FullName);
                if (type.GetInterface("UtilsGenerate.IActions") != null)
                {
                    object obj = Activator.CreateInstance(type);
                    if (obj != null)
                        actions.Add(obj);
                }
            }

            GetMenuItems();
            return itemPlugin;
        }

        private void GetMenuItems()
        {
            MenuCollection collMenu = new MenuCollection();

            foreach (var action in actions)
            {
                PropertyInfo menuPropertyInfo = action.GetType().GetProperty("MenuActions");
                object[] menu = (object[])menuPropertyInfo.GetValue(action, null);
                collMenu.ConvertArrayToMenu(menu);
            }

            itemPlugin.MenuCollection = collMenu;
        }

        public void EjectMethod(string method)
        {
            foreach (var action in actions)
            {
                MethodInfo mi = action.GetType().GetMethod(method);
            }
        }

        public void EjectMethod(string method,object[] param)
        {
            foreach (var action in actions)
            {
                action.GetType().InvokeMember(method, BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, action, param);

            }
        }

        public object[] GetProperty(string propertyName)
        {
            object[] obj = null;
            foreach (var action in actions)
            {
                PropertyInfo getPropertyInfo = action.GetType().GetProperty(propertyName);
                obj = (object[])getPropertyInfo.GetValue(action, null);
            }
            return obj;
        }
    }
}
