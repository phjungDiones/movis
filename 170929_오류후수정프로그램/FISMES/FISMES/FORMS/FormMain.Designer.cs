namespace FISMES
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
            this.components = new System.ComponentModel.Container();
            this.pictureBoxMain = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonExit = new System.Windows.Forms.Button();
            this.buttonManualRFID = new System.Windows.Forms.Button();
            this.buttonManualMES = new System.Windows.Forms.Button();
            this.buttonAuto = new System.Windows.Forms.Button();
            this.timerConnect = new System.Windows.Forms.Timer(this.components);
            this.buttonSetting = new System.Windows.Forms.Button();
            this.serialPortC0405 = new System.IO.Ports.SerialPort(this.components);
            this.buttonManualFIS = new System.Windows.Forms.Button();
            this.timerRemoveLogFile = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxMain
            // 
            this.pictureBoxMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pictureBoxMain.Location = new System.Drawing.Point(0, 62);
            this.pictureBoxMain.Name = "pictureBoxMain";
            this.pictureBoxMain.Size = new System.Drawing.Size(1008, 586);
            this.pictureBoxMain.TabIndex = 5;
            this.pictureBoxMain.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1008, 62);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(837, 4);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(158, 55);
            this.buttonExit.TabIndex = 14;
            this.buttonExit.Text = "EXIT";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // buttonManualRFID
            // 
            this.buttonManualRFID.Location = new System.Drawing.Point(503, 4);
            this.buttonManualRFID.Name = "buttonManualRFID";
            this.buttonManualRFID.Size = new System.Drawing.Size(158, 55);
            this.buttonManualRFID.TabIndex = 13;
            this.buttonManualRFID.Text = "Manual RFID";
            this.buttonManualRFID.UseVisualStyleBackColor = true;
            this.buttonManualRFID.Click += new System.EventHandler(this.buttonManualRFID_Click);
            // 
            // buttonManualMES
            // 
            this.buttonManualMES.Location = new System.Drawing.Point(336, 4);
            this.buttonManualMES.Name = "buttonManualMES";
            this.buttonManualMES.Size = new System.Drawing.Size(158, 55);
            this.buttonManualMES.TabIndex = 12;
            this.buttonManualMES.Text = "Manual MES";
            this.buttonManualMES.UseVisualStyleBackColor = true;
            this.buttonManualMES.Click += new System.EventHandler(this.buttonManualMES_Click);
            // 
            // buttonAuto
            // 
            this.buttonAuto.Location = new System.Drawing.Point(2, 4);
            this.buttonAuto.Name = "buttonAuto";
            this.buttonAuto.Size = new System.Drawing.Size(158, 55);
            this.buttonAuto.TabIndex = 11;
            this.buttonAuto.Text = "AUTO";
            this.buttonAuto.UseVisualStyleBackColor = true;
            this.buttonAuto.Click += new System.EventHandler(this.buttonAuto_Click);
            // 
            // timerConnect
            // 
            this.timerConnect.Interval = 5000;
            this.timerConnect.Tick += new System.EventHandler(this.timerConnect_Tick);
            // 
            // buttonSetting
            // 
            this.buttonSetting.Location = new System.Drawing.Point(670, 4);
            this.buttonSetting.Name = "buttonSetting";
            this.buttonSetting.Size = new System.Drawing.Size(158, 55);
            this.buttonSetting.TabIndex = 16;
            this.buttonSetting.Text = "SETTING";
            this.buttonSetting.UseVisualStyleBackColor = true;
            this.buttonSetting.Click += new System.EventHandler(this.buttonSetting_Click);
            // 
            // serialPortC0405
            // 
            this.serialPortC0405.PortName = "COM3";
            // 
            // buttonManualFIS
            // 
            this.buttonManualFIS.Location = new System.Drawing.Point(169, 4);
            this.buttonManualFIS.Name = "buttonManualFIS";
            this.buttonManualFIS.Size = new System.Drawing.Size(158, 55);
            this.buttonManualFIS.TabIndex = 18;
            this.buttonManualFIS.Text = "Manual FIS";
            this.buttonManualFIS.UseVisualStyleBackColor = true;
            this.buttonManualFIS.Click += new System.EventHandler(this.buttonManualFIS_Click);
            // 
            // timerRemoveLogFile
            // 
            this.timerRemoveLogFile.Interval = 3600000;
            this.timerRemoveLogFile.Tick += new System.EventHandler(this.timerRemoveLogFile_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1008, 648);
            this.ControlBox = false;
            this.Controls.Add(this.buttonManualFIS);
            this.Controls.Add(this.buttonSetting);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.buttonManualRFID);
            this.Controls.Add(this.buttonManualMES);
            this.Controls.Add(this.buttonAuto);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pictureBoxMain);
            this.IsMdiContainer = true;
            this.Name = "FormMain";
            this.Text = "FISMES";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxMain;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Button buttonManualRFID;
        private System.Windows.Forms.Button buttonManualMES;
        private System.Windows.Forms.Button buttonAuto;
        private System.Windows.Forms.Timer timerConnect;
        private System.Windows.Forms.Button buttonSetting;
        private System.IO.Ports.SerialPort serialPortC0405;
        private System.Windows.Forms.Button buttonManualFIS;
        private System.Windows.Forms.Timer timerRemoveLogFile;
    }
}

