using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UtilsGenerate
{
    public class UtilsProjects
    {
        public DtoProject[] FindProjects(string path)
        {

            ReadConfig XmlDoc = new ReadConfig();
            XmlDoc.GetClickOncePefix();
            List<DtoProject> ret = new List<DtoProject>();
            int id = 1;
            foreach (string item in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Where(file=>file.EndsWith("csproj")| file.EndsWith("vbproj")))
            {
                ret.Add(new DtoProject()
                    {
                        id = id++,
                        FullPath = item,
                        Name = Path.GetFileNameWithoutExtension(item),
                        ClickOnceSolution = XmlDoc.GetRelations(Path.GetFileNameWithoutExtension(item))
                    });
            }
            return ret.ToArray();
        }
    }

    public class DtoProject
    {
        public int id { get; set; }
        public string FullPath { get; set; }
        public string Name { get; set; }
        public string ClickOnceSolution { get; set; }
    }
}
