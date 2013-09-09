using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilsGenerate
{
    public class SearchProjects : IActions
    {
        #region "Interface"
        public void DoAction()
        {
            Console.WriteLine("EJECUTADO METODO SIN PARAMETROS");
        }

        public void SetAction(object arg1)
        {
            _result = new object[] { arg1.ToString() };
        }

        object[] _result = new object[] { };
        public object[] Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
            }
        }

        public object[] MenuActions
        {
            get
            {
                return new object[] { 
                    "Generar Click Once|GenerateClickOnce|M|M",
                    "Limpiar Proyectos|CleanProyects|E" 
                };
            }
        }
        #endregion

        public void GenerateClickOnce()
        {
            _result = new object[] { 
                @"proyecto 1", 
                @"proyecto 2", 
                @"proyecto 3", 
                @"proyecto 4" 
            };
        }
    }
}
