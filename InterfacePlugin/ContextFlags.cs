using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterfacePlugin
{
    [Flags]
    public enum TYPE_MENU 
    { 
        SUB_MENU=100,
        CONFIRMACION=101,
        EVENT=102,
        SUB_MENU_CONFIRMATION=103
    }
}
