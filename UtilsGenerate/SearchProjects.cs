using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InterfacePlugin;

namespace UtilsGenerate
{
    public class SearchProjects : IActions
    {
        #region "Interface"
        public void DoAction(string action)
        {
            switch (action)
            {
                case "GenerateClickOnce":
                    GenerateClickOnce();
                    break;
            }
        }

        public MenuProfile[] MenuActions
        {
            get
            {
                return new MenuProfile[] { 
                    new MenuProfile() { Name = "Generar Click Once", Event = "GenerateClickOnce", Config = TYPE_MENU.SUB_MENU_CONFIRMATION }, 
                    new MenuProfile(){Name="Limpiar Proyectos",Event="CleanProyects", Config= TYPE_MENU.EVENT}
                };
            }
        }
        public object[] Confirmation
        {
            get
            {
                return new object[] { 
                    "GenerateClickOnce"
                };
            }
        }
        #endregion

        object[] _result = new object[] { };
        public object[] result
        {
            get { return _result; }
            set { _result = value; }
        }

        private void GenerateClickOnce()
        {
            Print obj = new Print();
            obj.ClearConsole();
            obj.PrintTitle("Proyectos Disponibles para la Publicacion");
            obj.PrintNewLine();
            obj.PrintString("1: Proyect 1");
            obj.PrintString("2: Proyect 2");
            obj.PrintString("3: Proyect 3");
            obj.PrintNewLine();
            obj.PrintString("Seleccione los proyectos que desee publicar.....");
            obj.PrintString("Ejemplo...");
            obj.PrintString("\t Para un solo Proyecto: 1");
            obj.PrintString("\t Para Multiples Proyectos: 1 3");
            obj.PrintString("\t Para Todos los proyectos: A");
            obj.PrintString("\t Para Cancelar teclee Esc");
            obj.PrintNewLine();

            object[] selection = obj.GetKeyInt(" ");
        }
    }
}
