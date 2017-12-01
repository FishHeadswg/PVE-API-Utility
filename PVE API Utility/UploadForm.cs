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
using System.Threading.Tasks;

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

        private async void UploadForm_Load(object sender, EventArgs e)
        {
            await BuildIndexesAsync();
        }

        private void UploadForm_Show(object sender, EventArgs e)
        {
            SendMessage(txtFilePath.Handle, EM_SETCUEBANNER, 1, "Browse to file...");
        }

        private async Task BuildIndexesAsync()
        {
            nupProjID.Enabled = false;
            fieldList.Clear();
            fieldList = await APIHelper.TryBuildFieldList(entID, sessID, nupProjID.Value.ToString(), url);
            if (fieldList.Count == 0)
            {
                MessageBox.Show("Invalid Project ID", "Upload Error");
                nupProjID.Enabled = true;
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
            nupProjID.Enabled = true;
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
                MessageBox.Show("Unable to read file:\n" + e.Message, "I/O Error");
            }

            var parameters = new Dictionary<string, string> { { "ENTITYID", entID }, { "SESSIONID", sessID }, { "PARAMETERS", "" }, { "SOURCEIP", "" }, { "PROJID", nupProjID.Value.ToString() }, { "FIELDNAMES", fieldNames }, { "FIELDVALUES", fieldVals }, { "ORIGINALFILENAME", Path.GetFileName(txtFilePath.Text) }, { "SAVEDFILE", "" }, { "ORIGINALFILENAMEFT", "" }, { "SAVEDFILEFT", "" }, { "ADDTOFOLDER", "" }, { "INFILEDATA", base64 }, { "INFILEDATAFT", "" } };
            var query = APIHelper.BuildPVEQuery("AttachNewDocToProjectEx", parameters);

            return query.ReplaceFirst("INFILEDATA", @"INFILEDATA types:dt=""bin.base64"" xmlns:types=""urn:schemas-microsoft-com:datatypes""");
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnBrowseFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog())
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = fileDialog.FileName;
                }
        }

        private async void BtnUpload_Click(object sender, EventArgs e)
        {
            btnUpload.Text = "Uploading...";
            string response;
            string docID;
            if (txtFilePath.Text == string.Empty)
            {
                MessageBox.Show("No file specified.", "Upload Error");
                btnUpload.Text = "Upload";
                return;
            }

            response = await BuildUploadQuery().SendXml(url);
            try
            {
                docID = response.TryGetXmlNode("NEWDOCID");
                if (docID != string.Empty)
                    MessageBox.Show($"Upload Successful:\n\nProject ID: {nupProjID.Value.ToString()}\nDocument ID: {docID}", "Upload Success");
                else
                    MessageBox.Show("Upload failed:\nCheck your query parameters and/or session ID.", "Upload Error");
                txtFilePath.Clear();
            }
            catch
            {
                MessageBox.Show("Upload Failed.\n\nEnsure your index fields are valid, the system has proper access to the file, and your session is not expired.", "Upload Error");
            }
            finally
            {
                btnUpload.Text = "Upload";
            }
        }

        private async void NupProjID_ValueChanged(object sender, EventArgs e)
        {
            indexTable.Controls.Clear();
            indexTable.RowCount = 1;
            await BuildIndexesAsync();
        }
    }
}