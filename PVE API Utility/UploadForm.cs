/*
 * UploadForm.cs
 * Uploads selected files to a project with the specified index values.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PVEAPIUtility.CustomExtensions;

namespace PVEAPIUtility
{
    public partial class UploadForm : Form
    {
        private const int EM_SETCUEBANNER = 0x1501;
        private List<TextBox> condFields = new List<TextBox>();
        private List<TextBox> condVals = new List<TextBox>();
        private List<Label> labels = new List<Label>();
        private List<string> fieldList = new List<string>();
        private string entID;
        private string sessID;
        private string url;

        public UploadForm(string entID, string sessID, string url)
        {
            if (string.IsNullOrEmpty(entID) || string.IsNullOrEmpty(sessID) || string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException();
            }
            InitializeComponent();
            this.entID = entID;
            this.sessID = sessID;
            this.url = url;
        }

        public string[] CondFieldNames { get; set; }

        public string[] Values { get; set; }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)]string lParam);

        private void UploadForm_Load(object sender, EventArgs e)
        {
            BuildIndexes();
        }

        private void UploadForm_Show(object sender, EventArgs e)
        {
            SendMessage(txtFilePath.Handle, EM_SETCUEBANNER, 1, "Browse to file...");
        }

        private void BuildIndexes()
        {
            fieldList.Clear();
            fieldList = APIHelper.BuildFieldList(entID, sessID, nupProjID.Value.ToString(), url, out bool success);
            if (!success)
            {
                MessageBox.Show("Invalid Project ID");
                return;
            }

            indexTable.RowCount = fieldList.Count;
            int i = 0;
            foreach (string field in fieldList)
            {
                indexTable.Controls.Add(new Label() { Text = field + ":", Anchor = AnchorStyles.Right, AutoSize = true }, 0, i);
                indexTable.Controls.Add(new TextBox() { Dock = DockStyle.Fill }, 1, i);
                ++i;
            }
        }

        private string BuildUploadQuery()
        {
            string fieldNames = string.Empty;
            string fieldVals = string.Empty;
            string base64 = string.Empty;
            bool first = true;
            foreach (string field in fieldList)
            {
                if (fieldList.First() == field)
                    fieldNames += field;
                else
                    fieldNames += "|" + field;
            }

            foreach (Control c in indexTable.Controls)
            {
                if (c is TextBox && first)
                {
                    fieldVals += c.Text;
                    first = false;
                }
                else if (c is TextBox)
                {
                    fieldVals += "|" + c.Text;
                }
            }
            try
            {
                Byte[] fileBytes = File.ReadAllBytes(txtFilePath.Text);
                base64 = Convert.ToBase64String(fileBytes);
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to read file:\n" + e.Message);
            }

            /*
            XmlDocument xmlquery = new XmlDocument();
            XmlElement pve = xmlquery.CreateElement("PVE");
            xmlquery.AppendChild(pve);
            XmlElement function = xmlquery.CreateElement("FUNCTION");
            pve.AppendChild(function);
            XmlElement name = xmlquery.CreateElement("NAME");
            XmlText funcName = xmlquery.CreateTextNode("AttachNewDocToProjectEx");
            name.AppendChild(funcName);
            function.AppendChild(name);

            XmlElement parameters = xmlquery.CreateElement("PARAMETERS");

            XmlElement entid = xmlquery.CreateElement("ENTITYID");
            entid.AppendChild(xmlquery.CreateTextNode(entID));

            XmlElement sessIDNode = xmlquery.CreateElement("SESSIONID");
            sessIDNode.AppendChild(xmlquery.CreateTextNode(sessID));

            XmlElement paramNode = xmlquery.CreateElement("PARAMETERS");

            XmlElement srcIPNode = xmlquery.CreateElement("SOURCEIP");

            XmlElement projIDNode = xmlquery.CreateElement("PROJID");
            projIDNode.AppendChild(xmlquery.CreateTextNode(nupProjID.Value.ToString()));

            XmlElement fieldNNode = xmlquery.CreateElement("FIELDNAMES");
            fieldNNode.AppendChild(xmlquery.CreateTextNode(fieldNames));

            XmlElement fieldValNode = xmlquery.CreateElement("FIELDVALUES");
            fieldValNode.AppendChild(xmlquery.CreateTextNode(fieldVals));

            XmlElement ofnNode = xmlquery.CreateElement("ORIGINALFILENAME");
            ofnNode.AppendChild(xmlquery.CreateTextNode(Path.GetFileName(txtFilePath.Text)));

            XmlElement sfNode = xmlquery.CreateElement("SAVEDFILE");

            XmlElement ofnftNode = xmlquery.CreateElement("ORIGINALFILENAMEFT");

            XmlElement sfftNode = xmlquery.CreateElement("SAVEDFILEFT");

            XmlElement folderNode = xmlquery.CreateElement("ADDTOFOLDER");

            XmlElement ifdNode = xmlquery.CreateElement("INFILEDATA");
            ifdNode.SetAttribute("types:dt", "bin.base64");
            ifdNode.SetAttribute("xmlns:types", "urn:schemas-microsoft-com:datatypes");
            ifdNode.AppendChild(xmlquery.CreateTextNode(base64));

            XmlElement infftNode = xmlquery.CreateElement("INFILEDATAFT");*/

            return $@"<PVE><FUNCTION><NAME>AttachNewDocToProjectEx</NAME><PARAMETERS><ENTITYID>{entID}</ENTITYID><SESSIONID>{sessID}</SESSIONID><PARAMETERS/><SOURCEIP/><PROJID>{nupProjID.Value.ToString()}</PROJID><FIELDNAMES>{fieldNames}</FIELDNAMES><FIELDVALUES>{fieldVals}</FIELDVALUES><ORIGINALFILENAME>{Path.GetFileName(txtFilePath.Text)}</ORIGINALFILENAME><SAVEDFILE></SAVEDFILE><ORIGINALFILENAMEFT/><SAVEDFILEFT/><ADDTOFOLDER/><INFILEDATA types:dt=""bin.base64"" xmlns:types=""urn:schemas-microsoft-com:datatypes"">{base64}</INFILEDATA><INFILEDATAFT/></PARAMETERS></FUNCTION></PVE>";
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnBrowseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = fileDialog.FileName;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string response;
            string docID;
            bool success = false;
            if (txtFilePath.Text == string.Empty)
            {
                MessageBox.Show("No file specified.", "Upload Error");
                return;
            }

            response = BuildUploadQuery().SendXml(url);
            try
            {
                docID = response.TryFindXmlNode("NEWDOCID", out success);
                if (success)
                    MessageBox.Show($"Upload Successful:\n\nProject ID: {nupProjID.Value.ToString()}\nDocument ID: {docID}", "Upload Success");
                else
                    MessageBox.Show("Upload failed:\nCheck your query parameters and/or session ID.", "Upload Error");
                txtFilePath.Clear();
            }
            catch
            {
                MessageBox.Show("Upload Failed.\n\nEnsure your index fields are valid, the system has proper access to the file, and your session is not expired.", "Upload Error");
            }
        }

        private void NupProjID_ValueChanged(object sender, EventArgs e)
        {
            indexTable.Controls.Clear();
            indexTable.RowCount = 1;
            BuildIndexes();
        }
    }
}