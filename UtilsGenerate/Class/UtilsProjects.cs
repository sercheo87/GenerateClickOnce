using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace UtilsGenerate
{
    public class UtilsProjects
    {
        public DtoProject[] FindProjects(string path, bool getVersion = false)
        {

            ReadConfig XmlDoc = new ReadConfig();
            XmlDoc.GetClickOncePefix();
            List<DtoProject> ret = new List<DtoProject>();
            int id = 1;
            foreach (string item in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Where(file => file.EndsWith("csproj") | file.EndsWith("vbproj")))
            {
                ret.Add(new DtoProject()
                    {
                        id = id++,
                        FullPath = item,
                        Name = Path.GetFileNameWithoutExtension(item),
                        ClickOnceSolution = XmlDoc.GetRelations(Path.GetFileNameWithoutExtension(item)),
                        Version = (getVersion ? GetVersionProject(item) : string.Empty)
                    });
            }
            return ret.ToArray();
        }

        public bool UpdateVersionProject(DtoProject[] arg1)
        {
            foreach (DtoProject item in arg1)
            {

            }

            return false;
        }

        private string GetVersionProject(string file)
        {
            XDocument doc = XDocument.Load(file);
            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";

            string sVersion = doc
               .Element(ns + "Project")
               .Elements(ns + "PropertyGroup")
               .Elements(ns + "ApplicationVersion")
               .Select(refElem => refElem.Value)
               .SingleOrDefault<string>();

            string sRevision = doc
               .Element(ns + "Project")
               .Elements(ns + "PropertyGroup")
               .Elements(ns + "ApplicationRevision")
               .Select(refElem => refElem.Value)
               .SingleOrDefault<string>();

            if (string.IsNullOrEmpty(sVersion) || string.IsNullOrEmpty(sRevision))
            {
                return "ERROR OBTENIENDO VERSION";
            }

            Match match = Regex.Match(sVersion, @"^([0-9]{0,3})\.([0-9]{0,3})\.([0-9]{0,3})\.", RegexOptions.IgnoreCase);

            string version = string.Empty;
            if (match.Success)
            {
                version = string.Concat(match.ToString(), sRevision);
            }
            return version;
        }

        public bool SetVersionProject(DtoProject obj, string version)
        {
            XDocument doc = XDocument.Load(obj.FullPath);
            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";

            Match match = Regex.Match(version, @"^([0-9]{0,3})\.([0-9]{0,3})\.([0-9]{0,3})\.([0-9]{0,3})", RegexOptions.IgnoreCase);

            if (match.Success)
            {
                string temp = doc
                   .Element(ns + "Project")
                   .Elements(ns + "PropertyGroup")
                   .Elements(ns + "ApplicationVersion")
                   .Select(refElem => refElem.Value)
                   .SingleOrDefault<string>();

                string sReplace = Regex.Replace(temp, @"^([0-9]{0,3})\.([0-9]{0,3})\.([0-9]{0,3})\.", string.Concat(match.Groups[1], ".", match.Groups[2], ".", match.Groups[3], "."));
                
                doc
                   .Element(ns + "Project")
                   .Element(ns + "PropertyGroup")
                   .SetElementValue(ns + "ApplicationVersion", sReplace);

                doc
                   .Element(ns + "Project")
                   .Element(ns + "PropertyGroup")
                   .SetElementValue(ns + "ApplicationRevision", match.Groups[4]);

                doc.Save(obj.FullPath);
                return true;
            }
            return false;
        }
    }

    public class DtoProject
    {
        public int id { get; set; }
        public string FullPath { get; set; }
        public string Name { get; set; }
        public string ClickOnceSolution { get; set; }
        public string Version { get; set; }
        public string VersionNew { get; set; }
    }
}
