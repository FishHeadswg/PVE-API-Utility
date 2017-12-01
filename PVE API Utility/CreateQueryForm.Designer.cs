namespace PVEAPIUtility
{
    partial class CreateQueryForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateQueryForm));
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblFName0 = new System.Windows.Forms.Label();
            this.lblOp0 = new System.Windows.Forms.Label();
            this.lblVal0 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.chkRCO = new System.Windows.Forms.CheckBox();
            this.txtValue0 = new System.Windows.Forms.TextBox();
            this.cmbOp0 = new System.Windows.Forms.ComboBox();
            this.nupProjID = new System.Windows.Forms.NumericUpDown();
            this.cmbSort = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.condPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.cmbField0 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbSearchType = new System.Windows.Forms.ComboBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.cmbFTR = new System.Windows.Forms.ComboBox();
            this.ToolTipFTR = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.nupProjID)).BeginInit();
            this.condPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(110, 276);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 23);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Project ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Fields To Return";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Conditions:";
            // 
            // lblFName0
            // 
            this.lblFName0.AutoSize = true;
            this.lblFName0.Location = new System.Drawing.Point(3, 0);
            this.lblFName0.Name = "lblFName0";
            this.lblFName0.Padding = new System.Windows.Forms.Padding(6, 7, 21, 0);
            this.lblFName0.Size = new System.Drawing.Size(87, 20);
            this.lblFName0.TabIndex = 4;
            this.lblFName0.Text = "Field Name";
            // 
            // lblOp0
            // 
            this.lblOp0.AutoSize = true;
            this.lblOp0.Location = new System.Drawing.Point(3, 27);
            this.lblOp0.Name = "lblOp0";
            this.lblOp0.Padding = new System.Windows.Forms.Padding(6, 7, 33, 0);
            this.lblOp0.Size = new System.Drawing.Size(87, 20);
            this.lblOp0.TabIndex = 5;
            this.lblOp0.Text = "Operator";
            // 
            // lblVal0
            // 
            this.lblVal0.AutoSize = true;
            this.lblVal0.Location = new System.Drawing.Point(3, 54);
            this.lblVal0.Name = "lblVal0";
            this.lblVal0.Padding = new System.Windows.Forms.Padding(6, 7, 46, 0);
            this.lblVal0.Size = new System.Drawing.Size(86, 20);
            this.lblVal0.TabIndex = 6;
            this.lblVal0.Text = "Value";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 227);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Return Count Only";
            // 
            // chkRCO
            // 
            this.chkRCO.AutoSize = true;
            this.chkRCO.Location = new System.Drawing.Point(110, 227);
            this.chkRCO.Name = "chkRCO";
            this.chkRCO.Size = new System.Drawing.Size(15, 14);
            this.chkRCO.TabIndex = 10;
            this.chkRCO.UseVisualStyleBackColor = true;
            // 
            // txtValue0
            // 
            this.txtValue0.Location = new System.Drawing.Point(95, 57);
            this.txtValue0.Name = "txtValue0";
            this.txtValue0.Size = new System.Drawing.Size(100, 20);
            this.txtValue0.TabIndex = 7;
            // 
            // cmbOp0
            // 
            this.cmbOp0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOp0.FormattingEnabled = true;
            this.cmbOp0.Items.AddRange(new object[] {
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
            "NOTIN"});
            this.cmbOp0.Location = new System.Drawing.Point(96, 30);
            this.cmbOp0.Name = "cmbOp0";
            this.cmbOp0.Size = new System.Drawing.Size(141, 21);
            this.cmbOp0.TabIndex = 6;
            // 
            // nupProjID
            // 
            this.nupProjID.Location = new System.Drawing.Point(110, 7);
            this.nupProjID.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nupProjID.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nupProjID.Name = "nupProjID";
            this.nupProjID.Size = new System.Drawing.Size(50, 20);
            this.nupProjID.TabIndex = 1;
            this.nupProjID.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nupProjID.ValueChanged += new System.EventHandler(this.ProjChanged);
            // 
            // cmbSort
            // 
            this.cmbSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSort.FormattingEnabled = true;
            this.cmbSort.Location = new System.Drawing.Point(110, 200);
            this.cmbSort.Name = "cmbSort";
            this.cmbSort.Size = new System.Drawing.Size(100, 21);
            this.cmbSort.TabIndex = 9;
            this.cmbSort.Click += new System.EventHandler(this.CmbSort_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 203);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Sort By";
            // 
            // condPanel
            // 
            this.condPanel.AutoScroll = true;
            this.condPanel.Controls.Add(this.lblFName0);
            this.condPanel.Controls.Add(this.cmbField0);
            this.condPanel.Controls.Add(this.lblOp0);
            this.condPanel.Controls.Add(this.cmbOp0);
            this.condPanel.Controls.Add(this.lblVal0);
            this.condPanel.Controls.Add(this.txtValue0);
            this.condPanel.Location = new System.Drawing.Point(15, 84);
            this.condPanel.Name = "condPanel";
            this.condPanel.Size = new System.Drawing.Size(257, 81);
            this.condPanel.TabIndex = 4;
            // 
            // cmbField0
            // 
            this.cmbField0.FormattingEnabled = true;
            this.cmbField0.Location = new System.Drawing.Point(96, 3);
            this.cmbField0.Name = "cmbField0";
            this.cmbField0.Size = new System.Drawing.Size(141, 21);
            this.cmbField0.TabIndex = 8;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(110, 56);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Add Condition";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.BtnAddCond_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 176);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "Search Type";
            // 
            // cmbSearchType
            // 
            this.cmbSearchType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSearchType.FormattingEnabled = true;
            this.cmbSearchType.Items.AddRange(new object[] {
            "OR",
            "AND"});
            this.cmbSearchType.Location = new System.Drawing.Point(109, 173);
            this.cmbSearchType.Name = "cmbSearchType";
            this.cmbSearchType.Size = new System.Drawing.Size(100, 21);
            this.cmbSearchType.TabIndex = 8;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(110, 247);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(80, 23);
            this.btnReset.TabIndex = 11;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // cmbFTR
            // 
            this.cmbFTR.FormattingEnabled = true;
            this.cmbFTR.Location = new System.Drawing.Point(110, 32);
            this.cmbFTR.Name = "cmbFTR";
            this.cmbFTR.Size = new System.Drawing.Size(142, 21);
            this.cmbFTR.TabIndex = 13;
            // 
            // CreateQueryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 305);
            this.Controls.Add(this.cmbFTR);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.cmbSearchType);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.condPanel);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbSort);
            this.Controls.Add(this.nupProjID);
            this.Controls.Add(this.chkRCO);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateQueryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create Search Query";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CreateQueryForm_FormClosed);
            this.Shown += new System.EventHandler(this.QueryForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nupProjID)).EndInit();
            this.condPanel.ResumeLayout(false);
            this.condPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblFName0;
        private System.Windows.Forms.Label lblOp0;
        private System.Windows.Forms.Label lblVal0;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkRCO;
        private System.Windows.Forms.TextBox txtValue0;
        private System.Windows.Forms.ComboBox cmbOp0;
        private System.Windows.Forms.NumericUpDown nupProjID;
        private System.Windows.Forms.ComboBox cmbSort;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.FlowLayoutPanel condPanel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbSearchType;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.ComboBox cmbField0;
        private System.Windows.Forms.ComboBox cmbFTR;
        private System.Windows.Forms.ToolTip ToolTipFTR;
    }
}