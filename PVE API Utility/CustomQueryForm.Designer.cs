namespace PVEAPIUtility
{
    partial class CustomQueryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomQueryForm));
            this.txtXMLQuery = new System.Windows.Forms.TextBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnXML = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtXMLQuery
            // 
            this.txtXMLQuery.Location = new System.Drawing.Point(13, 13);
            this.txtXMLQuery.Multiline = true;
            this.txtXMLQuery.Name = "txtXMLQuery";
            this.txtXMLQuery.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtXMLQuery.Size = new System.Drawing.Size(459, 335);
            this.txtXMLQuery.TabIndex = 0;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(205, 383);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(90, 23);
            this.btnSubmit.TabIndex = 1;
            this.btnSubmit.Text = "Submit Query";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.BtnSubmit_Click);
            // 
            // btnXML
            // 
            this.btnXML.Location = new System.Drawing.Point(205, 354);
            this.btnXML.Name = "btnXML";
            this.btnXML.Size = new System.Drawing.Size(90, 23);
            this.btnXML.TabIndex = 3;
            this.btnXML.Text = "Format XML";
            this.btnXML.UseVisualStyleBackColor = true;
            this.btnXML.Click += new System.EventHandler(this.BtnXML_Click);
            // 
            // CustomQueryForm
            // 
            this.AcceptButton = this.btnSubmit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 418);
            this.Controls.Add(this.btnXML);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.txtXMLQuery);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CustomQueryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CustomQueryForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CustomQueryForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtXMLQuery;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnXML;
    }
}