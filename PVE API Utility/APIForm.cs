using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using static System.String;

namespace PVEAPIUtility
{
    using CustomExtensions;
    using System.Xml.Linq;

    public partial class APIForm : Form
    {
        private PVEAPIForm mainForm;
        private readonly MethodInfo[] pvActionQueries;
        private List<string> paramList = new List<string>();

        public APIForm(PVEAPIForm form)
        {
            mainForm = form ?? throw new ArgumentNullException();
            InitializeComponent();
            Type pvAction = Assembly.ReflectionOnlyLoad("Interop.PVDMSystem").GetModules()[0].Assembly.GetType("PVDMSystem.PVActionClass");
            pvActionQueries = pvAction.GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
               BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly).Where(x => x.GetParameters().Length > 2).OrderBy(x => x.Name).ToArray();
        }

        private void CmbQuery_SelectedIndexChanged(object sender, EventArgs e)
        {
            paramList.Clear();
            BuildParams((sender as ComboBox).SelectedItem as string);
        }

        private void APIForm_Load(object sender, EventArgs e)
        {
            PopulateQueries();
        }

        private void PopulateQueries()
        {
            var queryNames = new List<string>();
            foreach (var query in pvActionQueries)
            {
                queryNames.Add(query.Name);
            }
            cmbQuery.Items.AddRange(queryNames.ToArray());
        }

        private void BuildParams(string queryName)
        {
            paramTable.Controls.Clear();
            paramTable.RowCount = 0;
            var parameters = pvActionQueries.Single((method) => method.Name == queryName).GetParameters();
            int i = 0;
            foreach (var param in parameters)
            {
                ++paramTable.RowCount;
                paramTable.Controls.Add(new Label() { Text = $"{param.Name} ({param.ParameterType.ToString().Split('.')[1].TrimEnd(')')}):", Anchor = AnchorStyles.Right, AutoSize = true }, 0, i);
                if (param.Name == "SessionID")
                    paramTable.Controls.Add(new TextBox() { Dock = DockStyle.Fill, Text = mainForm.SessionID }, 1, i);
                else
                    paramTable.Controls.Add(new TextBox() { Dock = DockStyle.Fill, MaxLength = 0 }, 1, i);
                paramList.Add(param.Name);
                ++i;
            }
        }

        private void BtnClose_Click(object sender, EventArgs e) => Close();

        private async void BtnSend_Click(object sender, EventArgs e)
        {
            btnSend.Enabled = false;
            btnSend.Text = "Sending...";
            string response;
            try
            {
                response = await BuildQuery().SendXml(mainForm.Url);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Query Error");
                return;
            }
            finally
            {
                btnSend.Text = "Submit Query";
                btnSend.Enabled = true;
            }

            DialogResult = DialogResult.OK;
            Hide();
            response = XDocument.Parse(response).ToString();
            response = response
                .Replace("&gt;", ">")
                .Replace("&lt;", "<");
            mainForm.txtResponse.AppendText($"{response}\n\n", mainForm.GetQueryColor());
        }

        private string BuildQuery()
        {
            string paramNames = Empty;
            string paramVals = Empty;
            var parameters = new Dictionary<string, string>();
            int i = 0;
            try
            {
                foreach (Control c in paramTable.Controls)
                {
                    if (c is TextBox)
                    {
                        parameters.Add(paramList[i].ToUpper(), c.Text);
                        ++i;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return APIHelper.BuildPVEQuery(cmbQuery.SelectedItem as string, parameters);
        }
    }
}