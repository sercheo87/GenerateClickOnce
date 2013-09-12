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
        private string _SOLUTION_PATH = new ReadConfig().GetSolutionLocation();
        private string _CLICK_ONCE_PATH = new ReadConfig().GetClickOnceLocation();
        private string _CLICK_ONCE_PREFIX = new ReadConfig().GetClickOnceLocation();
        bool _PLATAFORM_64BIT = System.Environment.Is64BitOperatingSystem;

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

            DtoProject[] PROJECTS_FINDED = new UtilsProjects().FindProjects(Directory.GetParent(_SOLUTION_PATH).ToString());
            DtoProject[] CLICK_ONCE_FINDED = new UtilsProjects().FindProjects(_CLICK_ONCE_PATH);

            foreach (DtoProject project in PROJECTS_FINDED)
            {
                obj.PrintString(string.Format("{0}: {1}", project.id, project.Name));
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

            //validacion de seleccion nula
            if (selection == null || selection.Length <= 0)
            {
                obj.PrintString("Error seleccion incorrecta");
                obj.PressKeyToContinue();
                ReloadMenu();
            }

            //validacion de cada proyecto seleccionado
            foreach (object valor in selection)
            {
                if (string.IsNullOrEmpty(valor.ToString()))
                {
                    obj.PrintInfo("Item no encontrado: " + valor);
                    obj.PressKeyToContinue();
                    ReloadMenu();
                }

                int maxId = int.Parse(PROJECTS_FINDED.Max(x => x.id).ToString());
                if ((maxId <= (int.Parse(valor.ToString()))-1))
                {
                    obj.PrintInfo("Item no encontrado: " + valor);
                    obj.PressKeyToContinue();
                    ReloadMenu();
                }
            }

            obj.ClearConsole();
            obj.PrintString("Iniciando espere un momento...");

            obj.PrintInfo("Compilando Solucion....");

            //COMPILE SOLUTION
            RunDevEnv(_SOLUTION_PATH);

            obj.PrintString("Iniciando Generacion de Click Once....");
            foreach (object valor in selection)
            {
                DtoProject proj = (DtoProject)PROJECTS_FINDED.Where(x => x.id == int.Parse(valor.ToString())).SingleOrDefault();
                DtoProject clickOnce;

                if (proj == null)
                {
                    obj.PrintError("No se ha encontrado el proyecto.");
                    obj.PressKeyToContinue();
                    ReloadMenu();
                }

                if (string.IsNullOrEmpty(proj.ClickOnceSolution))
                {
                    clickOnce = (DtoProject)CLICK_ONCE_FINDED
                         .Where(x => x.Name == string.Concat(proj.Name, ".", _CLICK_ONCE_PREFIX))
                         .SingleOrDefault();
                }
                else
                {
                    clickOnce = (DtoProject)CLICK_ONCE_FINDED
                        .Where(x => x.Name == proj.ClickOnceSolution)
                        .SingleOrDefault();
                }

                obj.PrintInfo("Generando Click Once....");

                if (clickOnce == null)
                {
                    obj.PrintError("No se ha encontrado el proyecto del Click Once asociado.");
                    obj.PressKeyToContinue();
                    ReloadMenu();
                }

                //PUBLISH PROJECT
                RunMsbuild(clickOnce.FullPath);
            }
        }


        private void ReloadMenu()
        {
            GenerateClickOnce();
        }

        private void RunMsbuild(string Project)
        {
            string MSBUILD_ENV = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe";
            string ARGUMENTS_MSBUILD = string.Format(@" {0} /t:Clean;Rebuild;Publish /property:BootstrapperEnabled=false /property:PublishVersion='$(Proj.AssemblyVersion)'", Project);
            RunCommand(MSBUILD_ENV, ARGUMENTS_MSBUILD);
        }

        private void RunDevEnv(string Solution)
        {
            string DEV_ENV = Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles), @"Microsoft Visual Studio 10.0\Common7\IDE\devenv.com");
            string ARGUMENTS = string.Format(" {0} /rebuild ", Solution);
            RunCommand(DEV_ENV, ARGUMENTS);
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
