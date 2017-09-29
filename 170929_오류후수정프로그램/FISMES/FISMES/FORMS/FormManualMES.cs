using DionesTool.Tcp;
using FISMES.DATA;
using FISMES.GLOBAL;
using FISMES.SOCKET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FISMES.FORMS
{
    
    public partial class FormManualMES : Form
    {
        // DELEGATE
        public delegate void DeleConnected(bool bConnected, string msg = "");
        public delegate void DeleDisconnected(string msg = "");
        public delegate void DeleReceived(Socket sw, string msg);
        public delegate void DeleSent(Socket sw, int sentLen, string msg);

        // SOCKET
        TcpServerAsync socketMES = null;

        public FormManualMES()
        {
            InitializeComponent();

            socketMES = GlobalVariable.SOCKET_MES;
        }

        private void FormManualMES_Load(object sender, EventArgs e)
        {
            MessageBox.Show("테스트1");
            listViewMES.Items.Clear();

            comboBoxProcessResultM3.SelectedIndex = 0;
            comboBoxAlarmMA.SelectedIndex = 0;
        }

        private void FormManualMES_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Visible)
            {
                if (socketMES != null)
                {
                    socketMES.deleConnected -= socketMES_deleConnected;
                    socketMES.deleDisconnected -= socketMES_deleDisconnected;
                    socketMES.deleSent -= socketMES_deleSent;
                    socketMES.deleReceived -= socketMES_deleReceived;
                }
            }
        }

        private void FormManualMES_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void FormManualMES_VisibleChanged(object sender, EventArgs e)
        {
            Console.WriteLine(this.Visible.ToString());

            if (this.Visible)
            {
                if (socketMES != null)
                {
                    socketMES.deleConnected += socketMES_deleConnected;
                    socketMES.deleDisconnected += socketMES_deleDisconnected;
                    socketMES.deleSent += socketMES_deleSent;
                    socketMES.deleReceived += socketMES_deleReceived;
                }
            }
            else
            {
                if (socketMES != null)
                {
                    socketMES.deleConnected -= socketMES_deleConnected;
                    socketMES.deleDisconnected -= socketMES_deleDisconnected;
                    socketMES.deleSent -= socketMES_deleSent;
                    socketMES.deleReceived -= socketMES_deleReceived;
                }
            }
        }

        public void AddListView(bool flagSend, string msg)
        {
            AddListView(flagSend ? "->" : "<-", msg);
        }

        public void AddListView(string sendMsg, string msg)
        {
            ListViewItem item = new ListViewItem(sendMsg);
            item.SubItems.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            item.SubItems.Add(msg);

            listViewMES.Items.Add(item);

            if (listViewMES.Items.Count > 100)
                listViewMES.Items.RemoveAt(0);
        }

        #region ##### SOCKET MES EVENT #####

        void socketMES_deleReceived(Socket sw, string msg)
        {
            if (this.InvokeRequired)
                this.Invoke(new DeleReceived(ReceivedMES), sw, msg);
            else
                ReceivedMES(sw, msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="msg"></param>
        void ReceivedMES(Socket sw, string msg)
        {
            AddListView(false, msg);
        }

        void socketMES_deleSent(Socket sw, int sentLen, string msg)
        {
            if (this.InvokeRequired)
                this.Invoke(new DeleSent(SentMES), sw, sentLen, msg);
            else
                SentMES(sw, sentLen, msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="sentLen"></param>
        /// <param name="msg"></param>
        void SentMES(Socket sw, int sentLen, string msg)
        {
            AddListView(true, msg);
        }

        void socketMES_deleDisconnected(string msg = "")
        {
            if (this.InvokeRequired)
                this.Invoke(new DeleDisconnected(DisConnectedMES), msg);
            else
                DisConnectedMES(msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        void DisConnectedMES(string msg)
        {
            //frmAuto.Disconnected(COMM_TARGET.MES);
            AddListView("C", string.Format("Disconnected. {0}", msg));
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
        /// <param name="bConnected"></param>
        /// <param name="msg"></param>
        void ConnectedMES(bool bConnected, string msg = "")
        {
            if (bConnected)
            {
                //frmAuto.Connected(COMM_TARGET.MES);
                AddListView("C", "Connection succeeded");
            }
            else
            {
                //frmAuto.Disconnected(COMM_TARGET.MES);
                AddListView("C", string.Format("Connection failed. {0}", msg));
            }
        }
        #endregion ##### SOCKET MES EVENT #####

        string r = "";
        private void buttonSendM1_Click(object sender, EventArgs e)
        {
            DevBarcodeDm260 _get = GlobalVariable.DEV_BARCODE;
            MessageBox.Show("buttonSendM1_Click");
            r="";
            MesMsgM1 msg = new MesMsgM1();

            msg.SetSettings(SettingSocketMES.Instance.MSG_ID,
                            SettingSocketMES.Instance.FACTORY_ID,
                            SettingSocketMES.Instance.LINE_ID,
                            SettingSocketMES.Instance.OPER_ID,
                            SettingSocketMES.Instance.EQP_POS_ID);
            MessageBox.Show("_get.Getbarcode()");
            _get.TriggerOn();
            MessageBox.Show("_get.disc();");
            _get.TriggerOff();
             
            if (_get.result=="")
            {
                _get.bGetxml=false;
                MessageBox.Show("바코드값 읽지 못함");
            }
            else
            {
                _get.bGetxml=true;
                msg.SetValue(_get.result);
            }
            


            //msg.SetValue(textBoxM1_RFID.Text);
                msg.Packing();

            

            if (socketMES == null)
            {
                MessageBox.Show("Socket is not created.");
                return;
            }

            Socket clientSock = socketMES.GetClientSocket(0);

            if(clientSock == null)
            {
                MessageBox.Show("MES is not connected.");
                return;
            }
            if (_get.bGetxml)
            {
                socketMES.Send(clientSock, msg.PACKET_STRING);
            }
        }

        private void buttonSendM3_Click(object sender, EventArgs e)
        {
            MesMsgM3 msg = new MesMsgM3();

            msg.SetSettings(SettingSocketMES.Instance.MSG_ID,
                            SettingSocketMES.Instance.FACTORY_ID,
                            SettingSocketMES.Instance.LINE_ID,
                            SettingSocketMES.Instance.OPER_ID,
                            SettingSocketMES.Instance.EQP_POS_ID);

            msg.SetValue(textBoxBarcodeNo.Text, comboBoxProcessResultM3.SelectedIndex == 0 ? true : false);
            msg.Packing();

            if (socketMES == null)
            {
                MessageBox.Show("Socket is not created.");
                return;
            }

            Socket clientSock = socketMES.GetClientSocket(0);

            if (clientSock == null)
            {
                MessageBox.Show("MES is not connected.");
                return;
            }

            socketMES.Send(clientSock, msg.PACKET_STRING);
        }

        private void buttonSendMA_Click(object sender, EventArgs e)
        {
            MesMsgAlarm msg = new MesMsgAlarm();

            msg.SetSettings(SettingSocketMES.Instance.MSG_ID,
                            SettingSocketMES.Instance.FACTORY_ID,
                            SettingSocketMES.Instance.LINE_ID,
                            SettingSocketMES.Instance.OPER_ID,
                            SettingSocketMES.Instance.EQP_POS_ID);

            msg.SetValue(comboBoxAlarmMA.SelectedIndex == 0 ? true : false, textBoxAlarmCodeMA.Text);
            msg.Packing();

            if (socketMES == null)
            {
                MessageBox.Show("Socket is not created.");
                return;
            }

            Socket clientSock = socketMES.GetClientSocket(0);

            if (clientSock == null)
            {
                MessageBox.Show("MES is not connected.");
                return;
            }

            socketMES.Send(clientSock, msg.PACKET_STRING);
        }

        
    }
}
