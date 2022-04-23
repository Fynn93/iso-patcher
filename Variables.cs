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
		public static XmlDocument xml = new();
        public static string ProjectName()
        {
            xml.Load("settings.xml");
            XmlNodeList xNodeList = xml.SelectNodes("/Variables/ProjectName");
            string ProjectName = xNodeList![0]!["Value"]!.InnerText;
            return ProjectName;
        }
		
		public static string Region()
        {
            xml.Load("settings.xml");
            XmlNodeList xNodeList = xml.SelectNodes("/Variables/Region");
            string Region = xNodeList![0]!["Value"]!.InnerText;
            return Region;
        }

        public static string DataFile()
        {
            xml.Load("settings.xml");
            XmlNodeList xNodeList = xml.SelectNodes("/Variables/DataFile");
            string df = xNodeList![0]!["Value"]!.InnerText;
            return df;
        }

        public static string UpdateURL()
        {
            xml.Load("settings.xml");
            XmlNodeList xNodeList = xml.SelectNodes("/Variables/UpdateURL");
            string uu = xNodeList![0]!["Value"]!.InnerText;
            return uu;
        }

        public static string Version()
        {
            xml.Load("settings.xml");
            XmlNodeList xNodeList = xml.SelectNodes("/Variables/Version");
            string v = xNodeList![0]!["Value"]!.InnerText;
            return v;
        }

        public static string ID()
        {
            xml.Load("settings.xml");
            XmlNodeList xNodeList = xml.SelectNodes("/Variables/ID");
            string id = xNodeList![0]!["Value"]!.InnerText;
            return id;
        }

        public static bool MKDI()
        {
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
            xml.Load("settings.xml");
            XmlNodeList b = xml.SelectNodes("/Variables/LETool");
            string c = b![0]!["Value"]!.InnerText;
            if(c == "true")
            {
                return true;
            }
            return false;
        }
    }
}