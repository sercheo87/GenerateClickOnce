using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateClickOnceBVCmd.tools
{
    public class Plugin
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LocationFile { get; set; }
        public MenuCollection MenuCollection { get; set; }
    }

}
