using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateClickOnceBVCmd.tools
{
    public class MenuCollection
    {
        public List<MenuItem> Items { get; set; }

        public void ConvertArrayToMenu(object[] arg1)
        {
            int id = 0;
            Items = new List<MenuItem>();
            foreach (string item in arg1)
            {
                id++;
                MenuItem obj = new MenuItem();
                obj.Name = item.Split(char.Parse("|")).ToArray()[0].ToString();
                obj.EventItem = item.Split(char.Parse("|")).ToArray()[1].ToString();
                obj.Type = item.Split(char.Parse("|")).ToArray()[2].ToString();
                if (item.Split(char.Parse("|")).Length > 3)
                {
                    obj.Other = item.Split(char.Parse("|")).ToArray()[3].ToString();
                }
                obj.id = id;

                Items.Add(obj);
            }
        }
    }

}
