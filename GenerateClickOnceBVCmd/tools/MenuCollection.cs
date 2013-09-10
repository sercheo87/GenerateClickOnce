using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InterfacePlugin;

namespace GenerateClickOnceBVCmd.tools
{
    public class MenuCollection
    {
        public List<MenuItem> Items { get; set; }

        public void ConvertArrayToMenu(MenuProfile[] arg1)
        {
            int id = 0;
            Items = new List<MenuItem>();
            foreach (MenuProfile item in arg1)
            {
                id++;
                MenuItem obj = new MenuItem();
                obj.Name = item.Name;
                obj.EventItem = item.Event;
                obj.Type = item.Config;
                obj.id = id;

                Items.Add(obj);
            }
        }
    }

}
