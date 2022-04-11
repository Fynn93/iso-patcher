using System;
using System.Xml;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Fynns_ISO_Patcher
{
    public class Variables
    {
        public static string ProjectName()
        {
            XmlDocument xml = new();
            xml.Load("settings.xml");
            XmlNodeList xNodeList = xml.SelectNodes("/Variables/ProjectName");
            string ProjectName = xNodeList![0]!["Value"]!.InnerText;
            return ProjectName;
        }

        public static string DataFile()
        {
            XmlDocument xml = new();
            xml.Load("settings.xml");
            XmlNodeList xNodeList = xml.SelectNodes("/Variables/DataFile");
            string df = xNodeList![0]!["Value"]!.InnerText;
            return df;
        }

        public static string UpdateURL()
        {
            XmlDocument xml = new();
            xml.Load("settings.xml");
            XmlNodeList xNodeList = xml.SelectNodes("/Variables/UpdateURL");
            string uu = xNodeList![0]!["Value"]!.InnerText;
            return uu;
        }

        public static string Version()
        {
            XmlDocument xml = new();
            xml.Load("settings.xml");
            XmlNodeList xNodeList = xml.SelectNodes("/Variables/Version");
            string v = xNodeList![0]!["Value"]!.InnerText;
            return v;
        }

        public static bool MKDI()
        {
            XmlDocument xml = new();
            xml.Load("settings.xml");
            XmlNodeList xNodeList = xml.SelectNodes("/Variables/MKDI");
            string mkdi = xNodeList![0]!["Value"]!.InnerText;
            if(mkdi == "true")
            {
                return true;
            }
            return false;
        }

        public static bool LETool()
        {
            XmlDocument a = new();
            a.Load("settings.xml");
            XmlNodeList b = a.SelectNodes("/Variables/LETool");
            string c = b![0]!["Value"]!.InnerText;
            if(c == "true")
            {
                return true;
            }
            return false;
        }
    }
}