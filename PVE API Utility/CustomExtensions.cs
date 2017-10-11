﻿/*
 * CustomExtension.cs
 * Extensions for sending / searching XML strings and adding color support for RichTextBoxes.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace PVEAPIUtility
{
    namespace CustomExtensions
    {
        /// <summary>
        /// Extensions for sending / searching XML strings and adding color support for RichTextBoxes
        /// </summary>
        public static class CustomExtensions
        {
            /// <summary>
            /// Send an XML query to the server.
            /// </summary>
            /// <param name="xmlString">XML-formatted query to send.</param>
            /// <param name="hostURL">URL to send the query to.</param>
            /// <returns></returns>
            public static string SendXml(this string xmlString, string hostURL)
            {
                if (string.IsNullOrEmpty(hostURL))
                {
                    throw new ArgumentNullException("hostURL");
                }

                WebClient client = new WebClient()
                {
                    Credentials = CredentialCache.DefaultNetworkCredentials
                };
                hostURL = APIHelper.SanitizeURL(hostURL);
                try
                {
                    return client.UploadString(hostURL, xmlString);
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
            /// Parses XML in the form of a string, searches for a specific XML node and returns the value of the XML node.
            /// </summary>
            /// <param name="xmlString"></param>
            /// <param name="xmlNode"></param>
            /// <returns></returns>
            public static string TryFindXmlNode(this string xmlString, string xmlNode, out bool success)
            {
                StringBuilder output = new StringBuilder();
                try
                {
                    StringReader xml = new StringReader(xmlString);
                    XmlReader reader = XmlReader.Create(xml);
                    reader.ReadToFollowing(xmlNode);
                    output.AppendLine(reader.ReadElementContentAsString());
                    success = true;
                    return output.ToString().Trim();
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
            public static List<string> TryFindXmlNodes(this string xmlString, string xmlNode, out bool success)
            {
                List<string> nodeValues = new List<string>();
                try
                {
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
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    success = false;
                }

                success = true;
                return nodeValues;
            }

            /// <summary>
            /// Replaces only the first occurrence of the search text in the string.
            /// </summary>
            /// <param name="text"></param>
            /// <param name="search"></param>
            /// <param name="replace"></param>
            /// <returns></returns>
            public static string ReplaceFirst(this string text, string search, string replace)
            {
                int pos = text.IndexOf(search);
                if (pos < 0)
                {
                    return text;
                }
                return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
            }

            /// <summary>
            /// Appends supplied text to RichTextBox with the specified color.
            /// </summary>
            /// <param name="box"></param>
            /// <param name="text"></param>
            /// <param name="color"></param>
            public static void AppendText(this RichTextBox box, string text, Color color)
            {
                box.SelectionStart = box.TextLength;
                box.SelectionLength = 0;
                box.SelectionColor = color;
                box.AppendText(text);
                box.SelectionColor = box.ForeColor;
            }
        }
    }
}