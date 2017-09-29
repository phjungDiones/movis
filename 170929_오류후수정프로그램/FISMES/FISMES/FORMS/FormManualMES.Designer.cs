namespace FISMES.FORMS
{
    partial class FormManualMES
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
            this.listViewMES = new System.Windows.Forms.ListView();
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonSendM1 = new System.Windows.Forms.Button();
            this.textBoxM1_RFID = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxBarcodeNo = new System.Windows.Forms.TextBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxProcessResultM3 = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonSendM3 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.comboBoxAlarmMA = new System.Windows.Forms.ComboBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonSendMA = new System.Windows.Forms.Button();
            this.textBoxAlarmCodeMA = new System.Windows.Forms.TextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewMES
            // 
            this.listViewMES.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.listViewMES.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9});
            this.listViewMES.ForeColor = System.Drawing.Color.White;
            this.listViewMES.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewMES.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.listViewMES.Location = new System.Drawing.Point(12, 12);
            this.listViewMES.Name = "listViewMES";
            this.listViewMES.Size = new System.Drawing.Size(984, 416);
            this.listViewMES.TabIndex = 15;
            this.listViewMES.UseCompatibleStateImageBehavior = false;
            this.listViewMES.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "";
            this.columnHeader7.Width = 30;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Date";
            this.columnHeader8.Width = 120;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Contents";
            this.columnHeader9.Width = 812;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonSendM1);
            this.groupBox1.Controls.Add(this.textBoxM1_RFID);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Location = new System.Drawing.Point(12, 434);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(266, 92);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " M1 ";
            // 
            // buttonSendM1
            // 
            this.buttonSendM1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonSendM1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonSendM1.ForeColor = System.Drawing.Color.White;
            this.buttonSendM1.Location = new System.Drawing.Point(6, 50);
            this.buttonSendM1.Name = "buttonSendM1";
            this.buttonSendM1.Size = new System.Drawing.Size(254, 30);
            this.buttonSendM1.TabIndex = 18;
            this.buttonSendM1.Text = "Send M1";
            this.buttonSendM1.UseVisualStyleBackColor = false;
            this.buttonSendM1.Click += new System.EventHandler(this.buttonSendM1_Click);
            // 
            // textBoxM1_RFID
            // 
            this.textBoxM1_RFID.Location = new System.Drawing.Point(103, 23);
            this.textBoxM1_RFID.Name = "textBoxM1_RFID";
            this.textBoxM1_RFID.Size = new System.Drawing.Size(157, 21);
            this.textBoxM1_RFID.TabIndex = 17;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel1.Controls.Add(this.label1);
            this.panel1.ForeColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(6, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(97, 21);
            this.panel1.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Barcode No";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxBarcodeNo);
            this.groupBox2.Controls.Add(this.panel6);
            this.groupBox2.Controls.Add(this.comboBoxProcessResultM3);
            this.groupBox2.Controls.Add(this.panel3);
            this.groupBox2.Controls.Add(this.buttonSendM3);
            this.groupBox2.Location = new System.Drawing.Point(283, 434);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(266, 120);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " M3 ";
            // 
            // textBoxBarcodeNo
            // 
            this.textBoxBarcodeNo.Location = new System.Drawing.Point(103, 24);
            this.textBoxBarcodeNo.Name = "textBoxBarcodeNo";
            this.textBoxBarcodeNo.Size = new System.Drawing.Size(157, 21);
            this.textBoxBarcodeNo.TabIndex = 22;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel6.Controls.Add(this.label6);
            this.panel6.ForeColor = System.Drawing.Color.White;
            this.panel6.Location = new System.Drawing.Point(6, 24);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(97, 21);
            this.panel6.TabIndex = 21;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 21);
            this.label6.TabIndex = 0;
            this.label6.Text = "Barcode No";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBoxProcessResultM3
            // 
            this.comboBoxProcessResultM3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxProcessResultM3.FormattingEnabled = true;
            this.comboBoxProcessResultM3.Items.AddRange(new object[] {
            "OK",
            "NG"});
            this.comboBoxProcessResultM3.Location = new System.Drawing.Point(103, 49);
            this.comboBoxProcessResultM3.Name = "comboBoxProcessResultM3";
            this.comboBoxProcessResultM3.Size = new System.Drawing.Size(157, 20);
            this.comboBoxProcessResultM3.TabIndex = 20;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel3.Controls.Add(this.label3);
            this.panel3.ForeColor = System.Drawing.Color.White;
            this.panel3.Location = new System.Drawing.Point(6, 49);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(97, 21);
            this.panel3.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 21);
            this.label3.TabIndex = 0;
            this.label3.Text = "Process Result";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonSendM3
            // 
            this.buttonSendM3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonSendM3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonSendM3.ForeColor = System.Drawing.Color.White;
            this.buttonSendM3.Location = new System.Drawing.Point(6, 76);
            this.buttonSendM3.Name = "buttonSendM3";
            this.buttonSendM3.Size = new System.Drawing.Size(254, 30);
            this.buttonSendM3.TabIndex = 18;
            this.buttonSendM3.Text = "Send M3";
            this.buttonSendM3.UseVisualStyleBackColor = false;
            this.buttonSendM3.Click += new System.EventHandler(this.buttonSendM3_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboBoxAlarmMA);
            this.groupBox3.Controls.Add(this.panel4);
            this.groupBox3.Controls.Add(this.buttonSendMA);
            this.groupBox3.Controls.Add(this.textBoxAlarmCodeMA);
            this.groupBox3.Controls.Add(this.panel5);
            this.groupBox3.Location = new System.Drawing.Point(556, 434);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(266, 120);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = " MA ";
            // 
            // comboBoxAlarmMA
            // 
            this.comboBoxAlarmMA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAlarmMA.FormattingEnabled = true;
            this.comboBoxAlarmMA.Items.AddRange(new object[] {
            "Start",
            "End"});
            this.comboBoxAlarmMA.Location = new System.Drawing.Point(103, 23);
            this.comboBoxAlarmMA.Name = "comboBoxAlarmMA";
            this.comboBoxAlarmMA.Size = new System.Drawing.Size(157, 20);
            this.comboBoxAlarmMA.TabIndex = 20;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel4.Controls.Add(this.label4);
            this.panel4.ForeColor = System.Drawing.Color.White;
            this.panel4.Location = new System.Drawing.Point(6, 23);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(97, 21);
            this.panel4.TabIndex = 19;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 21);
            this.label4.TabIndex = 0;
            this.label4.Text = "Alarm";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonSendMA
            // 
            this.buttonSendMA.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonSendMA.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonSendMA.ForeColor = System.Drawing.Color.White;
            this.buttonSendMA.Location = new System.Drawing.Point(6, 77);
            this.buttonSendMA.Name = "buttonSendMA";
            this.buttonSendMA.Size = new System.Drawing.Size(254, 30);
            this.buttonSendMA.TabIndex = 18;
            this.buttonSendMA.Text = "Send MA";
            this.buttonSendMA.UseVisualStyleBackColor = false;
            this.buttonSendMA.Click += new System.EventHandler(this.buttonSendMA_Click);
            // 
            // textBoxAlarmCodeMA
            // 
            this.textBoxAlarmCodeMA.Location = new System.Drawing.Point(103, 50);
            this.textBoxAlarmCodeMA.Name = "textBoxAlarmCodeMA";
            this.textBoxAlarmCodeMA.Size = new System.Drawing.Size(157, 21);
            this.textBoxAlarmCodeMA.TabIndex = 17;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel5.Controls.Add(this.label5);
            this.panel5.ForeColor = System.Drawing.Color.White;
            this.panel5.Location = new System.Drawing.Point(6, 50);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(97, 21);
            this.panel5.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 21);
            this.label5.TabIndex = 0;
            this.label5.Text = "Alarm Code";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormManualMES
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1008, 586);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.listViewMES);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormManualMES";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "FormManualMES";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormManualMES_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormManualMES_FormClosed);
            this.Load += new System.EventHandler(this.FormManualMES_Load);
            this.VisibleChanged += new System.EventHandler(this.FormManualMES_VisibleChanged);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewMES;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonSendM1;
        private System.Windows.Forms.TextBox textBoxM1_RFID;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonSendM3;
        private System.Windows.Forms.ComboBox comboBoxProcessResultM3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox comboBoxAlarmMA;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonSendMA;
        private System.Windows.Forms.TextBox textBoxAlarmCodeMA;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxBarcodeNo;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label6;

    }
}