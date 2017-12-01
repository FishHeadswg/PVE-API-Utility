namespace PVEAPIUtility
{
    partial class PVEAPIForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PVEAPIForm));
            this.btnLogIn = new System.Windows.Forms.Button();
            this.btnSendQuery = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPW = new System.Windows.Forms.TextBox();
            this.btnCreateQuery = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.numEntID = new System.Windows.Forms.NumericUpDown();
            this.txtResponse = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip_Output = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOpenDoc = new System.Windows.Forms.Button();
            this.btnSaveResults = new System.Windows.Forms.Button();
            this.numProjID = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numDocID = new System.Windows.Forms.NumericUpDown();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnCustom = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioPVWA = new System.Windows.Forms.RadioButton();
            this.radioBBV = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkSave = new System.Windows.Forms.CheckBox();
            this.txtSessionID = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnCopy = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.aPIGuideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.servicesGuideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.numEntID)).BeginInit();
            this.contextMenuStrip_Output.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numProjID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDocID)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLogIn
            // 
            this.btnLogIn.Location = new System.Drawing.Point(9, 73);
            this.btnLogIn.Name = "btnLogIn";
            this.btnLogIn.Size = new System.Drawing.Size(80, 23);
            this.btnLogIn.TabIndex = 6;
            this.btnLogIn.Text = "Login";
            this.btnLogIn.UseVisualStyleBackColor = true;
            this.btnLogIn.Click += new System.EventHandler(this.BtnLogIn_Click);
            // 
            // btnSendQuery
            // 
            this.btnSendQuery.Enabled = false;
            this.btnSendQuery.Location = new System.Drawing.Point(130, 19);
            this.btnSendQuery.Name = "btnSendQuery";
            this.btnSendQuery.Size = new System.Drawing.Size(108, 23);
            this.btnSendQuery.TabIndex = 9;
            this.btnSendQuery.Text = "Send Search Query";
            this.btnSendQuery.Click += new System.EventHandler(this.BtnSendQuery_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "User Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(244, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Password:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(361, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Entity ID:";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(78, 47);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(160, 20);
            this.txtUsername.TabIndex = 3;
            this.txtUsername.Text = "admin";
            // 
            // txtPW
            // 
            this.txtPW.Location = new System.Drawing.Point(306, 47);
            this.txtPW.Name = "txtPW";
            this.txtPW.PasswordChar = '*';
            this.txtPW.Size = new System.Drawing.Size(167, 20);
            this.txtPW.TabIndex = 4;
            // 
            // btnCreateQuery
            // 
            this.btnCreateQuery.Enabled = false;
            this.btnCreateQuery.Location = new System.Drawing.Point(10, 19);
            this.btnCreateQuery.Name = "btnCreateQuery";
            this.btnCreateQuery.Size = new System.Drawing.Size(114, 23);
            this.btnCreateQuery.TabIndex = 8;
            this.btnCreateQuery.Text = "Create Search Query";
            this.btnCreateQuery.UseVisualStyleBackColor = true;
            this.btnCreateQuery.Click += new System.EventHandler(this.BtnCreateQuery_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Server URL:";
            // 
            // txtURL
            // 
            this.txtURL.Location = new System.Drawing.Point(78, 21);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(277, 20);
            this.txtURL.TabIndex = 1;
            this.txtURL.Text = "http://localhost";
            // 
            // numEntID
            // 
            this.numEntID.Location = new System.Drawing.Point(423, 21);
            this.numEntID.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numEntID.Name = "numEntID";
            this.numEntID.Size = new System.Drawing.Size(50, 20);
            this.numEntID.TabIndex = 2;
            this.numEntID.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // txtResponse
            // 
            this.txtResponse.BackColor = System.Drawing.SystemColors.Window;
            this.txtResponse.ContextMenuStrip = this.contextMenuStrip_Output;
            this.txtResponse.Location = new System.Drawing.Point(16, 225);
            this.txtResponse.Name = "txtResponse";
            this.txtResponse.ReadOnly = true;
            this.txtResponse.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtResponse.Size = new System.Drawing.Size(956, 287);
            this.txtResponse.TabIndex = 15;
            this.txtResponse.Text = "";
            // 
            // contextMenuStrip_Output
            // 
            this.contextMenuStrip_Output.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem});
            this.contextMenuStrip_Output.Name = "contextMenuStrip1";
            this.contextMenuStrip_Output.Size = new System.Drawing.Size(102, 26);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            // 
            // btnOpenDoc
            // 
            this.btnOpenDoc.Enabled = false;
            this.btnOpenDoc.Location = new System.Drawing.Point(211, 19);
            this.btnOpenDoc.Name = "btnOpenDoc";
            this.btnOpenDoc.Size = new System.Drawing.Size(40, 23);
            this.btnOpenDoc.TabIndex = 12;
            this.btnOpenDoc.Text = "View";
            this.btnOpenDoc.UseVisualStyleBackColor = true;
            this.btnOpenDoc.Click += new System.EventHandler(this.BtnOpenDoc_Click);
            // 
            // btnSaveResults
            // 
            this.btnSaveResults.Location = new System.Drawing.Point(887, 190);
            this.btnSaveResults.Name = "btnSaveResults";
            this.btnSaveResults.Size = new System.Drawing.Size(79, 23);
            this.btnSaveResults.TabIndex = 14;
            this.btnSaveResults.Text = "Save Results";
            this.btnSaveResults.UseVisualStyleBackColor = true;
            this.btnSaveResults.Click += new System.EventHandler(this.BtnSaveResults_Click);
            // 
            // numProjID
            // 
            this.numProjID.Enabled = false;
            this.numProjID.Location = new System.Drawing.Point(69, 21);
            this.numProjID.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numProjID.Name = "numProjID";
            this.numProjID.Size = new System.Drawing.Size(40, 20);
            this.numProjID.TabIndex = 10;
            this.numProjID.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "Project ID:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(115, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Doc ID:";
            // 
            // numDocID
            // 
            this.numDocID.Enabled = false;
            this.numDocID.Location = new System.Drawing.Point(165, 21);
            this.numDocID.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numDocID.Name = "numDocID";
            this.numDocID.Size = new System.Drawing.Size(40, 20);
            this.numDocID.TabIndex = 11;
            this.numDocID.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnUpload
            // 
            this.btnUpload.Enabled = false;
            this.btnUpload.Location = new System.Drawing.Point(693, 190);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(101, 23);
            this.btnUpload.TabIndex = 13;
            this.btnUpload.Text = "Upload Document";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.BtnUpload_Click);
            // 
            // btnCustom
            // 
            this.btnCustom.Enabled = false;
            this.btnCustom.Location = new System.Drawing.Point(800, 190);
            this.btnCustom.Name = "btnCustom";
            this.btnCustom.Size = new System.Drawing.Size(81, 23);
            this.btnCustom.TabIndex = 7;
            this.btnCustom.Text = "Custom Query";
            this.btnCustom.UseVisualStyleBackColor = true;
            this.btnCustom.Click += new System.EventHandler(this.BtnCustom_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCreateQuery);
            this.groupBox1.Controls.Add(this.btnSendQuery);
            this.groupBox1.Location = new System.Drawing.Point(16, 171);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(248, 48);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Document Search";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioPVWA);
            this.groupBox2.Controls.Add(this.radioBBV);
            this.groupBox2.Controls.Add(this.btnOpenDoc);
            this.groupBox2.Controls.Add(this.numProjID);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.numDocID);
            this.groupBox2.Location = new System.Drawing.Point(271, 171);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(416, 48);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "View Document";
            // 
            // radioPVWA
            // 
            this.radioPVWA.AutoSize = true;
            this.radioPVWA.Location = new System.Drawing.Point(331, 22);
            this.radioPVWA.Name = "radioPVWA";
            this.radioPVWA.Size = new System.Drawing.Size(79, 17);
            this.radioPVWA.TabIndex = 25;
            this.radioPVWA.Text = "Use PVWA";
            this.radioPVWA.UseVisualStyleBackColor = true;
            // 
            // radioBBV
            // 
            this.radioBBV.AutoSize = true;
            this.radioBBV.Checked = true;
            this.radioBBV.Location = new System.Drawing.Point(257, 22);
            this.radioBBV.Name = "radioBBV";
            this.radioBBV.Size = new System.Drawing.Size(68, 17);
            this.radioBBV.TabIndex = 24;
            this.radioBBV.TabStop = true;
            this.radioBBV.Text = "Use BBV";
            this.radioBBV.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkSave);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.txtUsername);
            this.groupBox3.Controls.Add(this.txtPW);
            this.groupBox3.Controls.Add(this.txtURL);
            this.groupBox3.Controls.Add(this.btnLogIn);
            this.groupBox3.Controls.Add(this.numEntID);
            this.groupBox3.Location = new System.Drawing.Point(16, 41);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(479, 105);
            this.groupBox3.TabIndex = 29;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Authentication";
            // 
            // chkSave
            // 
            this.chkSave.AutoSize = true;
            this.chkSave.Location = new System.Drawing.Point(365, 77);
            this.chkSave.Name = "chkSave";
            this.chkSave.Size = new System.Drawing.Size(108, 17);
            this.chkSave.TabIndex = 11;
            this.chkSave.Text = "Save Connection";
            this.chkSave.UseVisualStyleBackColor = true;
            this.chkSave.CheckedChanged += new System.EventHandler(this.ChkSave_CheckedChanged);
            // 
            // txtSessionID
            // 
            this.txtSessionID.Enabled = false;
            this.txtSessionID.Location = new System.Drawing.Point(59, 22);
            this.txtSessionID.Name = "txtSessionID";
            this.txtSessionID.Size = new System.Drawing.Size(406, 20);
            this.txtSessionID.TabIndex = 30;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnCopy);
            this.groupBox4.Controls.Add(this.txtSessionID);
            this.groupBox4.Location = new System.Drawing.Point(501, 41);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(471, 54);
            this.groupBox4.TabIndex = 31;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Session ID";
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(9, 20);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(44, 23);
            this.btnCopy.TabIndex = 5;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.BtnCopy_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.MenuBar;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(984, 24);
            this.menuStrip1.TabIndex = 16;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.saveToolStripMenuItem.Text = "&Save Login";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(168, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aPIGuideToolStripMenuItem,
            this.servicesGuideToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(178, 22);
            this.toolStripMenuItem1.Text = "API Documentation";
            // 
            // aPIGuideToolStripMenuItem
            // 
            this.aPIGuideToolStripMenuItem.Name = "aPIGuideToolStripMenuItem";
            this.aPIGuideToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.aPIGuideToolStripMenuItem.Text = "API Guide";
            this.aPIGuideToolStripMenuItem.Click += new System.EventHandler(this.APIGuideToolStripMenuItem_Click);
            // 
            // servicesGuideToolStripMenuItem
            // 
            this.servicesGuideToolStripMenuItem.Name = "servicesGuideToolStripMenuItem";
            this.servicesGuideToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.servicesGuideToolStripMenuItem.Text = "Services Guide";
            this.servicesGuideToolStripMenuItem.Click += new System.EventHandler(this.ServicesGuideToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // PVEAPIForm
            // 
            this.AcceptButton = this.btnLogIn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(984, 524);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCustom);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.btnSaveResults);
            this.Controls.Add(this.txtResponse);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "PVEAPIForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PaperVision Enterprise / ImageSilo API Utility";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.PVEAPIForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numEntID)).EndInit();
            this.contextMenuStrip_Output.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numProjID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDocID)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion

        private System.Windows.Forms.Button btnLogIn;
        private System.Windows.Forms.Button btnSendQuery;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPW;
        private System.Windows.Forms.Button btnCreateQuery;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numEntID;
        public System.Windows.Forms.RichTextBox txtResponse;
        private System.Windows.Forms.Button btnOpenDoc;
        private System.Windows.Forms.Button btnSaveResults;
        private System.Windows.Forms.NumericUpDown numProjID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numDocID;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnCustom;
        public System.Windows.Forms.TextBox txtURL;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtSessionID;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkSave;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem aPIGuideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem servicesGuideToolStripMenuItem;
        private System.Windows.Forms.RadioButton radioPVWA;
        private System.Windows.Forms.RadioButton radioBBV;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Output;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
    }
}

