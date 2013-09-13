using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InterfacePlugin;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace UtilsGenerate
{
    public class SearchProjects : IActions
    {
        #region "Constans and Variable"
        private string _SOLUTION_PATH = new ReadConfig().GetSolutionLocation();
        private string _CLICK_ONCE_PATH = new ReadConfig().GetClickOnceLocation();
        private string _CLICK_ONCE_PREFIX = new ReadConfig().GetClickOncePefix();
        private string _CLICK_ONCE_OUT = new ReadConfig().GetDirectoryOut();
        private bool _SQL_GENERATE = new ReadConfig().GetSqlGenerate();
        private string _SQL_TEMPLATE_HEADER = new ReadConfig().GetSqlTemplateHeader();
        private string _SQL_TEMPLATE_BODY = new ReadConfig().GetSqlTemplateBody();
        private string _SQL_TEMPLATE_FOOTER = new ReadConfig().GetSqlTemplateFooter();

        bool _PLATAFORM_64BIT = System.Environment.Is64BitOperatingSystem;

        object[] _result = new object[] { };
        public object[] result
        {
            get { return _result; }
            set { _result = value; }
        }

        #endregion

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

        private void GenerateClickOnce()
        {
            Print obj = new Print();
            obj.ClearConsole();
            obj.PrintTitle("Proyectos Disponibles para la Publicacion");
            obj.PrintNewLine();

            DtoProject[] PROJECTS_FINDED = new UtilsProjects().FindProjects(Directory.GetParent(_SOLUTION_PATH).ToString());

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
            ValidProjectSelection(selection, PROJECTS_FINDED);

            obj.ClearConsole();
            obj.PrintString("Iniciando espere un momento...");

            obj.PrintInfo("Compilando Solucion....");

            //COMPILE SOLUTION
            if (!RunDevEnv(_SOLUTION_PATH))
            {
                obj.PrintError("No se puede continuar error en la compilación del proyecto.");
                return;
            }

            obj.PrintString("Iniciando Generacion de Click Once....");

            List<DtoProject> CollectionCO = new List<DtoProject>();
            foreach (object valor in selection)
            {
                DtoProject proj = (DtoProject)PROJECTS_FINDED.Where(x => x.id == int.Parse(valor.ToString())).SingleOrDefault();
                DtoProject clickOnce = GetClickOnceRelation(proj);

                obj.PrintInfo("Generando Click Once....");

                //PUBLISH PROJECT
                if (!RunMsbuild(clickOnce.FullPath))
                {
                    if (!obj.ShowQuestionContinue())
                    {
                        return;
                    }
                }
                CollectionCO.Add(clickOnce);
            }

            if (_SQL_GENERATE)
            {
                GenerateScripts(CollectionCO.ToArray());
            }
        }

        #region "Events Menu"
        private void ReloadMenu()
        {
            GenerateClickOnce();
        }
        #endregion

        #region "Functions"
        private string GetFilePathLog(string name, string type, string folderOptional = "")
        {
            string folderParent = _CLICK_ONCE_OUT;

            //organizacion de carpetas por fecha
            string sDate = DateTime.Now.ToString("yyyy-MM-dd HH-mm");

            folderParent = Path.Combine(folderParent, sDate);

            //carpeta opcional
            if (!string.IsNullOrEmpty(folderOptional))
            {
                folderParent = Path.Combine(folderParent, folderOptional);
            }

            //creacion del archivo del log
            if (!Directory.Exists(folderParent))
            {
                Directory.CreateDirectory(folderParent);
            }

            string oLog = Path.Combine(folderParent, string.Concat(Path.GetFileNameWithoutExtension(name), ".", type));
            if (!File.Exists(oLog))
            {
                File.WriteAllText(oLog, string.Empty);
            }

            return oLog;
        }
        private void GenerateScripts(DtoProject[] objs)
        {
            string sScripts = GetFilePathLog("Script_DB_Update", "sql");
            string sContent = string.Empty;

            sContent += _SQL_TEMPLATE_HEADER;

            foreach (DtoProject item in objs)
            {
                sContent += string.Concat(string.Format(_SQL_TEMPLATE_BODY, item.Name, item.VersionNew),Environment.NewLine);
            }

            sContent += _SQL_TEMPLATE_FOOTER;

            string temp = sContent;
            temp = Regex.Replace(sContent, @"\n.*\[NEWLINE]", "\r");
            temp = Regex.Replace(temp, @"\r.*\[NTAB]", "\r\t");
            temp = Regex.Replace(temp, @"\n.*\[NTAB]", "\r\t");
            temp = Regex.Replace(temp, @".*\[TAB]", "\t");
            temp = Regex.Replace(temp, @"\t", new string(char.Parse(" "), 4));

            try
            {
                File.WriteAllText(sScripts, temp);
            }
            catch (Exception ex)
            {
                new Print().PrintError("No se pudo crear el archivo de los scripts.");
            }
        }
        private DtoProject GetClickOnceRelation(DtoProject proj)
        {
            DtoProject[] CLICK_ONCE_FINDED = new UtilsProjects().FindProjects(_CLICK_ONCE_PATH, true);
            DtoProject clickOnce;
            Print obj = new Print();
            if (proj == null)
            {
                obj.PrintError("No se ha encontrado el proyecto.");
                obj.PressKeyToContinue();
                ReloadMenu();
            }

            if (string.IsNullOrEmpty(proj.ClickOnceSolution))
            {
                clickOnce = (DtoProject)CLICK_ONCE_FINDED
                    .Where(x => x.Name == (string.IsNullOrEmpty(_CLICK_ONCE_PREFIX) ? proj.Name : string.Concat(proj.Name, ".", _CLICK_ONCE_PREFIX)))
                     .SingleOrDefault();
            }
            else
            {
                clickOnce = (DtoProject)CLICK_ONCE_FINDED
                    .Where(x => x.Name == proj.ClickOnceSolution)
                    .SingleOrDefault();
            }

            if (clickOnce == null)
            {
                obj.PrintError("No se ha encontrado el proyecto del Click Once asociado.");
                obj.PressKeyToContinue();
                ReloadMenu();
            }

            ShowInfoVersionProject(clickOnce);
            return clickOnce;
        }
        private void ShowInfoVersionProject(DtoProject project)
        {
            Print obj = new Print();
            obj.ClearConsole();
            obj.PrintTitle("Mantenimiento de Versiones Click Once");
            obj.PrintString(string.Format("\n Project: {0} \n Version Actual: {1}", project.Name, project.Version));
            obj.PrintString("Ingrese la nueva version...");

            string version = obj.GetKeyString();
            if (!new UtilsProjects().SetVersionProject(project, version))
            {
                obj.PrintError("No se pudo actualizar la version del Click Once");
                obj.PressKeyToContinue();
                ReloadMenu();
            }

            project.VersionNew = version;
        }
        private void ValidProjectSelection(object[] selection, DtoProject[] PROJECTS_FINDED)
        {
            Print obj = new Print();
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
                if ((maxId <= (int.Parse(valor.ToString())) - 1))
                {
                    obj.PrintInfo("Item no encontrado: " + valor);
                    obj.PressKeyToContinue();
                    ReloadMenu();
                }
            }
        }
        #endregion

        #region "Commands Execute"
        private bool RunMsbuild(string Project)
        {
            string oLog = GetFilePathLog(Project, "log", "ClickOnce");

            string MSBUILD_ENV = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe";
            string ARGUMENTS_MSBUILD = string.Format(@" {0} /t:Clean;Rebuild;Publish /property:BootstrapperEnabled=false /property:PublishVersion='$(Proj.AssemblyVersion)' /l:FileLogger,Microsoft.Build.Engine;logfile={1}", Project, string.Concat("\"", oLog, "\""));
            return RunCommand(MSBUILD_ENV, ARGUMENTS_MSBUILD);
        }

        private bool RunDevEnv(string Solution)
        {
            string oOut = GetFilePathLog(Solution, "out", "Projects");
            string oLog = GetFilePathLog(Solution, "log", @"Projects\log");

            string DEV_ENV = Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles), @"Microsoft Visual Studio 10.0\Common7\IDE\devenv.com");
            string ARGUMENTS = string.Format("/Rebuild Release \"{0}\" /out \"{1}\" /log \"{2}\"", Solution, oOut, oLog);
            return RunCommand(DEV_ENV, ARGUMENTS);
        }

        private bool RunCommand(string ProgramPath, string arguments)
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

            if (process.ExitCode == 0)
            {
                new Print().PrintInfo("Se ha ejecutado la tarea.");
                return true;
            }
            else
            {
                new Print().PrintError("Se encontraron errores en la ejecucion.");
                return false;
            }
        }
        #endregion
    }
}
