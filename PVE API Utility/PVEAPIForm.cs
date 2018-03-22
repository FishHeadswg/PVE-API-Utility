/*
 * PVEAPIForm.cs
 * Utility's functions include logging in, constructing/sending API queries, opening documents, and attaching documents.
 * Follow the comments to complete the form (or simply search for *TO BE DONE*).
 */

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.String;

// *TO BE DONE*: You will need to add this DLL reference to the project to link the utility with PVE's COM interface.
////using PVDMSystem;

namespace PVEAPIUtility
{
    using CustomExtensions;
    using DocSearchSvc;

    /// <summary>
    /// Main form for the API utility.
    /// </summary>
    public partial class PVEAPIForm : Form
    {
        /// <summary>
        /// Colors used for returned queries.
        /// </summary>
        private readonly Color[] rainbow = new[] { Color.Black, Color.Green, Color.Blue, Color.Indigo };

        /// <summary>
        /// Query color iterator.
        /// </summary>
        private readonly IEnumerator rainbowIter;

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
        private Lazy<UploadForm> uploadForm;
        private Lazy<CustomQueryForm> customForm;
        private Lazy<APIForm> apiForm;

        public PVEAPIForm()
        {
            InitializeComponent();
            pingTimer.Tick += new EventHandler(PingSessionAsync);
            rainbowIter = rainbow.GetEnumerator();
        }

        /// <summary>
        /// Batch operations.
        /// </summary>
        public enum BatchOp : byte
        {
            View = 0,
            Email = 1,
            Export = 2,
            Print = 3
        }

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
        private void SetPingInterval(int value) => pingTimer.Interval = (int)(value * 0.75f * 1000);

        /// <summary>
        /// Returns the next query color.
        /// </summary>
        /// <returns></returns>
        public Color GetQueryColor()
        {
            if (!rainbowIter.MoveNext())
            {
                rainbowIter.Reset();
                rainbowIter.MoveNext();
            }

            return (Color)rainbowIter.Current;
        }

        /// <summary>
        /// Loads utility settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void PVEAPIForm_Load(object sender, EventArgs e)
        {
            await TryLoadLoginAsync();
            txtLineNum.Font = txtResponse.Font;
        }

        private void PVEAPIForm__Resize(object sender, EventArgs e) => AddLineNumbers();

        private void TxtResponse_SelectionChanged(object sender, EventArgs e)
        {
            Point pt = txtResponse.GetPositionFromCharIndex(txtResponse.SelectionStart);
            if (pt.X == 1)
            {
                AddLineNumbers();
            }
        }

        private void TxtResponse_VScroll(object sender, EventArgs e)
        {
            txtLineNum.Clear();
            AddLineNumbers();
            txtLineNum.Invalidate();
        }

        private void TxtResponse_TextChanged(object sender, EventArgs e)
        {
            if (txtResponse.Text != Empty)
            {
                AddLineNumbers();
            }
        }

        private void TxtResponse_FontChanged(object sender, EventArgs e)
        {
            txtLineNum.Font = txtResponse.Font;
            txtResponse.Select();
            AddLineNumbers();
        }

        private void LineNumberTextBox_MouseDown(object sender, MouseEventArgs e)
        {
            txtResponse.Select();
            txtLineNum.DeselectAll();
        }

        /// <summary>
        /// Attempt to log user in.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnLogIn_Click(object sender, EventArgs e)
        {
            btnLogIn.Enabled = false;
            ssbar.Visible = true;
            if (btnLogIn.Text != "Login")
            {
                btnLogIn.Text = ssLbl.Text = "Logging out...";
                ssbar.PerformStep();
                await TryKillSessionAsync();
                ssbar.PerformStep();
                CloseForms();
                ssLbl.Text = "Logged out.";
                btnLogIn.Text = "Login";
            }
            else
            {
                btnLogIn.Text = ssLbl.Text = "Logging in...";
                ssbar.PerformStep();
                if (chkSave.Checked)
                    await TrySaveLoginAsync();
                bool loggedin = await TryLoginAsync();
                ssbar.PerformStep();
                if (loggedin)
                {
                    btnLogIn.Text = "Logout";
                    ssLbl.Text = "Logged in.";
                }
                else
                {
                    btnLogIn.Text = "Login";
                    ssLbl.Text = "Logged out.";
                }

                ssbar.PerformStep();
            }

            btnLogIn.Enabled = true;
            ssbar.Visible = false;
            ssbar.Value = 0;
        }

        private void CloseForms()
        {
            if (createQueryForm != null)
                createQueryForm.Close();
            if (uploadForm?.IsValueCreated ?? false)
                uploadForm.Value.Close();
            if (customForm?.IsValueCreated ?? false)
                customForm.Value.Close();
            if (apiForm?.IsValueCreated ?? false)
                apiForm.Value.Close();
        }

        /// <summary>
        /// Save query results to XML (not a properly formatted XML, but don't worry about that)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSaveResults_Click(object sender, EventArgs e)
        {
            using (var saveDiag = new SaveFileDialog()
            {
                Filter = "XML|*.xml",
                Title = "Save Results",
                FileName = $"Query Results({DateTimeOffset.Now.ToString("MMM-dd-yyyy_HH-mm")})"
            })
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
                docViewer.Show(this);

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
        /// Open upload form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUpload_Click(object sender, EventArgs e)
        {
            if (uploadForm?.Value?.IsDisposed ?? true)
            {
                {
                    uploadForm = new Lazy<UploadForm>(() => new UploadForm(this));
                }
            }

            uploadForm.Value.ShowDialog(this);
        }

        /// <summary>
        /// Open customer query form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCustom_Click(object sender, EventArgs e)
        {
            if (customForm?.Value?.IsDisposed ?? true)
            {
                customForm = new Lazy<CustomQueryForm>(() => new CustomQueryForm(this));
            }

            if (customForm.Value.ShowDialog(this) == DialogResult.OK)
            {
                txtResponse.Focus();
                txtResponse.ScrollToCaret();
            }
        }

        /// <summary>
        /// Open API query builder.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBuildAPIQuery_Click(object sender, EventArgs e)
        {
            if (apiForm?.Value?.IsDisposed ?? true)
            {
                apiForm = new Lazy<APIForm>(() => new APIForm(this));
            }

            if (apiForm.Value.ShowDialog(this) == DialogResult.OK)
            {
                txtResponse.Focus();
                txtResponse.ScrollToCaret();
            }
        }

        /// <summary>
        /// Build search query and append response to results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnSendQuery_Click(object sender, EventArgs e)
        {
            btnSendQuery.Enabled = false;
            btnSendQuery.Text = ssLbl.Text = "Sending...";
            ssbar.Visible = true;
            try
            {
                BuildQuery();
                ssbar.PerformStep();
                docSearchVars.PVResponse = await QueryExecutionAsync(Url, docSearchVars.PVSession, docSearchVars.PVQuery);
                ssbar.PerformStep();
                var results = docSearchVars.PVResponse.PROJECTQUERYRESPONSES[0].SEARCHRESULT;
                txtResponse.AppendText(
                    $"{(createQueryForm.RCO ? $"<COUNT>{docSearchVars.PVResponse.PROJECTQUERYRESPONSES[0].RESULTCOUNT.ToString()}</COUNT>" : (results?.ToString() ?? "<DOCUMENTRESULTS>NO RESULTS</DOCUMENTRESULTS>"))}\n\n", GetQueryColor());
                txtResponse.Focus();
                ssbar.PerformStep();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show($"Query invalid. Check your query parameters:\n{ex.Message}", "Query Error");
            }
            finally
            {
                docSearchVars.PVResponse = new PVQUERYRESPONSE();
                btnSendQuery.Text = "Send Search Query";
                ssLbl.Text = "Logged in.";
                ssbar.Visible = false;
                ssbar.Value = 0;
                btnSendQuery.Enabled = true;
            }
        }

        /// <summary>
        /// Open the Create Query form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCreateQuery_Click(object sender, EventArgs e)
        {
            if (createQueryForm?.IsDisposed ?? true)
            {
                createQueryForm = new CreateQueryForm(this);
                btnSendQuery.Enabled = true;
            }

            createQueryForm.Show(this);
            Enabled = false;
        }

        /// <summary>
        /// Kill any open sessions when closing the main form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnFormClosing(object sender, FormClosingEventArgs e) => await TryKillSessionAsync();

        /// <summary>
        /// Copies the session ID to clipboard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCopy_Click(object sender, EventArgs e) => Clipboard.SetText(txtSessionID.Text);

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) => Close();

        /// <summary>
        /// Show About window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox abtForm = new AboutBox();
            abtForm.Location = new Point(
                Location.X + ((Width - abtForm.Width) / 2),
                Location.Y + ((Height - abtForm.Height) / 2));
            abtForm.ShowDialog(this);
        }

        /// <summary>
        /// Saves the login credentials in IsolatedStorage.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chkSave.Checked = true;
            await TrySaveLoginAsync();
        }

        private async void ChkSave_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSave.Checked)
                await TrySaveLoginAsync();
            else
                TryDeleteLogin();
        }

        private void APIGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(@"C:\Program Files (x86)\Digitech Systems\PVE API Utility\Docs\PaperVision_Enterprise_APIGuide.pdf");
            }
            catch (Exception ex) when (ex.Message.Contains("cannot find the file"))
            {
                MessageBox.Show("Utility is not installed or the installation is missing files.", "Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void ServicesGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(@"C:\Program Files (x86)\Digitech Systems\PVE API Utility\Docs\PaperVisionEnterpriseServices.pdf");
            }
            catch (Exception ex) when (ex.Message.Contains("cannot find the file"))
            {
                MessageBox.Show("Utility is not installed or the installation is missing files.", "Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void ClearToolStripMenuItem_Click(object sender, EventArgs e) => txtResponse.Clear();

        /// <summary>
        /// Populates line numbers.
        /// </summary>
        private void AddLineNumbers()
        {
            Point pt = new Point(1, 1);
            int firstLine = txtResponse.GetLineFromCharIndex(txtResponse.GetCharIndexFromPosition(pt));
            pt.X = txtResponse.ClientRectangle.Width;
            pt.Y = txtResponse.ClientRectangle.Height;
            int lastLine = txtResponse.GetLineFromCharIndex(txtResponse.GetCharIndexFromPosition(pt));
            txtLineNum.SelectionAlignment = HorizontalAlignment.Right;
            txtLineNum.Clear();
            try
            {
                for (int i = firstLine; i <= lastLine + 1; i++)
                {
                    txtLineNum.Text += $"{checked(i + 1)}\n";
                }
            }
            catch (OverflowException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Builds the LoginUserEx3 query
        /// </summary>
        /// <returns></returns>
        private string BuildLoginQuery()
        {
            // *TO BE DONE*: Pass the entity ID, user name, and password to the LoginUserEx3 call from the form controls
            var parameters = new Dictionary<string, string>
            {
                ["ENTITYID"] = EntID,
                ["USERNAME"] = Username,
                ["PASSWORD"] = Password,
                ["SOURCEIP"] = Empty,
                ["CORELICTYPE"] = Empty,
                ["CLIENTPINGABLE"] = "TRUE",
                ["REMOTEAUTH"] = "FALSE"
            };
            return APIHelper.BuildPVEQuery("LoginUserEx3", parameters);
        }

        /// <summary>
        /// Tries to log in using the provided credentials.
        /// </summary>
        /// <returns></returns>
        private async ValueTask<bool> TryLoginAsync()
        {
            if (IsNullOrWhiteSpace(txtUsername.Text) || IsNullOrWhiteSpace(txtURL.Text))
            {
                MessageBox.Show("Please specify a Server URL and/or User Name.", "Login Error");
                return false;
            }

            numEntID.Enabled = txtPW.Enabled = txtUsername.Enabled = txtURL.Enabled = false;
            EntID = numEntID.Value.ToString();
            Username = txtUsername.Text;
            Password = txtPW.Text;
            Url = txtURL.Text;
            string response;
            string exc = Empty;
            try
            {
                response = await BuildLoginQuery().SendXml(Url);
            }
            catch (Exception e)
            {
                response = Empty;
                exc = e.Message;
            }

            if (response == Empty)
            {
                MessageBox.Show("Your root URL is invalid.", "Login Error");
                numEntID.Enabled = txtPW.Enabled = txtUsername.Enabled = txtURL.Enabled = true;
                try
                {
                    if (!EventLog.SourceExists("Papervision API Utility"))
                        EventLog.CreateEventSource("Papervision API Utility", "Application");
                    EventLog.WriteEntry("Papervision API Utility", exc, EventLogEntryType.Warning);
                }
                catch
                {
                    MessageBox.Show("There was a security error when attempting to save to your event logs. Please re-run the utility as administrator.");
                }
                return false;
            }

            try
            {
                // *TO BE DONE*: Find the SESSIONID node
                SessionID = response.TryGetXmlNode("SESSIONID", out bool successSession);
                int ping = Convert.ToInt32(response.TryGetXmlNode("PINGTIME", out bool successPing));
                if (ping != 0)
                {
                    SetPingInterval(ping);
                    pingTimer.Start();
                }

                txtSessionID.Text = SessionID;
                btnCreateQuery.Enabled = btnBuildAPIQuery.Enabled = btnOpenDoc.Enabled = btnUpload.Enabled = btnCustom.Enabled = numDocID.Enabled = numProjID.Enabled = true;
                numEntID.Enabled = txtPW.Enabled = txtUsername.Enabled = txtURL.Enabled = false;
                return true;
            }
            catch
            {
                MessageBox.Show("Please verify that your login credentials are correct.", "Login Error");
                numEntID.Enabled = txtPW.Enabled = txtUsername.Enabled = txtURL.Enabled = true;
                return false;
            }
        }

        /// <summary>
        /// Tries to kill the session and updates controls.
        /// </summary>
        /// <returns></returns>
        private async ValueTask<bool> TryKillSessionAsync()
        {
            if (SessionID != null)
                try
                {
                    var parameters = new Dictionary<string, string>
                    {
                        ["ENTITYID"] = EntID,
                        ["SESSIONID"] = SessionID,
                        ["SOURCEIP"] = Empty
                    };
                    var query = APIHelper.BuildPVEQuery("KillSession", parameters);
                    await query.SendXml(Url);
                    btnCreateQuery.Enabled = btnOpenDoc.Enabled = btnUpload.Enabled = btnBuildAPIQuery.Enabled = btnCustom.Enabled = btnSendQuery.Enabled = numDocID.Enabled = numProjID.Enabled = false;
                    numEntID.Enabled = txtPW.Enabled = txtUsername.Enabled = txtURL.Enabled = true;
                    txtSessionID.Clear();
                    pingTimer.Stop();
                }
                catch
                {
                    Console.WriteLine("Unable to kill session.");
                    return false;
                }
            return true;
        }

        /// <summary>
        /// Build search service query.
        /// </summary>
        private void BuildQuery()
        {
            // Build/Reset search criteria
            docSearchVars.PVSession = new PVSESSION();
            docSearchVars.PVQuery = new PVQUERY();
            docSearchVars.PVProjQuery = new PVPROJECTQUERY();
            docSearchVars.PVField = new PVFIELD();
            docSearchVars.PVCond = new PVCONDITION();
            docSearchVars.PVConds = new PVCONDITION[createQueryForm.CondFieldNames.Length];
            docSearchVars.PVSort = new PVSORT();

            docSearchVars.PVSession.ENTITYID = Convert.ToInt32(EntID);
            docSearchVars.PVSession.SESSIONID = SessionID;
            docSearchVars.PVProjQuery.PROJECTID = createQueryForm.ProjID;
            docSearchVars.PVProjQuery.FIELDSTORETURN = ReturnFields(createQueryForm.ReturnFields);

            if (createQueryForm.CondFieldNames.Length > 1)
            {
                docSearchVars.PVCond.CONDITIONGROUP = true;
                docSearchVars.PVCond.CONDITIONCONNECTOR = createQueryForm.SearchType;
                for (int i = 0; i < createQueryForm.CondFieldNames.Length; ++i)
                {
                    docSearchVars.PVConds[i] = new PVCONDITION()
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
            docSearchVars.PVSort.SORTORDER = PVSORTORDER.ASCENDING;
            docSearchVars.PVProjQuery.RETURNCOUNTONLY = createQueryForm.RCO;
            docSearchVars.PVProjQuery.SORTFIELDS = new PVSORT[] { docSearchVars.PVSort };
            docSearchVars.PVQuery.PROJECTQUERIES = new PVPROJECTQUERY[] { docSearchVars.PVProjQuery };
        }

        /// <summary>
        /// Parse the return field(s).
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        private PVFIELD[] ReturnFields(string fields)
        {
            if (fields == string.Empty)
            {
                PVFIELD[] pvField = new PVFIELD[1];
                pvField[0] = new PVFIELD()
                {
                    FIELDNAME = "DOCID"
                };
                return pvField;
            }

            string[] flds = fields.Split(new string[] { ",", ", " }, StringSplitOptions.RemoveEmptyEntries);
            PVFIELD[] pvFields = new PVFIELD[flds.Length];
            int i = 0;
            foreach (string fld in flds)
            {
                pvFields[i] = new PVFIELD()
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
        private async Task<PVQUERYRESPONSE> QueryExecutionAsync(string conurl, PVSESSION session, PVQUERY query)
        {
            var myProjQueryResponse = new SEARCHResponse();

            // Create Document Search Client object for opening and closing the DSS for query searches
            var mySearchClient = new PVDOCUMENTSEARCHClient();

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
                myProjQueryResponse = await mySearchClient.SEARCHAsync(session, query);
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

            return myProjQueryResponse.Body.SEARCHResult;
        }

        /// <summary>
        /// Parse query operations and return the corresponding PVOPERATOR
        /// </summary>
        /// <param name="op">Operation type.</param>
        /// <returns></returns>
        private PVOPERATOR GetOp(string op)
        {
            if (IsNullOrEmpty(op)) throw new ArgumentNullException();

            return (PVOPERATOR)Enum.Parse(typeof(PVOPERATOR), op);
        }

        private async void PingSessionAsync(object sender, EventArgs e)
        {
            var parameters = new Dictionary<string, string>
            {
                ["ENTITYID"] = EntID,
                ["SESSIONID"] = SessionID,
                ["SOURCEIP"] = Empty
            };
            var pingQuery = APIHelper.BuildPVEQuery("PingSession", parameters);
            await pingQuery.SendXml(Url);
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
        /// <returns></returns>
        private async ValueTask<bool> TrySaveLoginAsync()
        {
            var isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

            try
            {
                if (!isoStore.FileExists("PVEAPIUtility.ini"))
                {
                    using (var isoStream = new IsolatedStorageFileStream("PVEAPIUtility.ini", FileMode.CreateNew, isoStore))
                    {
                        using (var writer = new StreamWriter(isoStream))
                        {
                            // Writes encrypted login info
                            await writer.WriteAsync(EncryptLoginInfo());
                            return true;
                        }
                    }
                }
                else
                {
                    using (var isoStream = new IsolatedStorageFileStream("PVEAPIUtility.ini", FileMode.Truncate, isoStore))
                    {
                        using (var writer = new StreamWriter(isoStream))
                        {
                            await writer.WriteAsync(EncryptLoginInfo());
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Attempt to load saved login data if it exists.
        /// </summary>
        /// <returns></returns>
        private async ValueTask<bool> TryLoadLoginAsync()
        {
            var isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

            try
            {
                if (isoStore.FileExists("PVEAPIUtility.ini"))
                {
                    using (var isoStream = new IsolatedStorageFileStream("PVEAPIUtility.ini", FileMode.Open, isoStore))
                    {
                        using (var reader = new StreamReader(isoStream))
                        {
                            var loginInfo = DecryptLoginInfo(await reader.ReadToEndAsync());
                            using (var strReader = new StringReader(loginInfo))
                            {
                                try
                                {
                                    txtURL.Text = await strReader.ReadLineAsync();
                                    txtUsername.Text = await strReader.ReadLineAsync();
                                    txtPW.Text = await strReader.ReadLineAsync();
                                    numEntID.Value = Convert.ToDecimal(await strReader.ReadLineAsync());
                                }
                                catch
                                {
                                    MessageBox.Show("Unable to read PVEAPIUtility.ini!", "I/O Error");
                                    return false;
                                }
                            }
                        }
                    }

                    chkSave.Checked = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Variables for constructing a DSS query.
        /// </summary>
        private struct DocSearchVars
        {
            public PVSESSION PVSession;
            public PVQUERY PVQuery;
            public PVPROJECTQUERY PVProjQuery;
            public PVFIELD PVField;
            public PVCONDITION PVCond;
            public PVCONDITION[] PVConds;
            public PVSORT PVSort;
            public PVQUERYRESPONSE PVResponse;
        }
    }
}