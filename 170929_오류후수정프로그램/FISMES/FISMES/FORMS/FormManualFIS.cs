using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FISMES.GLOBAL;
using FISMES.SOCKET;
using DionesTool.Tcp;
using System.Net.Sockets;

namespace FISMES.FORMS
{
    public partial class FormManualFIS : Form
    {
        // DELEGATE
        public delegate void DeleConnected(bool bConnected, string msg = "");
        public delegate void DeleDisconnected(string msg = "");
        public delegate void DeleReceived(Socket sw, string msg);
        public delegate void DeleSent(Socket sw, int sentLen, string msg);

        TcpServerAsync socketFIS = null;

        public FormManualFIS()
        {
            InitializeComponent();

            socketFIS = GlobalVariable.SOCKET_FIS;
        }

        private void FormManualFIS_Load(object sender, EventArgs e)
        {
            listViewFIS.Items.Clear();

            comboBoxResultF2.SelectedIndex = 0;
            comboBoxResultF4.SelectedIndex = 0;
        }

        private void FormManualFIS_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Visible)
            {
                if (socketFIS != null)
                {
                    socketFIS.deleConnected -= new TcpServerAsync.DeleConnected(socketFIS_deleConnected);
                    socketFIS.deleDisconnected -= new TcpServerAsync.DeleDisconnected(socketFIS_deleDisconnected);
                    socketFIS.deleSent -= new TcpServerAsync.DeleSent(socketFIS_deleSent);
                    socketFIS.deleReceived -= new TcpServerAsync.DeleReceived(socketFIS_deleReceived);
                }
            }
        }

        private void buttonSendF2_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.SOCKET_FIS.CLIENT_COUNT > 0)
            {
                int nSelected = comboBoxResultF2.SelectedIndex;

                if (nSelected < 0)
                    return;

                FisMsg fmsg = new FisMsg();
                fmsg.SetValue("F2", nSelected.ToString(), "");
                fmsg.Packing();
                GlobalVariable.SOCKET_FIS.GetClientSocket(0).Send(fmsg.PACKET);
            }
            else
            {
                MessageBox.Show("FIS is not connected.");
            }
        }

        private void buttonSendF4_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.SOCKET_FIS.CLIENT_COUNT > 0)
            {
                int nSelected = comboBoxResultF4.SelectedIndex;

                if (nSelected < 0)
                    return;

                FisMsg fmsg = new FisMsg();
                fmsg.SetValue("F4", nSelected.ToString(), "");
                fmsg.Packing();
                GlobalVariable.SOCKET_FIS.GetClientSocket(0).Send(fmsg.PACKET);
            }
        }

        private void FormManualFIS_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                if (socketFIS != null)
                {
                    socketFIS.deleConnected += new TcpServerAsync.DeleConnected(socketFIS_deleConnected);
                    socketFIS.deleDisconnected += new TcpServerAsync.DeleDisconnected(socketFIS_deleDisconnected);
                    socketFIS.deleSent += new TcpServerAsync.DeleSent(socketFIS_deleSent);
                    socketFIS.deleReceived += new TcpServerAsync.DeleReceived(socketFIS_deleReceived);
                }
            }
            else
            {
                if (socketFIS != null)
                {
                    socketFIS.deleConnected -= new TcpServerAsync.DeleConnected(socketFIS_deleConnected);
                    socketFIS.deleDisconnected -= new TcpServerAsync.DeleDisconnected(socketFIS_deleDisconnected);
                    socketFIS.deleSent -= new TcpServerAsync.DeleSent(socketFIS_deleSent);
                    socketFIS.deleReceived -= new TcpServerAsync.DeleReceived(socketFIS_deleReceived);
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

            listViewFIS.Items.Add(item);

            if (listViewFIS.Items.Count > 100)
                listViewFIS.Items.RemoveAt(0);
        }

        #region ##### SOCKET MES EVENT #####

        void socketFIS_deleReceived(System.Net.Sockets.Socket sw, string msg)
        {
            if (this.InvokeRequired)
                this.Invoke(new DeleReceived(ReceivedFIS), sw, msg);
            else
                ReceivedFIS(sw, msg);
        }

        void socketFIS_deleSent(System.Net.Sockets.Socket sw, int sentLen, string msg)
        {
            if (this.InvokeRequired)
                this.Invoke(new DeleSent(SentFIS), sw, sentLen, msg);
            else
                SentFIS(sw, sentLen, msg);
        }

        void socketFIS_deleDisconnected(string msg = "")
        {
            if (this.InvokeRequired)
                this.Invoke(new DeleDisconnected(DisConnectedFIS), msg);
            else
                DisConnectedFIS(msg);
        }

        void socketFIS_deleConnected(bool bConnected, string msg = "")
        {
            if (this.InvokeRequired)
                this.Invoke(new DeleConnected(ConnectedFIS), bConnected, msg);
            else
                ConnectedFIS(bConnected, msg);
        }

        void ReceivedFIS(Socket sw, string msg)
        {
            AddListView(false, msg);

            FisMsg fmsg = new FisMsg();

            fmsg.Parsing(msg);

            if (fmsg.COMMAND_ID == "F1")
            {
                textBoxDateF1.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (fmsg.COMMAND_ID == "F3")
            {
                textBoxDateF3.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                if(fmsg.RESULT.Trim() == "0")
                    textBoxResultF3.Text = "OK";
                else
                    textBoxResultF3.Text = "NG";

                textBoxCoatingAmountF3.Text = fmsg.VALUE;
            }
            else if (fmsg.COMMAND_ID == "FA")
            {
                if (fmsg.RESULT == "S")
                {
                    textBoxAlarmStatus.Text = "START";
                }
                else
                {
                    textBoxAlarmStatus.Text = "END";
                }
                textBoxAlarmCode.Text = fmsg.VALUE;
            }
        }

        void SentFIS(Socket sw, int sentLen, string msg)
        {
            AddListView(true, msg);
        }

        void DisConnectedFIS(string msg)
        {
            AddListView("C", string.Format("Disconnected. {0}", msg));
        }

        void ConnectedFIS(bool bConnected, string msg = "")
        {
            if (bConnected)
            {
                AddListView("C", "Connection succeeded");
            }
            else
            {
                AddListView("C", string.Format("Connection failed. {0}", msg));
            }
        }
        #endregion
    }
}
