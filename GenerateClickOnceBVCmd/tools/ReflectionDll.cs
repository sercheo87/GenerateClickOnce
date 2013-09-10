using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using InterfacePlugin;

namespace GenerateClickOnceBVCmd.tools
{
    public class ReflectionDll
    {
        private static string dllFile { get; set; }
        private List<IActions> actions { get; set; }
        public Plugin itemPlugin { get; set; }

        public ReflectionDll()
        {
        }

        public ReflectionDll(string file)
        {
            dllFile = file;
        }

        public Plugin LoadMethods()
        {
            int idPlugin = 1;
            actions = new List<IActions>();

            itemPlugin = new Plugin()
            {
                Id = idPlugin,
                Name = Path.GetFileName(dllFile),
                LocationFile = dllFile
            };

            Assembly myDll = Assembly.LoadFrom(dllFile);
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type t in a.GetTypes())
                {
                    if (t.GetInterface("IActions") != null)
                    {
                        IActions obj = Activator.CreateInstance(t) as IActions;
                        if (obj != null)
                        {
                            actions.Add(obj);
                            itemPlugin.Action = obj;
                        }
                    }

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
                MenuProfile[] menu = (MenuProfile[])menuPropertyInfo.GetValue(action, null);
                collMenu.ConvertArrayToMenu(menu);
            }

            itemPlugin.MenuCollection = collMenu;
        }

        public object[] EjectMethod(string method)
        {
            foreach (IActions action in actions)
            {
                MethodInfo mi = action.GetType().GetMethod(method);
                return (object[])action.GetType().InvokeMember(method, BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, action, null);
            }
            return null;
        }

        public object[] EjectMethod(string method, object[] param)
        {
            foreach (IActions action in actions)
            {
                action.GetType().InvokeMember(method, BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, action, param);

            }
            return null;
        }

        public object[] GetProperty(string propertyName)
        {
            object[] obj = null;
            foreach (IActions action in actions)
            {
                PropertyInfo getPropertyInfo = action.GetType().GetProperty(propertyName);
                obj = (object[])getPropertyInfo.GetValue(action, null);
            }
            return obj;
        }
    }
}
