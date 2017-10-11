/*
 * PVEAPIForm.cs
 * Utility's functions include logging in, constructing/sending API queries, opening documents, and attaching documents.
 * Follow the comments to complete the form (or simply search for *TO BE DONE*).
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Windows.Forms;
using Newtonsoft.Json;
using PVEAPIUtility.CustomExtensions;

// *TO BE DONE*: You will need to add this DLL reference to the project to link the utility with PVE's COM interface.
//// using PVDMSystem;

namespace PVEAPIUtility
{
    /// <summary>
    /// Main form for the API utility.
    /// </summary>
    public partial class PVEAPIForm : Form
    {
        /// <summary>
        /// Cycles through colors for returned queries to help separate them.
        /// </summary>
        private int rainbowCount;

        /// <summary>
        /// Keeps track of whether the create query window has been created already.
        /// </summary>
        private bool createOpen;

        /// <summary>
        /// Keeps track of whether the upload window has been created already.
        /// </summary>
        private bool uploadOpen;

        /// <summary>
        /// Keeps track of whether the custom query window has been created already.
        /// </summary>
        private bool customOpen;

        /// <summary>
        /// Tracks if login info has been initialized.
        /// </summary>
        private bool initLoginInfo;

        /// <summary>
        /// Client ping timer (keeps session alive).
        /// </summary>
        private Timer pingTimer = new Timer();

        /// <summary>
        /// Variables for constructing a document search.
        /// </summary>
        private DocSearchVars docSearchVars;

        //// Child forms.

        private CreateQueryForm createQueryForm;
        private UploadForm uploadForm;
        private CustomQueryForm customForm;

        public PVEAPIForm()
        {
            InitializeComponent();
            pingTimer.Tick += new EventHandler(PingSession);
            Rainbow = new[] { Color.Black, Color.Green, Color.Blue, Color.Indigo };
        }

        /// <summary>
        /// Batch operations.
        /// </summary>
        public enum BatchOp
        {
            View = 0,
            Email = 1,
            Export = 2,
            Print = 3
        }

        /// <summary>
        /// Gets colors used for returned queries.
        /// </summary>
        public Color[] Rainbow { get; }

        /// <summary>
        /// Gets entity ID.
        /// </summary>
        public string EntID { get; private set; }

        /// <summary>
        /// Gets host URL.
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// Gets the session ID.
        /// </summary>
        public string SessionID { get; private set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        private string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        private string Password { get; set; }

        /// <summary>
        /// Sets the ping interval (75% of client ping).
        /// </summary>
        private int PingInterval
        {
            set
            {
                pingTimer.Interval = (int)(value * 0.75f * 1000);
            }
        }

        /// <summary>
        /// Returns the next query color.
        /// </summary>
        /// <returns></returns>
        public int NextColor()
        {
            if (rainbowCount == Rainbow.Length)
                rainbowCount = 0;
            return rainbowCount++;
        }

        /// <summary>
        /// Builds the LoginUserEx3 query
        /// </summary>
        /// <returns></returns>
        private string BuildLoginQuery()
        {
            // *TO BE DONE*: Pass the entity ID, user name, and password to the LoginUserEx3 call from the form controls
            var parameters = new Dictionary<string, string> { { "ENTITYID", EntID }, { "USERNAME", Username }, { "PASSWORD", Password }, { "SOURCEIP", "" }, { "CORELICTYPE", "" }, { "CLIENTPINGABLE", "TRUE" }, { "REMOTEAUTH", "FALSE" } };
            return APIHelper.BuildPVEQuery("LoginUserEx3", parameters);
        }

        /// <summary>
        /// Attempt to log user in.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLogIn_Click(object sender, EventArgs e)
        {
            if (btnLogIn.Text != "Login")
            {
                TryKillSession();
                btnLogIn.Text = "Login";
            }
            else
            {
                TryLogin();
                if (chkSave.Checked)
                    TrySaveLogin();
            }
        }

        /// <summary>
        /// Tries to log in using the provided credentials.
        /// </summary>
        private void TryLogin()
        {
            EntID = numEntID.Value.ToString();
            Username = txtUsername.Text;
            Password = txtPW.Text;
            Url = txtURL.Text;
            string response;

            try
            {
                response = BuildLoginQuery().SendXml(Url);
            }
            catch
            {
                MessageBox.Show("Your root URL is invalid.");
                return;
            }

            try
            {
                // *TO BE DONE*: Find the SESSIONID node
                SessionID = response.TryFindXmlNode("SESSIONID", out bool success);
                int ping = Convert.ToInt32(response.TryFindXmlNode("PINGTIME", out success));
                if (ping != 0)
                {
                    PingInterval = ping;
                    pingTimer.Start();
                }

                txtSessionID.Text = SessionID;
                btnCreateQuery.Enabled = btnOpenDoc.Enabled = btnUpload.Enabled = btnCustom.Enabled = numDocID.Enabled = numProjID.Enabled = true;
                numEntID.Enabled = txtPW.Enabled = txtUsername.Enabled = txtURL.Enabled = false;
                btnLogIn.Text = "Logout";
            }
            catch
            {
                MessageBox.Show("Please verify that your login credentials are correct.");
            }
        }

        /// <summary>
        /// Tries to kill the session and updates controls.
        /// </summary>
        private void TryKillSession()
        {
            if (SessionID != null)
                try
                {
                    var parameters = new Dictionary<string, string> { { "ENTITYID", EntID }, { "SESSIONID", SessionID }, { "SOURCEIP", "" } };
                    var query = APIHelper.BuildPVEQuery("KillSession", parameters);
                    query.SendXml(Url);
                    btnCreateQuery.Enabled = btnOpenDoc.Enabled = btnUpload.Enabled = btnCustom.Enabled = btnSendQuery.Enabled = numDocID.Enabled = numProjID.Enabled = false;
                    numEntID.Enabled = txtPW.Enabled = txtUsername.Enabled = txtURL.Enabled = true;
                    txtSessionID.Clear();
                    pingTimer.Stop();
                }
                catch
                {
                    return;
                }
        }

        /// <summary>
        /// Save query results to XML (not a properly formatted XML, but don't worry about that)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSaveResults_Click(object sender, EventArgs e)
        {
            var saveDiag = new SaveFileDialog()
            {
                Filter = "XML|*.xml",
                Title = "Save Results",
                FileName = "Query_Results_{" + DateTime.Now.ToString("MMM-dd-yyyy--HH-mm-ss") + "}"
            };
            if (saveDiag.ShowDialog() == DialogResult.OK)
            {
                using (var sw = new StreamWriter(saveDiag.FileName))
                    sw.Write(txtResponse.Text);
            }
        }

        /// <summary>
        /// // Open a document (can be opened in BBV [default browser or DocViewer (default)] or PVWA)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpenDoc_Click(object sender, EventArgs e)
        {
            var docViewer = new DocViewer();
            if (radioBBV.Checked)
            {
                String url = $"{Url}/PVEDocViewer.aspx?SessionID={SessionID}&EntID={Convert.ToString(numEntID.Value)}&ProjID={Convert.ToString(numProjID.Value)}&DocId={Convert.ToString(numDocID.Value)}&Page=1";
                docViewer.SetURL(url);
                docViewer.Show();

                // Uncomment to use default browser instead
                /*
                string url = String.Format("{0}/PVEDocViewer.aspx?SessionID={1}&EntID={2}&ProjID={3}&DocId={4}&Page=1", Url, sessid, Convert.ToString(numEntID.Value), Convert.ToString(numProjID.Value), Convert.ToString(numDocID.Value));
                ProcessStartInfo startBrowser = new ProcessStartInfo(url);
                Process.Start(startBrowser);
                */
            }
            else
            {
                var opInfo = new
                {
                    PROJECTID = Convert.ToString(numProjID.Value),  // Document project ID
                    BATCHOPID = BatchOp.View,                       // see the above enum
                    BATCHDOCIDS = "null",                           // used when using batch operation such as (print/email/export)
                    DOCIDS = Convert.ToString(numDocID.Value),      // used to show the doc. Leave empty if batch operation
                    INDEXNAMES = "index names pip delimited",       // used to populate the index grid
                    INDEXVALUES = "index values pip delimited",     // used to populate the index grid
                    SORTEDDOCIDS = ""                               // used by next/previous doc commands
                };
                String url = $"LaunchPVWA:op?entID={EntID}&sessID={SessionID}&user={Username}&server={Url}/HTTPInterface.aspx&pvAuth=&op=AXDocViewer&opInfo={JsonConvert.SerializeObject(opInfo)}";
                Process.Start(url);
            }
        }

        /// <summary>
        /// Open upload form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (!uploadOpen)
                {
                    uploadForm = new UploadForm(EntID, SessionID, Url);
                    uploadOpen = true;
                }

                uploadForm.Show();
            }
            catch
            {
                uploadForm = new UploadForm(EntID, SessionID, Url);
                uploadForm.Show();
                btnSendQuery.Enabled = true;
            }
        }

        /// <summary>
        /// Open customer query form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCustom_Click(object sender, EventArgs e)
        {
            try
            {
                if (!customOpen)
                {
                    customForm = new CustomQueryForm(this, Url);
                    customOpen = true;
                }
                else
                    customForm.RootURL = Url;
                customForm.Show();
            }
            catch
            {
                customForm = new CustomQueryForm(this, Url);
                customForm.Show();
                btnSendQuery.Enabled = true;
            }
        }

        /// <summary>
        /// Build search query and append response to results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSendQuery_Click(object sender, EventArgs e)
        {
            try
            {
                BuildQuery();
                docSearchVars.PVResponse = QueryExecution(Url, docSearchVars.PVSession, docSearchVars.PVQuery);
                txtResponse.AppendText(
                    (createQueryForm.RCO ? docSearchVars.PVResponse.PROJECTQUERYRESPONSES[0].RESULTCOUNT.ToString() :
                    docSearchVars.PVResponse.PROJECTQUERYRESPONSES[0].SEARCHRESULT.ToString()) + Environment.NewLine + Environment.NewLine,
                    Rainbow[NextColor()]);
                txtResponse.Focus();
                docSearchVars.PVResponse = new DocSearchSvc.PVQUERYRESPONSE();
            }
            catch (Exception ex)
            {
                try
                {
                    // Check if there were no query results simply because there were no matches
                    if (docSearchVars.PVResponse.PROJECTQUERYRESPONSES[0].RESULTCOUNT == 0)
                    {
                        txtResponse.AppendText(
                            docSearchVars.PVResponse.PROJECTQUERYRESPONSES[0].RESULTCOUNT.ToString() + Environment.NewLine + Environment.NewLine,
                            Rainbow[NextColor()]);
                        txtResponse.Focus();
                    }
                }
                catch
                {
                    MessageBox.Show("Query invalid. Check your query parameters:" + Environment.NewLine + ex.Message, "Query Error");
                    ////throw;
                }

                docSearchVars.PVResponse = new DocSearchSvc.PVQUERYRESPONSE();
            }
        }

        /// <summary>
        /// Build search service query.
        /// </summary>
        private void BuildQuery()
        {
            // Build/Reset search criteria
            docSearchVars.PVSession = new DocSearchSvc.PVSESSION();
            docSearchVars.PVQuery = new DocSearchSvc.PVQUERY();
            docSearchVars.PVProjQuery = new DocSearchSvc.PVPROJECTQUERY();
            docSearchVars.PVField = new DocSearchSvc.PVFIELD();
            docSearchVars.PVCond = new DocSearchSvc.PVCONDITION();
            docSearchVars.PVConds = new DocSearchSvc.PVCONDITION[createQueryForm.CondFieldNames.Length];
            docSearchVars.PVSort = new DocSearchSvc.PVSORT();

            docSearchVars.PVSession.ENTITYID = Convert.ToInt32(EntID);
            docSearchVars.PVSession.SESSIONID = SessionID;
            docSearchVars.PVProjQuery.PROJECTID = createQueryForm.ProjID;
            docSearchVars.PVProjQuery.FIELDSTORETURN = ReturnFields(createQueryForm.ReturnFields);

            if (createQueryForm.CondFieldNames.Length > 1)
            {
                docSearchVars.PVCond.CONDITIONGROUP = true;
                docSearchVars.PVCond.CONDITIONCONNECTOR = (createQueryForm.SearchType == "AND") ? DocSearchSvc.PVBOOLEAN.AND : DocSearchSvc.PVBOOLEAN.OR;
                for (int i = 0; i < createQueryForm.CondFieldNames.Length; ++i)
                {
                    docSearchVars.PVConds[i] = new DocSearchSvc.PVCONDITION()
                    {
                        FIELDNAME = createQueryForm.CondFieldNames[i],
                        OPERATOR = GetOp(createQueryForm.Ops[i]),
                        QUERYVALUE = createQueryForm.Values[i]
                    };
                }

                docSearchVars.PVCond.CONDITIONS = docSearchVars.PVConds;
            }
            else
            {
                docSearchVars.PVCond.FIELDNAME = createQueryForm.CondFieldNames[0];
                docSearchVars.PVCond.OPERATOR = GetOp(createQueryForm.Ops[0]);
                docSearchVars.PVCond.QUERYVALUE = createQueryForm.Values[0];
            }

            docSearchVars.PVProjQuery.CONDITION = docSearchVars.PVCond;
            docSearchVars.PVSort.FIELDNAME = (createQueryForm.SortFieldName == string.Empty) ? "DOCID" : createQueryForm.SortFieldName;
            docSearchVars.PVSort.SORTORDER = DocSearchSvc.PVSORTORDER.ASCENDING;
            docSearchVars.PVProjQuery.RETURNCOUNTONLY = createQueryForm.RCO;
            docSearchVars.PVProjQuery.SORTFIELDS = new DocSearchSvc.PVSORT[] { docSearchVars.PVSort };
            docSearchVars.PVQuery.PROJECTQUERIES = new DocSearchSvc.PVPROJECTQUERY[] { docSearchVars.PVProjQuery };
        }

        /// <summary>
        /// Parse the return field(s).
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        private DocSearchSvc.PVFIELD[] ReturnFields(string fields)
        {
            if (fields == string.Empty)
            {
                DocSearchSvc.PVFIELD[] pvField = new DocSearchSvc.PVFIELD[1];
                pvField[0] = new DocSearchSvc.PVFIELD()
                {
                    FIELDNAME = "DOCID"
                };
                return pvField;
            }

            string[] flds = fields.Split(new string[] { ",", ", " }, StringSplitOptions.RemoveEmptyEntries);
            DocSearchSvc.PVFIELD[] pvFields = new DocSearchSvc.PVFIELD[flds.Length];
            int i = 0;
            foreach (string fld in flds)
            {
                pvFields[i] = new DocSearchSvc.PVFIELD()
                {
                    FIELDNAME = fld.Trim()
                };
                ++i;
            }

            return pvFields;
        }

        /// <summary>
        /// Execute the built query.
        /// </summary>
        /// <param name="conurl"></param>
        /// <param name="session"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        private DocSearchSvc.PVQUERYRESPONSE QueryExecution(string conurl, DocSearchSvc.PVSESSION session, DocSearchSvc.PVQUERY query)
        {
            var myProjQueryResponse = new DocSearchSvc.PVQUERYRESPONSE();

            // Create Document Search Client object for opening and closing the DSS for query searches
            var mySearchClient = new DocSearchSvc.PVDOCUMENTSEARCHClient();

            // Set endpoint
            mySearchClient.Endpoint.Address = new EndpointAddress($"{conurl}/Services/DocumentSearch/DocumentSearch.svc");

            // Check for SSL (e.g., Silo).
            if (conurl.ToLower().StartsWith("https") || conurl.ToLower().StartsWith("login.imagesilo"))
            {
                mySearchClient.Endpoint.Binding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);
            }
            else
            {
                mySearchClient.Endpoint.Binding = new BasicHttpBinding();
            }

            // Send SOAP envelope
            try
            {
                mySearchClient.Open();
                myProjQueryResponse = mySearchClient.SEARCH(session, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (mySearchClient != null)
                {
                    // Close any open clients
                    mySearchClient.Close();
                }
            }

            return myProjQueryResponse;
        }

        /// <summary>
        /// Open the Create Query form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCreateQuery_Click(object sender, EventArgs e)
        {
            try
            {
                if (!createOpen)
                {
                    createQueryForm = new CreateQueryForm(EntID, SessionID, Url);
                    createOpen = true;
                }

                createQueryForm.Show();
                btnSendQuery.Enabled = true;
            }
            catch
            {
                createQueryForm = new CreateQueryForm(EntID, SessionID, Url);
                createQueryForm.Show();
                btnSendQuery.Enabled = true;
            }
        }

        /// <summary>
        /// Parse query operations and return the corresponding PVOPERATOR
        /// </summary>
        /// <param name="op">Operation type.</param>
        /// <returns></returns>
        private DocSearchSvc.PVOPERATOR GetOp(string op)
        {
            if (string.IsNullOrEmpty(op))
            {
                throw new ArgumentNullException();
            }
            return (DocSearchSvc.PVOPERATOR)Enum.Parse(typeof(DocSearchSvc.PVOPERATOR), op);
        }

        /// <summary>
        /// Kill any open sessions when closing the main form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            TryKillSession();
        }

        /// <summary>
        /// Copies the session ID to clipboard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtSessionID.Text);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Show About window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox abtForm = new AboutBox();
            abtForm.Location = new Point(
                this.Location.X + (this.Width - abtForm.Width) / 2,
                this.Location.Y + (this.Height - abtForm.Height) / 2);
            abtForm.Show(this);
        }

        private void PingSession(object sender, EventArgs e)
        {
            var parameters = new Dictionary<string, string> { { "ENTITYID", EntID }, { "SESSIONID", SessionID }, { "SOURCEIP", "" } };
            var pingQuery = APIHelper.BuildPVEQuery("PingSession", parameters);
            pingQuery.SendXml(Url);
        }

        /// <summary>
        /// Encrypt login info.
        /// </summary>
        /// <returns></returns>
        private string EncryptLoginInfo()
        {
            var salt = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), true)[0]).Value;
            var loginInfo = txtURL.Text + Environment.NewLine + txtUsername.Text + Environment.NewLine + txtPW.Text + Environment.NewLine + numEntID.Value.ToString();
            return RijndaelManagedEncryption.RijndaelManagedEncryption.EncryptRijndael(loginInfo, salt);
        }

        /// <summary>
        /// Decrypt saved login info.
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        private string DecryptLoginInfo(string loginInfo)
        {
            var salt = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), true)[0]).Value;
            return RijndaelManagedEncryption.RijndaelManagedEncryption.DecryptRijndael(loginInfo, salt);
        }

        private bool TryDeleteLogin()
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

            if (isoStore.FileExists("PVEAPIUtility.ini"))
            {
                isoStore.DeleteFile("PVEAPIUtility.ini");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Encrypt and store login info.
        /// </summary>
        private void TrySaveLogin()
        {
            var isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

            if (!isoStore.FileExists("PVEAPIUtility.ini"))
            {
                using (var isoStream = new IsolatedStorageFileStream("PVEAPIUtility.ini", FileMode.CreateNew, isoStore))
                {
                    using (var writer = new StreamWriter(isoStream))
                    {
                        // Writes encrypted login info
                        writer.Write(EncryptLoginInfo());
                    }
                }
            }
            else
            {
                using (var isoStream = new IsolatedStorageFileStream("PVEAPIUtility.ini", FileMode.Truncate, isoStore))
                {
                    using (var writer = new StreamWriter(isoStream))
                    {
                        writer.Write(EncryptLoginInfo());
                    }
                }
            }
        }

        /// <summary>
        /// Saves the login credentials in IsolatedStorage.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chkSave.Checked = true;
            TrySaveLogin();
        }

        /// <summary>
        /// Loads utility settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PVEAPIForm_Load(object sender, EventArgs e)
        {
            TryLoadLogin();
        }

        /// <summary>
        /// Attempt to load saved login data if it exists.
        /// </summary>
        private void TryLoadLogin()
        {
            var isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

            if (isoStore.FileExists("PVEAPIUtility.ini"))
            {
                using (var isoStream = new IsolatedStorageFileStream("PVEAPIUtility.ini", FileMode.Open, isoStore))
                {
                    using (var reader = new StreamReader(isoStream))
                    {
                        var loginInfo = DecryptLoginInfo(reader.ReadToEnd());
                        using (var strReader = new StringReader(loginInfo))
                        {
                            try
                            {
                                txtURL.Text = strReader.ReadLine();
                                txtUsername.Text = strReader.ReadLine();
                                txtPW.Text = strReader.ReadLine();
                                numEntID.Value = Convert.ToDecimal(strReader.ReadLine());
                            }
                            catch
                            {
                                MessageBox.Show("Unable to read PVEAPIUtility.ini!");
                            }
                        }
                    }
                }

                chkSave.Checked = true;
            }

            initLoginInfo = true;
        }

        private void ChkSave_CheckedChanged(object sender, EventArgs e)
        {
            if (initLoginInfo)
            {
                switch (chkSave.Checked)
                {
                    case true:
                        TrySaveLogin();
                        break;

                    case false:
                        TryDeleteLogin();
                        break;
                }

                return;
            }
        }

        private void APIGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(@"C:\Program Files (x86)\Digitech Systems\PVE API Utility\Docs\PaperVision_Enterprise_APIGuide.pdf");
            }
            catch
            {
                MessageBox.Show("Utility is not installed or the installation is missing files.", "Error");
            }
        }

        private void ServicesGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(@"C:\Program Files (x86)\Digitech Systems\PVE API Utility\Docs\PaperVisionEnterpriseServices.pdf");
            }
            catch
            {
                MessageBox.Show("Utility is not installed or the installation is missing files.", "Error");
            }
        }

        /// <summary>
        /// Variables for constructing a DSS query.
        /// </summary>
        private struct DocSearchVars
        {
            public DocSearchSvc.PVSESSION PVSession;
            public DocSearchSvc.PVQUERY PVQuery;
            public DocSearchSvc.PVPROJECTQUERY PVProjQuery;
            public DocSearchSvc.PVFIELD PVField;
            public DocSearchSvc.PVCONDITION PVCond;
            public DocSearchSvc.PVCONDITION[] PVConds;
            public DocSearchSvc.PVSORT PVSort;
            public DocSearchSvc.PVQUERYRESPONSE PVResponse;
        }
    }
}