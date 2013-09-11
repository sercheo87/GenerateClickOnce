using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InterfacePlugin;
using System.Diagnostics;
using System.IO;

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

            DtoProject[] items = new UtilsProjects().FindProjects(@"E:\Views\CEN_247\VB_IB\Dev\FE\Admin\Src\CEN\ClickOnce");
            foreach (DtoProject item in items)
            {
                obj.PrintString(string.Format("{0}: {1}", item.id, item.Name));
            }

            obj.PrintNewLine();
            obj.PrintString("Seleccione los proyectos que desee publicar.....");
            obj.PrintString("Ejemplo...");
            obj.PrintString("\t Para un solo Proyecto: 1");
            obj.PrintString("\t Para Multiples Proyectos: 1 3");
            obj.PrintString("\t Para Todos los proyectos: A");
            obj.PrintString("\t Para Cancelar teclee Esc");
            obj.PrintNewLine();

            object[] selection = obj.GetKeyInt(" ");

            foreach (object valor in selection)
            {
                if (string.IsNullOrEmpty(valor.ToString()))
                {
                    GenerateClickOnce();
                }

                int maxId = int.Parse(items.Max(x => x.id).ToString());
                if ((maxId <= (int.Parse(valor.ToString()))))
                {
                    obj.PrintString("Error Item no encontrado: " + valor);
                    Console.Read();
                    GenerateClickOnce();
                }
            }
            
            bool is64 = System.Environment.Is64BitOperatingSystem;

            //COMPILE SOLUTION
            string DEV_ENV = Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles), @"Microsoft Visual Studio 10.0\Common7\IDE\devenv.com");
            string ARGUMENTS=string.Format(" {0} /rebuild ", @"E:\Views\CEN_247\VB_IB\Dev\FE\Admin\Src\CEN\Solution\COBISCorp.tCOBIS.BVI.sln");
            RunCommand(DEV_ENV, ARGUMENTS);

            foreach (object valor in selection)
            {
                var projSel = items.Where(x => x.id == int.Parse(valor.ToString())).Select(x => x.FullPath).ToArray();
                string project = projSel[0];

                //PUBLISH PROJECT
                string MSBUILD_ENV = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe";
                string ARGUMENTS_MSBUILD = string.Format(@" {0} /t:Clean;Rebuild;Publish /property:BootstrapperEnabled=false /property:PublishVersion='$(Proj.AssemblyVersion)'", project);
                RunCommand(MSBUILD_ENV, ARGUMENTS_MSBUILD);
            }
        }

        private void RunCommand(string ProgramPath, string arguments)
        {
            var tempColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            var process = new Process();
            process.StartInfo = new ProcessStartInfo(ProgramPath);
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.OutputDataReceived += (sender, args) => Console.WriteLine("-->: {0}", args.Data);
            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
            Console.ForegroundColor = tempColor;
        }
    }
}
