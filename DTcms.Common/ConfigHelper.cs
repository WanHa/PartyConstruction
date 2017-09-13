using System.Configuration;
using System.Xml;

namespace DTcms.Common
{
    public class ConfigHelper
    {
        public static string GetAppSettings(string key)
        {
            return ConfigurationManager.AppSettings[key].ToString().Trim();
        }
        public static void SetValue(XmlDocument xmlDocument, string selectPath, string key, string keyValue)
        {
            XmlNodeList xmlNodeList = xmlDocument.SelectNodes(selectPath);
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.Attributes["key"].Value.ToUpper().Equals(key.ToUpper()))
                {
                    xmlNode.Attributes["value"].Value = keyValue;
                    break;
                }
            }
        }

        public static void Set(XmlDocument xmlDocument, string selectPath, string key, string keyValue)
        {
            XmlNodeList xmlNodeList = xmlDocument.SelectNodes(selectPath);
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                foreach (XmlNode item in xmlNode.ChildNodes)
                {

                if (item.Attributes["key"].Value.ToUpper().Equals(key.ToUpper()))
                {
                        item.Attributes["value"].Value = keyValue;
                    break;
                }
                }
            }
        }

    }
}