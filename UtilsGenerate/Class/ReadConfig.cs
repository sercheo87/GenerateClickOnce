using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using System.IO;

namespace UtilsGenerate
{
    public class ReadConfig
    {
        private XDocument document = XDocument.Load(Path.Combine( Directory.GetParent(Assembly.GetExecutingAssembly().Location.ToString()).ToString(),"ProjectConfig.xml"));
        public string GetSolutionLocation()
        {
            XElement xElement = this.document.Descendants("config").Elements("SolutionLocation").SingleOrDefault<XElement>();
            return xElement.Value;
        }
        public string GetDirectoryOut()
        {
            XElement xElement = this.document.Descendants("config").Elements("DirectoryOut").SingleOrDefault<XElement>();
            return xElement.Value;
        }
        public string GetClickOnceLocation()
        {
            XElement xElement = this.document.Descendants("config").Elements("ClickOnceLocation").SingleOrDefault<XElement>();
            return xElement.Value;
        }
        public string GetClickOncePefix()
        {
            XAttribute xAttribute = this.document.Descendants("config").Elements("ClickOnceLocation").Attributes("prefix").SingleOrDefault<XAttribute>();
            return xAttribute.Value;
        }
        public string GetRelations(string arg1)
        {
            XElement xElement = (
                from x in this.document.Descendants("config").Elements("relations").Elements("relation")
                where x.Element("project").Value == arg1
                select x).Elements("installer").SingleOrDefault<XElement>();
            string result;
            if (xElement != null)
            {
                result = xElement.Value;
            }
            else
            {
                result = null;
            }
            return result;
        }
    }
}
