using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilsGenerate
{
    public interface IActions
    {
        void DoAction();
        void SetAction(object arg1);
        object[] Result { get; set; }
        /// <summary>
        /// Formato de cada item del menu
        /// NOMBRE_MENU|EVENTO_ASOCIADO|TIPO|OPCIONAL
        /// 
        /// TIPO por default E
        /// E:EVENTO
        /// M:MENU
        /// 
        /// </summary>
        object[] MenuActions { get; }
    }
}
