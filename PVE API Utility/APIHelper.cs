/*
 * APIHelper.cs
 * API helper methods.
 */

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PVEAPIUtility
{
    using CustomExtensions;

    public static class APIHelper
    {
        /// <summary>
        /// Builds and returns a list of indexes for the project.
        /// </summary>
        /// <param name="entID"></param>
        /// <param name="sessID"></param>
        /// <param name="projID"></param>
        /// <param name="url"></param>
        /// <returns>Tuple for fields and success.</returns>
        public static async Task<(List<string> Results, bool Success)> TryBuildIndexList(string entID, string sessID, string projID, string url)
        {
            string query = BuildPVEQuery("ADLoadProject", new Dictionary<string, string>
            {
                ["ENTITYID"] = entID,
                ["SESSIONID"] = sessID,
                ["SOURCEIP"] = string.Empty,
                ["PROJID"] = projID
            });
            string response = await query.SendXml(url);
            string projattrs = response.TryGetXmlNode("PROJATTRS", out bool _);
            var fields = new List<string>();
            if (projattrs != string.Empty)
                fields = projattrs.TryGetXmlNodes("NAME", out bool succ);
            return (fields, true);
        }

        /// <summary>
        /// Construct a basic PVE query using the method name and parameters (node, value). Values are XML-encoded.
        /// </summary>
        /// <param name="fName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string BuildPVEQuery(string fName, Dictionary<string, string> parameters)
        {
            var sXML = new StringBuilder(@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes"" ?>");
            sXML.Append($"<PVDM_HTTPINTERFACE><FUNCTION><NAME>{fName}</NAME><PARAMETERS>");
            foreach (var kvp in parameters)
            {
                sXML.Append($"<{kvp.Key}>{kvp.Value}</{kvp.Key}>");
            }

            sXML.Append("</PARAMETERS></FUNCTION></PVDM_HTTPINTERFACE>");
            return sXML.ToString();
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
    }
}