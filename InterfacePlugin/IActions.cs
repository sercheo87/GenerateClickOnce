using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterfacePlugin
{
    public interface IActions
    {
        void DoAction(string action);
        MenuProfile[] MenuActions { get; }
        Object[] Confirmation { get; }
    }
}
