/*
 * APIHelper.cs
 * API helper methods.
 */

using System;
using System.Collections.Generic;

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
            string query = $"<PVE><FUNCTION><NAME>ADLoadProject</NAME><PARAMETERS><ENTITYID>{entID}</ENTITYID><SESSIONID>{sessID}</SESSIONID><SOURCEIP/><PROJID>{projID}</PROJID></PARAMETERS></FUNCTION></PVE>";
            string response = query.SendXml(url);
            success = false;
            string projattrs = response.TryFindXmlNode("PROJATTRS", out success);
            List<String> fields = new List<string>();
            if (success)
                fields = projattrs.FindXmlNodes("NAME");
            return fields;
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