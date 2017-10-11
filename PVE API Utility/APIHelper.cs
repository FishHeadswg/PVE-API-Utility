/*
 * APIHelper.cs
 * API helper methods.
 */

using System;
using System.Collections.Generic;
using PVEAPIUtility.CustomExtensions;

namespace PVEAPIUtility
{
    public static class APIHelper
    {
        /// <summary>
        /// Builds and returns a list of fields for the project as well as a success variable.
        /// </summary>
        /// <param name="entID"></param>
        /// <param name="sessID"></param>
        /// <param name="projID"></param>
        /// <param name="url"></param>
        /// <param name="success"></param>
        /// <returns></returns>
        public static List<string> BuildFieldList(string entID, string sessID, string projID, string url, out bool success)
        {
            string query = BuildPVEQuery("ADLoadProject", new Dictionary<string, string> { { "ENTITYID", entID }, { "SESSIONID", sessID }, { "SOURCEIP", "" }, { "PROJID", projID }, });
            string response = query.SendXml(url);
            string projattrs = response.TryFindXmlNode("PROJATTRS", out success);
            List<String> fields = new List<string>();
            if (success)
                fields = projattrs.TryFindXmlNodes("NAME", out bool succ);
            return fields;
        }

        public static string BuildPVEQuery(string fName, Dictionary<string, string> parameters)
        {
            string sXML = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\" ?>";
            sXML += "<PVDM_HTTPINTERFACE>";
            sXML += "<FUNCTION>";
            sXML += "<NAME>" + fName + "</NAME>";
            sXML += "<PARAMETERS>";

            foreach (var kvp in parameters)
            {
                sXML += "<" + kvp.Key + ">";
                sXML += EncodeXMLString(kvp.Value);
                sXML += "</" + kvp.Key + ">";
            }

            sXML += "</PARAMETERS>";
            sXML += "</FUNCTION>";
            sXML += "</PVDM_HTTPINTERFACE>";

            return sXML;
        }

        /// <summary>
        /// Add httpinterface.aspx to URL (if needed).
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string SanitizeURL(string url)
        {
            if (url.Contains("/httpinterface.aspx"))
                return url;
            else if (url.EndsWith("/"))
                return $"{url}httpinterface.aspx";
            else
                return $"{url}/httpinterface.aspx";
        }

        private static string EncodeXMLString(string xmlString)
        {
            return xmlString
                .Replace(">", "&gt;")
                .Replace("<", "&lt;")
                .Replace("&", "&amp;");
        }
    }
}