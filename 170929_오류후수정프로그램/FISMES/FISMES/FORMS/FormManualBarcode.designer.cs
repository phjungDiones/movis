namespace FISMES.FORMS
{
    partial class FormManualBarcode
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "->",
            "2016-04-04 12:23:57",
            "asdf"}, -1);
            this.listViewBarcode = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonSendTrg = new System.Windows.Forms.Button();
            this.textBoxDateF1 = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewBarcode
            // 
            this.listViewBarcode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.listViewBarcode.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listViewBarcode.ForeColor = System.Drawing.Color.White;
            this.listViewBarcode.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewBarcode.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.listViewBarcode.Location = new System.Drawing.Point(12, 12);
            this.listViewBarcode.Name = "listViewBarcode";
            this.listViewBarcode.Size = new System.Drawing.Size(984, 302);
            this.listViewBarcode.TabIndex = 15;
            this.listViewBarcode.UseCompatibleStateImageBehavior = false;
            this.listViewBarcode.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 30;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Date";
            this.columnHeader2.Width = 120;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Contents";
            this.columnHeader3.Width = 130;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonSendTrg);
            this.groupBox1.Controls.Add(this.textBoxDateF1);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Location = new System.Drawing.Point(12, 325);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(241, 90);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Trigger";
            // 
            // buttonSendTrg
            // 
            this.buttonSendTrg.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonSendTrg.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonSendTrg.ForeColor = System.Drawing.Color.White;
            this.buttonSendTrg.Location = new System.Drawing.Point(6, 48);
            this.buttonSendTrg.Name = "buttonSendTrg";
            this.buttonSendTrg.Size = new System.Drawing.Size(227, 30);
            this.buttonSendTrg.TabIndex = 29;
            this.buttonSendTrg.Text = "Send Trigger On -> Off";
            this.buttonSendTrg.UseVisualStyleBackColor = false;
            this.buttonSendTrg.Click += new System.EventHandler(this.buttonSendTrg_Click);
            // 
            // textBoxDateF1
            // 
            this.textBoxDateF1.Location = new System.Drawing.Point(83, 21);
            this.textBoxDateF1.Name = "textBoxDateF1";
            this.textBoxDateF1.ReadOnly = true;
            this.textBoxDateF1.Size = new System.Drawing.Size(150, 39);
            this.textBoxDateF1.TabIndex = 19;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel2.Controls.Add(this.label2);
            this.panel2.ForeColor = System.Drawing.Color.White;
            this.panel2.Location = new System.Drawing.Point(6, 21);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(77, 21);
            this.panel2.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 21);
            this.label2.TabIndex = 0;
            this.label2.Text = "Barcode";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormManualBarcode
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1008, 586);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.listViewBarcode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormManualBarcode";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "a";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormManualBarcode_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormManualBarcode_FormClosed);
            this.Load += new System.EventHandler(this.FormManualBarcode_Load);
            this.VisibleChanged += new System.EventHandler(this.FormManualBarcode_VisibleChanged);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewBarcode;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxDateF1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonSendTrg;
    }
}