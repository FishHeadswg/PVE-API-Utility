/*
 * UploadForm.cs
 * Uploads selected files to a project with the specified index values.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.String;

namespace PVEAPIUtility
{
    using CustomExtensions;

    public partial class UploadForm : Form
    {
        private const int EM_SETCUEBANNER = 0x1501;
        private PVEAPIForm mainForm;
        private List<string> indexList = new List<string>();

        public UploadForm(PVEAPIForm form)
        {
            mainForm = form ?? throw new ArgumentNullException();
            InitializeComponent();
        }

        public string[] CondFieldNames { get; set; }

        public string[] Values { get; set; }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)]string lParam);

        private async void UploadForm_Load(object sender, EventArgs e) => await BuildIndexesAsync();

        private void UploadForm_Show(object sender, EventArgs e)
        {
            SendMessage(txtFilePath.Handle, EM_SETCUEBANNER, 1, "Browse to file...");
        }

        private async Task BuildIndexesAsync()
        {
            nupProjID.Enabled = false;
            indexList.Clear();
            var (Results, _) = await APIHelper.TryBuildIndexList(mainForm.EntID, mainForm.SessionID, nupProjID.Value.ToString(), mainForm.Url);
            indexList = Results;
            if (indexList.Count == 0)
            {
                MessageBox.Show("Invalid Project ID", "Upload Error");
                nupProjID.Enabled = true;
                return;
            }

            indexTable.RowCount = indexList.Count;
            int i = 0;
            foreach (string field in indexList)
            {
                indexTable.Controls.Add(new Label() { Text = $"{field}:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, i);
                indexTable.Controls.Add(new TextBox() { Dock = DockStyle.Fill }, 1, i);
                ++i;
            }

            nupProjID.Enabled = true;
        }

        private string BuildUploadQuery()
        {
            string indexNames = Empty;
            string indexVals = Empty;
            string base64 = Empty;
            bool first = true;
            foreach (string index in indexList)
            {
                if (indexList.First() == index)
                    indexNames += index;
                else
                    indexNames += $"|{index}";
            }

            foreach (Control c in indexTable.Controls)
            {
                if (c is TextBox && first)
                {
                    indexVals += c.Text;
                    first = false;
                }
                else if (c is TextBox)
                {
                    indexVals += $"|{c.Text}";
                }
            }

            try
            {
                Byte[] fileBytes = File.ReadAllBytes(txtFilePath.Text);
                base64 = Convert.ToBase64String(fileBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show($"Unable to read file:\n{ex.Message}", "I/O Error");
                throw ex;
            }

            var parameters = new Dictionary<string, string>
            {
                ["ENTITYID"] = mainForm.EntID,
                ["SESSIONID"] = mainForm.SessionID,
                ["PARAMETERS"] = Empty,
                ["SOURCEIP"] = Empty,
                ["PROJID"] = nupProjID.Value.ToString(),
                ["FIELDNAMES"] = indexNames,
                ["FIELDVALUES"] = indexVals,
                ["ORIGINALFILENAME"] = Path.GetFileName(txtFilePath.Text),
                ["SAVEDFILE"] = Empty,
                ["ORIGINALFILENAMEFT"] = Empty,
                ["SAVEDFILEFT"] = Empty,
                ["ADDTOFOLDER"] = Empty,
                ["INFILEDATA"] = base64,
                ["INFILEDATAFT"] = Empty
            };
            var query = APIHelper.BuildPVEQuery("AttachNewDocToProjectEx", parameters);

            return query.ReplaceFirst("INFILEDATA", @"INFILEDATA types:dt=""bin.base64"" xmlns:types=""urn:schemas-microsoft-com:datatypes""");
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
            btnUpload.Enabled = false;
            btnUpload.Text = "Uploading...";
            if (txtFilePath.Text == Empty)
            {
                MessageBox.Show("No file specified.", "Upload Error");
                btnUpload.Text = "Upload";
                btnUpload.Enabled = true;
                return;
            }

            try
            {
                string response = await BuildUploadQuery().SendXml(mainForm.Url);
                string docID = response.TryGetXmlNode("NEWDOCID", out bool success);
                if (docID != Empty)
                    MessageBox.Show($"Upload Successful:\n\nProject ID: {nupProjID.Value.ToString()}\nDocument ID: {docID}", "Upload Success");
                else
                    MessageBox.Show("Upload failed:\nCheck your query parameters and/or session ID.", "Upload Error");
                txtFilePath.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("Upload Failed.\n\nEnsure your index fields are valid, the system has proper access to the file, and your session is not expired.", "Upload Error");
            }
            finally
            {
                btnUpload.Text = "Upload";
                btnUpload.Enabled = true;
            }
        }

        private async void NupProjID_ValueChanged(object sender, EventArgs e)
        {
            indexTable.Controls.Clear();
            indexTable.RowCount = 1;
            await BuildIndexesAsync();
        }

        private void UploadForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Enabled = true;
        }
    }
}