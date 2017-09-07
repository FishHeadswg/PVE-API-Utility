/*
 * XMLHelper.cs
 * Helper class for sending / searching XML.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;

namespace PVEAPIUtility
{
    public static class XMLHelper
    {
        /// <summary>
        /// Send an XML query to the server.
        /// </summary>
        /// <param name="hostURL">URL to send the query to.</param>
        /// <param name="xml">XML-formatted query to send.</param>
        /// <returns></returns>
        public static string SendXml(string hostURL, string xml)
        {
            WebClient client = new WebClient()
            {
                Credentials = CredentialCache.DefaultNetworkCredentials
            };
            hostURL = SanitizeURL(hostURL);
            try
            {
                return client.UploadString(hostURL, xml);
            }
            catch (Exception e)
            {
                if (e.Message != null && e.InnerException != null)
                    throw new Exception("*****ERROR*****" + Environment.NewLine + e.Message + Environment.NewLine + e.InnerException.Message);
                else if (e.Message != null)
                    throw new Exception("*****ERROR*****" + Environment.NewLine + e.Message);
                return null;
            }
        }

        /// <summary>
        /// Parses XML in the form of a string, searches for a specific XML node, and returns the value of the XML node.
        /// </summary>
        /// <param name="xmlString"></param>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public static string TryFindXmlNode(string xmlString, string xmlNode, out bool success)
        {
            StringBuilder output = new StringBuilder();
            try
            {
                StringReader xml = new StringReader(xmlString);
                XmlReader reader = XmlReader.Create(xml);
                reader.ReadToFollowing(xmlNode);
                output.AppendLine(reader.ReadElementContentAsString());
                success = true;
                return output.ToString();
            }
            catch
            {
                success = false;
                return xmlString;
            }
        }

        /// <summary>
        /// Returns all values for a particular node (designed specifically for fields).
        /// </summary>
        /// <param name="xmlString"></param>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public static List<string> FindXmlNodes(string xmlString, string xmlNode)
        {
            List<string> nodeValues = new List<string>();
            using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
            {
                reader.ReadToFollowing("DOCUMENT_FIELDS");
                int count = Convert.ToInt32(reader.GetAttribute("COUNT"));
                for (int i = 0; i < count; ++i)
                {
                    reader.ReadToFollowing(xmlNode);
                    nodeValues.Add(reader.ReadElementContentAsString());
                }
            }

            return nodeValues;
        }

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
            string query = String.Format("<PVE><FUNCTION><NAME>ADLoadProject</NAME><PARAMETERS><ENTITYID>{0}</ENTITYID><SESSIONID>{1}</SESSIONID><SOURCEIP/><PROJID>{2}</PROJID></PARAMETERS></FUNCTION></PVE>", entID, sessID, projID);
            string response = XMLHelper.SendXml(url, query);
            success = false;
            string projattrs = XMLHelper.TryFindXmlNode(response, "PROJATTRS", out success);
            List<String> fields = new List<string>();
            if (success)
                fields = XMLHelper.FindXmlNodes(projattrs, "NAME");
            return fields;
        }

        /// <summary>
        /// Add httpinterface.aspx to URL (if needed).
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string SanitizeURL(string url)
        {
            if (url.Contains("/httpinterface.aspx"))
                return url;
            else if (url.EndsWith("/"))
                return string.Format("{0}httpinterface.aspx", url);
            else
                return string.Format("{0}/httpinterface.aspx", url);
        }
    }
}