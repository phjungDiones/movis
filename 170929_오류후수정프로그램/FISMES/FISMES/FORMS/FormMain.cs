using FISMES.SOCKET;
using FISMES.FORMS;
using FISMES.FORMS.POPUP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FISMES.DATA;
using DionesTool.DEVICE.SERIAL.C0405;
using DionesTool.Tcp;
using FISMES.GLOBAL;
using System.Net.Sockets;

namespace FISMES
{
    public partial class FormMain : Form
    {
        // DELEGATE SOCKET
        public delegate void DeleConnected(bool bConnected, string msg = "");
        public delegate void DeleDisconnected(string msg = "");
        public delegate void DeleReceived(Socket sw, string msg);
        public delegate void DeleSent(Socket sw, int sentLen, string msg);

        // DELEGATE RFID
        public delegate void DeleStartContiReadData(byte[] data, byte[] tagID);
        public delegate void DeleError(byte command, byte errorCode);
        public delegate void DeleGetConfig(C0405Packet packet);
        public delegate void DeleNormal();
        public delegate void DeleStartContiRead(byte[] data);
        public delegate void DeleReadTagID(byte[] data);
        public delegate void DeleReadData(byte[] data);

        // SOCKET
        TcpServerAsync socketMES;
        TcpServerAsync socketFIS;

        DevBarcodeDm260 devBarcode;

        bool m_bPrevConnectedBarcode = false;

        // FORM
        FormAuto frmAuto = new FormAuto();
        FormManualFIS frmManualFIS = new FormManualFIS();
        FormManualMES frmManualMES = new FormManualMES();
        FormManualBarcode frmManualRFID = new FormManualBarcode();
        FormSetting frmSetting = new FormSetting();

        public FormMain()
        {
            InitializeComponent();

            GlobalDefine.STARTUP_PATH = Application.StartupPath;

            socketMES = GlobalVariable.SOCKET_MES;
            socketFIS = GlobalVariable.SOCKET_FIS;
            devBarcode = GlobalVariable.DEV_BARCODE;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            LogManager.Instance.WriteI(LOG_TYPE.EVENT, "=======================================");
            LogManager.Instance.WriteI(LOG_TYPE.EVENT, "PROGRAM START");
            LogManager.Instance.WriteI(LOG_TYPE.EVENT, "=======================================");

            frmAuto.MdiParent = this;
            frmAuto.Parent = this.pictureBoxMain;
            frmAuto.Show();

            frmAuto.OnChangeAutoMode += new FormAuto.DeleChangeAutoMode(frmAuto_OnChangeAutoMode);

            frmManualFIS.MdiParent = this;
            frmManualFIS.Parent = this.pictureBoxMain;
            frmManualFIS.Hide();

            frmManualMES.MdiParent = this;
            frmManualMES.Parent = this.pictureBoxMain;
            frmManualMES.Hide();

            frmManualRFID.MdiParent = this;
            frmManualRFID.Parent = this.pictureBoxMain;
            frmManualRFID.Hide();

            frmSetting.MdiParent = this;
            frmSetting.Parent = this.pictureBoxMain;
            frmSetting.Hide();

            // Default Auto Mode
            frmAuto.SetAutoMode(false);

            // Setup Communication
            SetupCommunication();

            // Setup Device
            SetupDevice();

            // socket & serial 연결 확인 (바로 호출)
            timerConnect.Enabled = true;
            timerConnect_Tick(timerConnect, new EventArgs());

            LogManager.Instance.ToString();
            ExceptionManager.Instance.Init();
        }

        void frmAuto_OnChangeAutoMode(bool bAutoMode)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    OnChangedAutoMode();
                });
            }
            else
            {
                OnChangedAutoMode();
            }
        }

        void OnChangedAutoMode()
        {
            if (MachineStatus.AUTO_MODE)
            {
                buttonAuto.Enabled = false;
                buttonManualFIS.Enabled = false;
                buttonManualMES.Enabled = false;
                buttonManualRFID.Enabled = false;
                buttonSetting.Enabled = false;

                HideAllForm();
                frmAuto.Show();
            }
            else
            {
                buttonAuto.Enabled = true;
                buttonManualFIS.Enabled = true;
                buttonManualMES.Enabled = true;
                buttonManualRFID.Enabled = true;
                buttonSetting.Enabled = true;

                //HideAllForm();
                //frmAuto.Show();
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmAuto.OnChangeAutoMode -= new FormAuto.DeleChangeAutoMode(frmAuto_OnChangeAutoMode);

            timerConnect.Enabled = false;

            socketMES.deleConnected -= socketMES_deleConnected;
            socketMES.deleDisconnected -= socketMES_deleDisconnected;
            socketMES.deleSent -= socketMES_deleSent;
            socketMES.deleReceived -= socketMES_deleReceived;

            socketMES.Stop();

            socketFIS.deleConnected -= socketFIS_deleConnected;
            socketFIS.deleDisconnected -= socketFIS_deleDisconnected;
            socketFIS.deleSent -= socketFIS_deleSent;
            socketFIS.deleReceived -= socketFIS_deleReceived;

            socketFIS.Stop();

            devBarcode.deleException -= new DevBarcodeDm260.DeleException(devBarcode_deleException);
            devBarcode.deleReceived -= new DevBarcodeDm260.DeleReceived(devBarcode_deleReceived);
            devBarcode.deleSent -= new DevBarcodeDm260.DeleSent(devBarcode_deleSent);
            devBarcode.Disconnect();

            ExceptionManager.Instance.Dispose();

            GlobalVariable.autoRun.Dispose();

            LogManager.Instance.WriteI(LOG_TYPE.EVENT, "=======================================");
            LogManager.Instance.WriteI(LOG_TYPE.EVENT, "PROGRAM EXIT");
            LogManager.Instance.WriteI(LOG_TYPE.EVENT, "=======================================");
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        void SetupCommunication()
        {
            // MES
            if (!SettingSocketMES.Instance.Deserialize(PathManager.Instance.PATH_SETTING_SOCKET_MES))
                SettingSocketMES.Instance.Serialize(PathManager.Instance.PATH_SETTING_SOCKET_MES);
            System.Net.IPAddress addrMes = System.Net.IPAddress.Parse(SettingSocketMES.Instance.IPADDR);
            socketMES.SetServerInfo(addrMes, SettingSocketMES.Instance.PORT, SettingSocketMES.Instance.MAX_CLIENT_COUNT);
            socketMES.SetDelimeter(TcpBase.ETX.ToString());

            socketMES.deleConnected += socketMES_deleConnected;
            socketMES.deleDisconnected += socketMES_deleDisconnected;
            socketMES.deleSent += socketMES_deleSent;
            socketMES.deleReceived += socketMES_deleReceived;

            socketMES.Start();
            LogManager.Instance.WriteI(LOG_TYPE.EVENT, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + "\t" + 
                                                       System.Reflection.MethodBase.GetCurrentMethod().Name + "\t" + 
                                                       "MES SERVER START");

            // FIS
            if (!SettingSocketFIS.Instance.Deserialize(PathManager.Instance.PATH_SETTING_SOCKET_FIS))
                SettingSocketFIS.Instance.Serialize(PathManager.Instance.PATH_SETTING_SOCKET_FIS);
            //System.Net.IPAddress addrFis = System.Net.IPAddress.Parse(SettingSocketMES.Instance.IPADDR);
            socketFIS.SetServerInfo(SettingSocketFIS.Instance.PORT, SettingSocketFIS.Instance.MAX_CLIENT_COUNT);
            socketFIS.SetDelimeter(TcpBase.ETX.ToString());

            socketFIS.deleConnected += socketFIS_deleConnected;
            socketFIS.deleDisconnected += socketFIS_deleDisconnected;
            socketFIS.deleSent += socketFIS_deleSent;
            socketFIS.deleReceived += socketFIS_deleReceived;

            socketFIS.Start();
            LogManager.Instance.WriteI(LOG_TYPE.EVENT, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + "\t" +
                                                       System.Reflection.MethodBase.GetCurrentMethod().Name + "\t" +
                                                       "FIS SERVER START");
        }

        void SetupDevice()
        {
            devBarcode.deleException += new DevBarcodeDm260.DeleException(devBarcode_deleException);
            devBarcode.deleReceived += new DevBarcodeDm260.DeleReceived(devBarcode_deleReceived);
            devBarcode.deleSent += new DevBarcodeDm260.DeleSent(devBarcode_deleSent);

            devBarcode.Connect();
        }

        void devBarcode_deleSent(string strMsg)
        {
            frmAuto.AddListView(COMM_TARGET.BARCODE, true, strMsg);
        }

        void devBarcode_deleReceived(string strBarcode)
        {
            GlobalVariable.BARCODE = strBarcode;
            GlobalVariable.FLAG_RECV_BARCODE = true;

            frmAuto.AddListView(COMM_TARGET.BARCODE, false, strBarcode);
        }

        void devBarcode_deleException(string strErr)
        {
            //
        }

        void CheckAutoMode()
        {
            if (socketFIS.CLIENT_COUNT > 0 && socketMES.CLIENT_COUNT > 0 && devBarcode.IsConnected)
            {
                frmAuto.SetAutoMode(true);

                LogManager.Instance.WriteI(LOG_TYPE.EVENT, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + "\t" +
                                                       System.Reflection.MethodBase.GetCurrentMethod().Name + "\t" +
                                                       "AUTO MODE");
            }
            else
            {
                frmAuto.SetAutoMode(false);

                LogManager.Instance.WriteI(LOG_TYPE.EVENT, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + "\t" +
                                                       System.Reflection.MethodBase.GetCurrentMethod().Name + "\t" +
                                                       "MANUAL MODE");
            }
        }

        #region ##### [SOCKET MES] #####
        void socketMES_deleReceived(Socket sw, string msg)
        {
            if (this.InvokeRequired)
                this.Invoke(new DeleReceived(ReceivedMES), sw, msg);
            else
                ReceivedMES(sw, msg);
        }

        void socketMES_deleSent(Socket sw, int sentLen, string msg)
        {
            if (this.InvokeRequired)
                this.Invoke(new DeleSent(SentMES), sw, sentLen, msg);
            else
                SentMES(sw, sentLen, msg);
        }

        void socketMES_deleDisconnected(string msg = "")
        {
            if (this.InvokeRequired)
                this.Invoke(new DeleDisconnected(DisConnectedMES), msg);
            else
                DisConnectedMES(msg);
        }

        void socketMES_deleConnected(bool bConnected, string msg = "")
        {
            if (this.InvokeRequired)
                this.Invoke(new DeleConnected(ConnectedMES), bConnected, msg);
            else
                ConnectedMES(bConnected, msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="msg"></param>
        void ReceivedMES(Socket sw, string msg)
        {
            LogManager.Instance.WriteI(LOG_TYPE.COMM_MES, "RECEIVED : " + msg);

            frmAuto.AddListView(COMM_TARGET.MES, false, msg);

            // Temp Comment
            if (MachineStatus.AUTO_MODE)
            {
                MesMsg msgMes = new MesMsg();
                if (msgMes.Parsing(msg))
                {
                    GlobalVariable.MES_MSG = msgMes;
                    GlobalVariable.FLAG_RECV_MES = true;

                    frmAuto.RefreshInfoMES(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), msgMes.BARCODE_NO);
                    //if (msgMes.MSG_ID == "M2")
                    //{
                    //    GlobalVariable.BARCODE_NO = msgMes.BARCODE_NO;

                    //    FisMsg fmsg = new FisMsg();
                    //    fmsg.SetValue("F2", msgMes.MES_RESULT, "");
                    //    fmsg.Packing();
                    //    GlobalVariable.SOCKET_FIS.GetClientSocket(0).Send(fmsg.PACKET);
                    //}
                    //else if (msgMes.MSG_ID == "M4")
                    //{
                    //    FisMsg fmsg = new FisMsg();
                    //    fmsg.SetValue("F4", msgMes.MES_RESULT, "");
                    //    fmsg.Packing();
                    //    GlobalVariable.SOCKET_FIS.GetClientSocket(0).Send(fmsg.PACKET);
                    //}
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="sentLen"></param>
        /// <param name="msg"></param>
        void SentMES(Socket sw, int sentLen, string msg)
        {
            LogManager.Instance.WriteI(LOG_TYPE.COMM_MES, "SENT : " + msg);

            frmAuto.AddListView(COMM_TARGET.MES, true, msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        void DisConnectedMES(string msg)
        {
            LogManager.Instance.WriteI(LOG_TYPE.COMM_MES, "DISCONNECTED : " + msg);
            LogManager.Instance.WriteI(LOG_TYPE.COMM_MES, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + "\t" +
                                                              System.Reflection.MethodBase.GetCurrentMethod().Name + "\t" +
                                                              "MES CLIENT CONNECTED");

            frmAuto.Disconnected(COMM_TARGET.MES);
            frmAuto.AddListView(COMM_TARGET.MES, "C", string.Format("Disconnected. {0}", msg));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bConnected"></param>
        /// <param name="msg"></param>
        void ConnectedMES(bool bConnected, string msg = "")
        {
            if (bConnected)
            {
                frmAuto.Connected(COMM_TARGET.MES);
                frmAuto.AddListView(COMM_TARGET.MES, "C", "Connection succeeded");

                LogManager.Instance.WriteI(LOG_TYPE.COMM_MES, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + "\t" +
                                                              System.Reflection.MethodBase.GetCurrentMethod().Name + "\t" +
                                                              "MES CLIENT CONNECTED");

                CheckAutoMode();
            }
            else
            {
                frmAuto.Disconnected(COMM_TARGET.MES);
                frmAuto.AddListView(COMM_TARGET.MES, "C", string.Format("Connection failed. {0}", msg));

                LogManager.Instance.WriteI(LOG_TYPE.COMM_MES, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + "\t" +
                                                       System.Reflection.MethodBase.GetCurrentMethod().Name + "\t" +
                                                       "MES CLIENT NOT CONNECTED : " + msg);
            }
        }
        #endregion ##### [SOCKET MES] #####


        #region ##### [SOCKET FIS] #####
        void socketFIS_deleReceived(Socket sw, string msg)
        {
            if (this.InvokeRequired)
                this.Invoke(new DeleReceived(ReceivedFIS), sw, msg);
            else
                ReceivedFIS(sw, msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="msg"></param>
        void ReceivedFIS(Socket sw, string msg)
        {
            LogManager.Instance.WriteI(LOG_TYPE.COMM_FIS, "RECEIVED : " + msg);

            frmAuto.AddListView(COMM_TARGET.FIS, false, msg);

            // Temp Comment
            if (MachineStatus.AUTO_MODE)
            {
                FisMsg fmsg = new FisMsg();
                fmsg.Parsing(msg);
                GlobalVariable.FIS_MSG = fmsg;
                GlobalVariable.FLAG_RECV_FIS = true;

                frmAuto.RefreshInfoFIS(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), fmsg.RESULT, fmsg.VALUE);

                //switch (fmsg.COMMAND_ID)
                //{
                //    case "F1":
                //        {
                //            devC0405.ReadData(0, 7);
                //        }
                //        break;
                //    case "F3":
                //        {
                //            // 현재 성공일 때만 메시지를 전달한다
                //            if (fmsg.RESULT == "0")
                //            {
                //                MesMsgM3 msgM3 = new MesMsgM3();
                //                msgM3.SetSettings(SettingSocketMES.Instance.MSG_ID,
                //                        SettingSocketMES.Instance.FACTORY_ID,
                //                        SettingSocketMES.Instance.LINE_ID,
                //                        SettingSocketMES.Instance.OPER_ID,
                //                        SettingSocketMES.Instance.EQP_POS_ID);
                //                msgM3.SetValue(GlobalVariable.RFID, GlobalVariable.BARCODE_NO, true, 3.0);

                //                Socket clientSock = socketMES.GetClientSocket(0);

                //                if (clientSock == null)
                //                {
                //                    MessageBox.Show("MES is not connected.");
                //                    return;
                //                }
                //                socketMES.Send(clientSock, msgM3.PACKET);
                //            }
                //        }
                //        break;
                //    case "FA":
                //        {
                //        }
                //        break;
                //    default:
                //        break;
                //}
            }
        }

        void socketFIS_deleSent(Socket sw, int sentLen, string msg)
        {
            if (this.InvokeRequired)
                this.Invoke(new DeleSent(SentFIS), sw, sentLen, msg);
            else
                SentFIS(sw, sentLen, msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="sentLen"></param>
        /// <param name="msg"></param>
        void SentFIS(Socket sw, int sentLen, string msg)
        {
            LogManager.Instance.WriteI(LOG_TYPE.COMM_FIS, "SENT : " + msg);

            frmAuto.AddListView(COMM_TARGET.FIS, true, msg);
        }

        void socketFIS_deleDisconnected(string msg = "")
        {
            if (this.InvokeRequired)
                this.Invoke(new DeleDisconnected(DisConnectedFIS), msg);
            else
                DisConnectedFIS(msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        void DisConnectedFIS(string msg)
        {
            LogManager.Instance.WriteI(LOG_TYPE.COMM_FIS, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + "\t" +
                                                       System.Reflection.MethodBase.GetCurrentMethod().Name + "\t" +
                                                       "FIS CLIENT DISCONNECTED : " + msg);

            frmAuto.Disconnected(COMM_TARGET.FIS);
            frmAuto.AddListView(COMM_TARGET.FIS, "C", string.Format("Disconnected. {0}", msg));
        }

        void socketFIS_deleConnected(bool bConnected, string msg = "")
        {
            if (this.InvokeRequired)
                this.Invoke(new DeleConnected(ConnectedFIS), bConnected, msg);
            else
                ConnectedFIS(bConnected, msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bConnected"></param>
        /// <param name="msg"></param>
        void ConnectedFIS(bool bConnected, string msg = "")
        {
            if(bConnected)
            {
                frmAuto.Connected(COMM_TARGET.FIS);
                frmAuto.AddListView(COMM_TARGET.FIS, "C", "Connection succeeded");

                LogManager.Instance.WriteI(LOG_TYPE.COMM_FIS, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + "\t" +
                                                       System.Reflection.MethodBase.GetCurrentMethod().Name + "\t" +
                                                       "FIS CLIENT CONNECTED");

                CheckAutoMode();
            }
            else
            {
                frmAuto.Disconnected(COMM_TARGET.FIS);
                frmAuto.AddListView(COMM_TARGET.FIS, "C", string.Format("Connection failed. {0}", msg));

                LogManager.Instance.WriteI(LOG_TYPE.COMM_FIS, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + "\t" +
                                                       System.Reflection.MethodBase.GetCurrentMethod().Name + "\t" +
                                                       "FIS CLIENT NOT CONNECTED : " + msg);

                frmAuto.SetAutoMode(false);
            }
        }
        #endregion ##### [SOCKET FIS] #####


        #region ##### [MENU] #####
        void HideAllForm()
        {
            frmAuto.Hide();
            frmManualFIS.Hide();
            frmManualMES.Hide();
            frmManualRFID.Hide();
            frmSetting.Hide();
        }

        private void buttonAuto_Click(object sender, EventArgs e)
        {
            HideAllForm();

            frmAuto.Show();
        }

        private void buttonManualFIS_Click(object sender, EventArgs e)
        {
            HideAllForm();

            frmManualFIS.Show();
        }

        private void buttonManualMES_Click(object sender, EventArgs e)
        {
            HideAllForm();

            frmManualMES.Show();
        }

        private void buttonManualRFID_Click(object sender, EventArgs e)
        {
            HideAllForm();

            frmManualRFID.Show();
        }

        private void buttonSetting_Click(object sender, EventArgs e)
        {
            HideAllForm();

            frmSetting.Show();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("프로그램을 종료하시겠습니까?", "프로그램 종료", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                Application.Exit();
            }
        } 
        #endregion

        /// <summary>
        /// 소켓 및 시리얼 연결을 5초마다 시도합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerConnect_Tick(object sender, EventArgs e)
        {
            // MES
            // client 수로 체크해야하나? 아니면 connected, disconnected delegate로만 가능한가

            // FIS
            // client 수로 체크해야하나? 아니면 connected, disconnected delegate로만 가능한가

            // RFID
            //if (!devC0405.IsOpen)
            //{
            //    frmAuto.AddListView(COMM_TARGET.RFID, "C", "trying connection...");
            //    if (devC0405.Open())
            //    {
            //        frmAuto.Connected(COMM_TARGET.RFID);
            //        frmAuto.AddListView(COMM_TARGET.RFID, "C", "connection succeeded");

            //        LogManager.Instance.WriteI(LOG_TYPE.EVENT, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + "\t" +
            //                                           System.Reflection.MethodBase.GetCurrentMethod().Name + "\t" +
            //                                           "RFID CONNECTED");

            //        CheckAutoMode();
            //    }
            //    else
            //    {
            //        frmAuto.Disconnected(COMM_TARGET.RFID);
            //        frmAuto.AddListView(COMM_TARGET.RFID, "C", "connection failed");

            //        LogManager.Instance.WriteI(LOG_TYPE.EVENT, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + "\t" +
            //                                           System.Reflection.MethodBase.GetCurrentMethod().Name + "\t" +
            //                                           "RFID NOT CONNECTED");

            //        CheckAutoMode();
            //    }
            //}

            if (m_bPrevConnectedBarcode != devBarcode.IsConnected)
            {
                m_bPrevConnectedBarcode = devBarcode.IsConnected;
                if (devBarcode.IsConnected)
                {
                    frmAuto.Connected(COMM_TARGET.BARCODE);
                    frmAuto.AddListView(COMM_TARGET.BARCODE, "C", "connection succeeded");

                    LogManager.Instance.WriteI(LOG_TYPE.EVENT, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + "\t" +
                                                       System.Reflection.MethodBase.GetCurrentMethod().Name + "\t" +
                                                       "BARCODE CONNECTED");

                    CheckAutoMode();
                }
                else
                {
                    frmAuto.Disconnected(COMM_TARGET.BARCODE);
                    frmAuto.AddListView(COMM_TARGET.BARCODE, "C", "connection failed");

                    LogManager.Instance.WriteI(LOG_TYPE.EVENT, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + "\t" +
                                                       System.Reflection.MethodBase.GetCurrentMethod().Name + "\t" +
                                                       "BARCODE NOT CONNECTED");

                    CheckAutoMode();
                }
            }
        }

        private void timerRemoveLogFile_Tick(object sender, EventArgs e)
        {
            LogManager.Instance.DeleteOldFile();
        }
        
    }
}
