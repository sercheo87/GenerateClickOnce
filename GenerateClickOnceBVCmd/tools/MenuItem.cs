using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InterfacePlugin;

namespace GenerateClickOnceBVCmd.tools
{
    public class MenuItem
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string EventItem { get; set; }
        public TYPE_MENU Type { get; set; }
        public string Other { get; set; }
    }

}
