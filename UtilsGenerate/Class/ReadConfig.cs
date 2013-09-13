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
        private XDocument document = XDocument.Load(Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location.ToString()).ToString(), "ProjectConfig.xml"));

        #region "Params Publics"
        public string GetSolutionLocation()
        {
            XElement xElement = this.document
                .Descendants("config")
                .Elements("SolutionLocation")
                .SingleOrDefault<XElement>();
            return GetValueString(xElement);
        }
        public string GetDirectoryOut()
        {
            XElement xElement = this.document
                .Descendants("config")
                .Elements("DirectoryOut")
                .SingleOrDefault<XElement>();
            return GetValueString(xElement);
        }
        public string GetClickOnceLocation()
        {
            XElement xElement = this.document
                .Descendants("config")
                .Elements("ClickOnceLocation")
                .SingleOrDefault<XElement>();
            return GetValueString(xElement);
        }
        public string GetClickOncePefix()
        {
            XAttribute xAttribute = this.document
                .Descendants("config")
                .Elements("ClickOnceLocation")
                .Attributes("prefix")
                .SingleOrDefault<XAttribute>();
            return GetValueString(xAttribute);
        }
        public string GetRelations(string arg1)
        {
            XElement xElement = (
                from x in this.document.Descendants("config").Elements("relations").Elements("relation")
                where x.Element("project").Value == arg1
                select x).Elements("installer").SingleOrDefault<XElement>();
            return GetValueString(xElement);
        }
        public bool GetGenerateZip()
        {
            XElement xElement = this.document
                .Descendants("config")
                .Elements("general")
                .Elements("zip")
                .SingleOrDefault<XElement>();

            return GetValueBool(xElement);
        }

        public bool GetDistributionActive()
        {
            XAttribute xAtribute = this.document
                .Descendants("config")
                .Elements("distribution")
                .Attributes("active")
                .SingleOrDefault<XAttribute>();

            return GetValueBool(xAtribute);
        }
        public string GetDistributionPathDestination()
        {
            XElement xElement = this.document
                .Descendants("config")
                .Elements("distribution")
                .Elements("path_destination")
                .SingleOrDefault<XElement>();

            return GetValueString(xElement);
        }
        public bool GetDistributionReplace()
        {
            XElement XElement = this.document
                .Descendants("config")
                .Elements("distribution")
                .Elements("replace")
                .SingleOrDefault<XElement>();

            return GetValueBool(XElement);
        }

        public bool GetSqlGenerate()
        {
            XAttribute xAttribute = this.document
                .Descendants("config")
                .Elements("sql")
                .Attributes("active")
                .SingleOrDefault<XAttribute>();

            return GetValueBool(xAttribute);
        }
        public string GetSqlTemplateHeader()
        {
            XElement xElement = this.document
                .Descendants("config")
                .Elements("sql")
                .Elements("template")
                .Elements("header")
                .SingleOrDefault<XElement>();

            return GetValueString(xElement);
        }
        public string GetSqlTemplateBody()
        {
            XElement xElement = this.document
                .Descendants("config")
                .Elements("sql")
                .Elements("template")
                .Elements("body")
                .SingleOrDefault<XElement>();

            return GetValueString(xElement);
        }
        public string GetSqlTemplateFooter()
        {
            XElement xElement = this.document
                .Descendants("config")
                .Elements("sql")
                .Elements("template")
                .Elements("footer")
                .SingleOrDefault<XElement>();

            return GetValueString(xElement);
        }
        #endregion

        #region "Function Privates"
        private bool ValidValue(XElement obj)
        {
            if (obj != null)
            {
                if (!string.IsNullOrEmpty(obj.Value.ToString().Trim()))
                {
                    return true;
                }
            }

            return false;
        }
        private bool ValidValue(XAttribute obj)
        {
            if (obj != null)
            {
                if (!string.IsNullOrEmpty(obj.Value.ToString().Trim()))
                {
                    return true;
                }
            }

            return false;
        }

        private bool GetValueBool(XElement obj)
        {
            if (!ValidValue(obj))
            {
                if (obj.Value.ToString().Trim().ToUpper().Equals("S"))
                {
                    return true;
                }
            }
            return false;
        }
        private bool GetValueBool(XAttribute obj)
        {
            if (ValidValue(obj))
            {
                if (obj.Value.ToString().Trim().ToUpper().Equals("S"))
                {
                    return true;
                }
            }
            return false;
        }

        private string GetValueString(XElement obj)
        {
            if (ValidValue(obj))
            {
                if (!string.IsNullOrEmpty(obj.Value.ToString().Trim()))
                {
                    return obj.Value.ToString().Trim();
                }
            }
            return string.Empty;
        }
        private string GetValueString(XAttribute obj)
        {
            if (ValidValue(obj))
            {
                if (!string.IsNullOrEmpty(obj.Value.ToString().Trim()))
                {
                    return obj.Value.ToString().Trim();
                }
            }
            return string.Empty;
        }
        #endregion
    }
}
