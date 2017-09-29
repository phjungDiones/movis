namespace CommTestFIS
{
    partial class FormMain
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.listView1 = new System.Windows.Forms.ListView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBoxPreviousResult = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSendF2 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxProcessResult = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.comboBoxAnswerProcessResult = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonSendF4 = new System.Windows.Forms.Button();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxAlarmStatus = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxAlarmCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxCoatingAmount = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.GridLines = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView1.Location = new System.Drawing.Point(12, 42);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(994, 320);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBoxPreviousResult);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.buttonSendF2);
            this.groupBox2.Location = new System.Drawing.Point(12, 368);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(244, 90);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "F2";
            // 
            // comboBoxPreviousResult
            // 
            this.comboBoxPreviousResult.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPreviousResult.FormattingEnabled = true;
            this.comboBoxPreviousResult.Items.AddRange(new object[] {
            "OK",
            "NG"});
            this.comboBoxPreviousResult.Location = new System.Drawing.Point(146, 14);
            this.comboBoxPreviousResult.Name = "comboBoxPreviousResult";
            this.comboBoxPreviousResult.Size = new System.Drawing.Size(91, 20);
            this.comboBoxPreviousResult.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Answer Previous result";
            // 
            // buttonSendF2
            // 
            this.buttonSendF2.Location = new System.Drawing.Point(6, 40);
            this.buttonSendF2.Name = "buttonSendF2";
            this.buttonSendF2.Size = new System.Drawing.Size(232, 42);
            this.buttonSendF2.TabIndex = 0;
            this.buttonSendF2.Text = "Send F2";
            this.buttonSendF2.UseVisualStyleBackColor = true;
            this.buttonSendF2.Click += new System.EventHandler(this.buttonSendF2_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBoxCoatingAmount);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.textBoxProcessResult);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(262, 368);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(244, 90);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "F3";
            // 
            // textBoxProcessResult
            // 
            this.textBoxProcessResult.Location = new System.Drawing.Point(99, 14);
            this.textBoxProcessResult.Name = "textBoxProcessResult";
            this.textBoxProcessResult.ReadOnly = true;
            this.textBoxProcessResult.Size = new System.Drawing.Size(139, 21);
            this.textBoxProcessResult.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "Process result";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.comboBoxAnswerProcessResult);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.buttonSendF4);
            this.groupBox4.Location = new System.Drawing.Point(512, 368);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(244, 90);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "F4";
            // 
            // comboBoxAnswerProcessResult
            // 
            this.comboBoxAnswerProcessResult.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAnswerProcessResult.FormattingEnabled = true;
            this.comboBoxAnswerProcessResult.Items.AddRange(new object[] {
            "OK",
            "NG"});
            this.comboBoxAnswerProcessResult.Location = new System.Drawing.Point(145, 14);
            this.comboBoxAnswerProcessResult.Name = "comboBoxAnswerProcessResult";
            this.comboBoxAnswerProcessResult.Size = new System.Drawing.Size(95, 20);
            this.comboBoxAnswerProcessResult.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Answer process result";
            // 
            // buttonSendF4
            // 
            this.buttonSendF4.Location = new System.Drawing.Point(6, 40);
            this.buttonSendF4.Name = "buttonSendF4";
            this.buttonSendF4.Size = new System.Drawing.Size(232, 42);
            this.buttonSendF4.TabIndex = 2;
            this.buttonSendF4.Text = "Send F4";
            this.buttonSendF4.UseVisualStyleBackColor = true;
            this.buttonSendF4.Click += new System.EventHandler(this.buttonSendF4_Click);
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(43, 12);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.ReadOnly = true;
            this.textBoxPort.Size = new System.Drawing.Size(105, 21);
            this.textBoxPort.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "Port";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxAlarmStatus);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textBoxAlarmCode);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(762, 368);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(244, 90);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "FA";
            // 
            // textBoxAlarmStatus
            // 
            this.textBoxAlarmStatus.Location = new System.Drawing.Point(89, 14);
            this.textBoxAlarmStatus.Name = "textBoxAlarmStatus";
            this.textBoxAlarmStatus.ReadOnly = true;
            this.textBoxAlarmStatus.Size = new System.Drawing.Size(139, 21);
            this.textBoxAlarmStatus.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = "Alarm Status";
            // 
            // textBoxAlarmCode
            // 
            this.textBoxAlarmCode.Location = new System.Drawing.Point(89, 41);
            this.textBoxAlarmCode.Name = "textBoxAlarmCode";
            this.textBoxAlarmCode.ReadOnly = true;
            this.textBoxAlarmCode.Size = new System.Drawing.Size(139, 21);
            this.textBoxAlarmCode.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "Alarm Code";
            // 
            // textBoxCoatingAmount
            // 
            this.textBoxCoatingAmount.Location = new System.Drawing.Point(99, 41);
            this.textBoxCoatingAmount.Name = "textBoxCoatingAmount";
            this.textBoxCoatingAmount.ReadOnly = true;
            this.textBoxCoatingAmount.Size = new System.Drawing.Size(139, 21);
            this.textBoxCoatingAmount.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 12);
            this.label7.TabIndex = 4;
            this.label7.Text = "Coating amount";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 466);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxPort);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.listView1);
            this.Name = "FormMain";
            this.Text = "CommTestFIS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button buttonSendF2;
        private System.Windows.Forms.Button buttonSendF4;
        private System.Windows.Forms.ComboBox comboBoxPreviousResult;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxProcessResult;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxAnswerProcessResult;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxAlarmStatus;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxAlarmCode;
        private System.Windows.Forms.TextBox textBoxCoatingAmount;
        private System.Windows.Forms.Label label7;
    }
}

