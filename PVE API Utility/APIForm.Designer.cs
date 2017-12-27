namespace PVEAPIUtility
{
    partial class APIForm
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
            this.cmbQuery = new System.Windows.Forms.ComboBox();
            this.lblQuery = new System.Windows.Forms.Label();
            this.groupParams = new System.Windows.Forms.GroupBox();
            this.panelParams = new System.Windows.Forms.Panel();
            this.paramTable = new System.Windows.Forms.TableLayoutPanel();
            this.btnSend = new System.Windows.Forms.Button();
            this.groupParams.SuspendLayout();
            this.panelParams.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbQuery
            // 
            this.cmbQuery.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbQuery.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbQuery.FormattingEnabled = true;
            this.cmbQuery.Location = new System.Drawing.Point(57, 10);
            this.cmbQuery.Name = "cmbQuery";
            this.cmbQuery.Size = new System.Drawing.Size(415, 21);
            this.cmbQuery.TabIndex = 0;
            this.cmbQuery.SelectedIndexChanged += new System.EventHandler(this.CmbQuery_SelectedIndexChanged);
            // 
            // lblQuery
            // 
            this.lblQuery.AutoSize = true;
            this.lblQuery.Location = new System.Drawing.Point(13, 13);
            this.lblQuery.Name = "lblQuery";
            this.lblQuery.Size = new System.Drawing.Size(38, 13);
            this.lblQuery.TabIndex = 1;
            this.lblQuery.Text = "Query:";
            // 
            // groupParams
            // 
            this.groupParams.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupParams.Controls.Add(this.panelParams);
            this.groupParams.Location = new System.Drawing.Point(12, 37);
            this.groupParams.Name = "groupParams";
            this.groupParams.Size = new System.Drawing.Size(460, 384);
            this.groupParams.TabIndex = 18;
            this.groupParams.TabStop = false;
            this.groupParams.Text = "Query Parameters";
            // 
            // panelParams
            // 
            this.panelParams.AutoScroll = true;
            this.panelParams.Controls.Add(this.paramTable);
            this.panelParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelParams.Location = new System.Drawing.Point(3, 16);
            this.panelParams.Name = "panelParams";
            this.panelParams.Size = new System.Drawing.Size(454, 365);
            this.panelParams.TabIndex = 1;
            // 
            // paramTable
            // 
            this.paramTable.AutoSize = true;
            this.paramTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.paramTable.ColumnCount = 2;
            this.paramTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.36018F));
            this.paramTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.63982F));
            this.paramTable.Dock = System.Windows.Forms.DockStyle.Top;
            this.paramTable.Location = new System.Drawing.Point(0, 0);
            this.paramTable.Name = "paramTable";
            this.paramTable.RowCount = 1;
            this.paramTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.paramTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.paramTable.Size = new System.Drawing.Size(454, 0);
            this.paramTable.TabIndex = 0;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(210, 427);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(80, 23);
            this.btnSend.TabIndex = 19;
            this.btnSend.Text = "Submit Query";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.BtnSend_Click);
            // 
            // APIForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 462);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.groupParams);
            this.Controls.Add(this.lblQuery);
            this.Controls.Add(this.cmbQuery);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "APIForm";
            this.Text = "APIForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.APIForm_FormClosed);
            this.Load += new System.EventHandler(this.APIForm_Load);
            this.groupParams.ResumeLayout(false);
            this.panelParams.ResumeLayout(false);
            this.panelParams.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbQuery;
        private System.Windows.Forms.Label lblQuery;
        private System.Windows.Forms.GroupBox groupParams;
        private System.Windows.Forms.Panel panelParams;
        private System.Windows.Forms.TableLayoutPanel paramTable;
        private System.Windows.Forms.Button btnSend;
    }
}