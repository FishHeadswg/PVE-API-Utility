/*
 * CreateQueryForm.cs
 * Form for constructing search queries.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PVEAPIUtility
{
    public partial class CreateQueryForm : Form
    {
        private string entID;
        private string sessID;
        private string url;
        private int condCtr = 0;
        private List<ComboBox> condFields = new List<ComboBox>();
        private List<ComboBox> condOps = new List<ComboBox>();
        private List<TextBox> condVals = new List<TextBox>();
        private List<Label> labels = new List<Label>();

        public CreateQueryForm(string entityID, string sessionID, string hosturl)
        {
            if (string.IsNullOrEmpty(entityID) || string.IsNullOrEmpty(sessionID) || string.IsNullOrEmpty(hosturl))
            {
                throw new ArgumentNullException();
            }
            InitializeComponent();
            entID = entityID;
            sessID = sessionID;
            url = hosturl;
        }

        public int ProjID { get; set; }
        public string ReturnFields { get; set; }
        public string[] CondFieldNames { get; set; }
        public string[] Ops { get; set; }
        public string[] Values { get; set; }
        public string SearchType { get; set; }
        public string SortFieldName { get; set; }
        public bool RCO { get; set; }

        private void Form2_Load(object sender, EventArgs e)
        {
            QueryFormInit();
            ToolTipFTR.SetToolTip(cmbFTR, "Comma separated list of indexes to return.");
        }

        /// <summary>
        /// Populates the query window with controls.
        /// </summary>
        private async void QueryFormInit()
        {
            await TrySetCBox(cmbFTR);
            await TrySetCBox(cmbField0);
            condFields.Add(cmbField0);
            condOps.Add(new ComboBox());
            condOps[0] = cmbOp0;
            cmbOp0.SelectedIndex = 0;
            condVals.Add(new TextBox());
            condVals[0] = txtValue0;
            cmbSearchType.SelectedIndex = 0;
            ++condCtr;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            ProjID = Convert.ToInt32(nupProjID.Value);
            ReturnFields = cmbFTR.Text;
            CondFieldNames = new string[condFields.Count];
            Ops = new string[condFields.Count];
            Values = new string[condFields.Count];
            int i = 0;
            foreach (ComboBox cBox in condFields)
            {
                CondFieldNames[i] = cBox.Text;
                ++i;
            }

            i = 0;
            foreach (ComboBox cBox in condOps)
            {
                Ops[i] = cBox.Text;
                ++i;
            }

            i = 0;
            foreach (TextBox tBox in condVals)
            {
                Values[i] = tBox.Text;
                ++i;
            }

            SearchType = cmbSearchType.Text;
            SortFieldName = cmbSort.Text;
            RCO = chkRCO.Checked;
            this.Visible = false;
        }

        private void CmbSort_Click(object sender, EventArgs e)
        {
            string[] flds = cmbFTR.Text.Split(new string[] { ",", ", " }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < flds.Length; ++i)
                flds[i] = flds[i].Trim();
            this.cmbSort.Items.Clear();
            this.cmbSort.Items.AddRange(flds);
        }

        /// <summary>
        /// Add condition to the condition panel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnAddCond_Click(object sender, EventArgs e)
        {
            labels.Add(new Label());
            condPanel.Controls.Add(CreateLabel("field", labels[(3 * (condCtr - 1))]));
            condFields.Add(new ComboBox());
            await TrySetCBox(condFields[condCtr]);
            condPanel.Controls.Add(condFields[condCtr]);
            labels.Add(new Label());
            condPanel.Controls.Add(CreateLabel("operator", labels[3 * (condCtr - 1) + 1]));
            condOps.Add(new ComboBox());
            condPanel.Controls.Add(WomboCombo("operator", condOps[condCtr]));
            labels.Add(new Label());
            condPanel.Controls.Add(CreateLabel("value", labels[3 * (condCtr - 1) + 2]));
            condVals.Add(new TextBox());
            condPanel.Controls.Add(condVals[condCtr]);
            ++condCtr;
        }

        /// <summary>
        /// Creates new operator combobox.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="cBox"></param>
        /// <returns>Returns new Operator combobox.</returns>
        private ComboBox WomboCombo(string type, ComboBox cBox)
        {
            if (string.IsNullOrEmpty(type) || cBox == null)
            {
                throw new ArgumentNullException();
            }
            cBox.DropDownStyle = ComboBoxStyle.DropDownList;
            cBox.Items.AddRange(new[]
            {
            "EQUAL",
            "NOTEQUAL",
            "GREATERTHAN",
            "GREATERTHANOREQUAL",
            "LESSTHAN",
            "LESSTHANOREQUAL",
            "ISNULL",
            "ISNOTNULL",
            "LIKE",
            "NOTLIKE",
            "IN",
            "NOTIN"
            });
            cBox.Size = new System.Drawing.Size(141, 21);
            cBox.SelectedIndex = 0;
            return cBox;
        }

        /// <summary>
        /// Set combobox properties to populate project fields.
        /// </summary>
        /// <param name="cBox"></param>
        private async Task<bool> TrySetCBox(ComboBox cBox)
        {
            if (cBox == null)
            {
                throw new ArgumentNullException();
            }

            var projectFields = await APIHelper.TryBuildFieldList(entID, sessID, nupProjID.Value.ToString(), url);
            if (projectFields.Count == 0)
            {
                MessageBox.Show("Invalid Project ID.");
                return false;
            }

            var projectFieldsArray = projectFields.ToArray();
            var autocomp = new AutoCompleteStringCollection();
            cBox.Items.Clear();
            cBox.Items.AddRange(projectFieldsArray);
            cBox.SelectedIndex = 0;
            autocomp.AddRange(projectFieldsArray);
            cBox.AutoCompleteCustomSource = autocomp;
            cBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            return true;
        }

        /// <summary>
        /// Update comboboxes when changing project.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ProjChanged(object sender, EventArgs e)
        {
            if (await TrySetCBox(cmbFTR))
            {
                foreach (ComboBox cmbField in condFields)
                {
                    await TrySetCBox(cmbField);
                }
            }
        }

        private Label CreateLabel(string type, Label label)
        {
            switch (type)
            {
                case "field":
                    label.Padding = new System.Windows.Forms.Padding(6, 7, 21, 0);
                    label.Size = new System.Drawing.Size(87, 20);
                    label.Text = "Field " + Convert.ToInt32(condCtr + 1);
                    return label;

                case "operator":
                    label.Padding = new System.Windows.Forms.Padding(6, 7, 33, 0);
                    label.Size = new System.Drawing.Size(87, 20);
                    label.Text = "Operator";
                    return label;

                case "value":
                    label.Padding = new System.Windows.Forms.Padding(6, 7, 46, 0);
                    label.Size = new System.Drawing.Size(86, 20);
                    label.Text = "Value";
                    return label;

                default:
                    return label;
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            ResetControls();
        }

        private void ResetControls()
        {
            foreach (Control ctrl in Controls)
            {
                switch (ctrl)
                {
                    case TextBox tBox:
                        tBox.Text = null;
                        break;

                    case ComboBox cBox:
                        if (cBox.Items.Count > 0 && cBox != cmbField0)
                        {
                            cBox.SelectedIndex = 0;
                        }
                        break;

                    case CheckBox chkBox:
                        chkBox.Checked = false;
                        break;

                    case FlowLayoutPanel flp:
                        List<Control> listControls = flp.Controls.Cast<Control>().ToList();

                        foreach (Control flowControl in listControls)
                        {
                            if (flowControl.Name == "")
                            {
                                condPanel.Controls.Remove(flowControl);
                                flowControl.Dispose();
                                continue;
                            }

                            if (flowControl is TextBox tBox2)
                            {
                                tBox2.Text = null;
                            }

                            if (flowControl is ComboBox cBox2)
                            {
                                if (cBox2.Items.Count > 0)
                                    cBox2.SelectedIndex = 0;
                            }
                        }
                        break;

                    default:
                        break;
                }
            }

            labels.Clear();
            condFields.Clear();
            condOps.Clear();
            condVals.Clear();
            condCtr = 0;
            QueryFormInit();
        }
    }
}