/*
 * Simple form for submitting manual API queries.
 */

using System;
using System.Windows.Forms;
using System.Xml.Linq;
using PVEAPIUtility.CustomExtensions;
using System.Text;

namespace PVEAPIUtility
{
    public partial class CustomQueryForm : Form
    {
        private PVEAPIForm mainForm;
        private string response;

        public CustomQueryForm(PVEAPIForm form, string url)
        {
            if (form == null || string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException();
            }
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
        /// Sends the XML query and prints it as an XDocument (with encoded characters replaced for readability).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnSubmit_Click(object sender, EventArgs e)
        {
            btnSubmit.Text = "Sending...";
            try
            {
                response = await txtXMLQuery.Text.SendXml(RootURL);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Query Error");
                return;
            }
            finally
            {
                btnSubmit.Text = "Submit Query";
            }
            response = XDocument.Parse(response).ToString();
            response = response
                .Replace("&gt;", ">")
                .Replace("&lt;", "<");
            mainForm.txtResponse.AppendText(Environment.NewLine + "CUSTOM QUERY RESPONSE: " + Environment.NewLine + response + Environment.NewLine, mainForm.Rainbow[mainForm.NextColor()]);
            Hide();
            mainForm.txtResponse.Focus();
        }

        private void BtnXML_Click(object sender, EventArgs e)
        {
            try
            {
                txtXMLQuery.Text = XDocument.Parse(txtXMLQuery.Text).ToString();
            }
            catch
            {
                return;
            }
        }
    }
}