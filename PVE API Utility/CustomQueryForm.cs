/*
 * Simple form for submitting manual API queries.
 */

using System;
using System.Windows.Forms;

namespace PVEAPIUtility
{
    public partial class CustomQueryForm : Form
    {
        private PVEAPIForm mainForm;
        private string response;

        public CustomQueryForm(PVEAPIForm form, string url)
        {
            InitializeComponent();
            mainForm = form;
            RootURL = url;
        }

        public string RootURL { get; set; }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Hide();
        }

        /// <summary>
        /// Sends the XML query and prints it as an XDoxument (with encoded characters replaced for readability).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                response = txtXMLQuery.Text.SendXml(RootURL);
            }
            catch
            {
                throw;
            }

            response = System.Xml.Linq.XDocument.Parse(response).ToString();
            response = response.Replace("&gt;", ">");
            response = response.Replace("&lt;", "<");
            mainForm.txtResponse.AppendText(Environment.NewLine + "CUSTOM QUERY RESPONSE: " + Environment.NewLine + response + Environment.NewLine, mainForm.Rainbow[mainForm.NextColor()]);
            Hide();
            mainForm.txtResponse.Focus();
        }

        private void BtnXML_Click(object sender, EventArgs e)
        {
            try
            {
                txtXMLQuery.Text = System.Xml.Linq.XDocument.Parse(txtXMLQuery.Text).ToString();
            }
            catch
            {
                return;
            }
        }
    }
}