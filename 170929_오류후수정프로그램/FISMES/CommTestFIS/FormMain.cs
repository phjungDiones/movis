using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DionesTool.Tcp;

namespace CommTestFIS
{
    public partial class FormMain : Form
    {
        TcpServerAsync socketFIS = new TcpServerAsync();

        public FormMain()
        {
            InitializeComponent();

            comboBoxPreviousResult.SelectedIndex = 0;
            comboBoxAnswerProcessResult.SelectedIndex = 0;

            listView1.Columns.Clear();
            listView1.Columns.Add("recData", 323);
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            if (!SettingSocketFIS.Instance.Deserialize(PathManager.Instance.PATH_SETTING_SOCKET_FIS))
            {
                SettingSocketFIS.Instance.PORT = 9000;
                SettingSocketFIS.Instance.Serialize(PathManager.Instance.PATH_SETTING_SOCKET_FIS);
            }

            System.Net.IPAddress addr = System.Net.IPAddress.Parse(SettingSocketFIS.Instance.IPADDR);
            socketFIS.SetServerInfo(addr, SettingSocketFIS.Instance.PORT, 1);
            socketFIS.SetDelimeter(TcpBase.ETX.ToString());

            socketFIS.deleConnected += new TcpServerAsync.DeleConnected(socketFIS_deleConnected);
            socketFIS.deleDisconnected += new TcpServerAsync.DeleDisconnected(socketFIS_deleDisconnected);
            socketFIS.deleReceived += new TcpServerAsync.DeleReceived(socketFIS_deleReceived);
            socketFIS.deleSent += new TcpServerAsync.DeleSent(socketFIS_deleSent);

            socketFIS.Start();

            textBoxPort.Text = SettingSocketFIS.Instance.PORT.ToString();
        }

        void socketFIS_deleSent(System.Net.Sockets.Socket sw, int sentLen, string msg)
        {
            this.Invoke((MethodInvoker)delegate()
            {
                listView1.Items.Add("message sent");
            });
        }

        void socketFIS_deleReceived(System.Net.Sockets.Socket sw, string msg)
        {
            this.Invoke((MethodInvoker)delegate()
            {
                listView1.Items.Add(msg);

                FisMsg fmsg = new FisMsg();
                if (fmsg.Parsing(msg))
                {
                    if(fmsg.COMMAND_ID == "F1")
                    {
                        // 할게 없다
                    }
                    else if (fmsg.COMMAND_ID == "F3")
                    {
                        // 화면에 표시
                        if(fmsg.RESULT.Trim() == "0")
                        {
                            textBoxProcessResult.Text = "OK";
                            textBoxCoatingAmount.Text = fmsg.VALUE;
                        }
                        else
                        {
                            textBoxProcessResult.Text = "NG";
                            textBoxCoatingAmount.Text = "";
                        }
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
                    else
                    {
                        listView1.Items.Add(string.Format("Message command ID is wrong. Command ID : {0}", fmsg.COMMAND_ID));
                    }
                }
                else
                {
                    listView1.Items.Add(string.Format("Message is wrong. Message : {0}", msg));
                }
            });
        }

        void socketFIS_deleDisconnected(string msg = "")
        {
            this.Invoke((MethodInvoker)delegate()
            {
                if(msg == "")
                    listView1.Items.Add("Disconnected");
                else
                    listView1.Items.Add(string.Format("Disconnected : {0}", msg));
            });
        }

        void socketFIS_deleConnected(bool bConnected, string msg = "")
        {
            this.Invoke((MethodInvoker)delegate()
            {
                listView1.Items.Add("Connected");
            });
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            socketFIS.deleConnected -= new TcpServerAsync.DeleConnected(socketFIS_deleConnected);
            socketFIS.deleDisconnected -= new TcpServerAsync.DeleDisconnected(socketFIS_deleDisconnected);
            socketFIS.deleReceived -= new TcpServerAsync.DeleReceived(socketFIS_deleReceived);
            socketFIS.deleSent -= new TcpServerAsync.DeleSent(socketFIS_deleSent);

            socketFIS.Stop(3000);
        }

        private void buttonSendF2_Click(object sender, EventArgs e)
        {
            if (socketFIS.CLIENT_COUNT > 0)
            {
                int nSelected = comboBoxPreviousResult.SelectedIndex;

                if (nSelected < 0)
                    return;

                FisMsg fmsg = new FisMsg();
                fmsg.SetValue("F2", nSelected.ToString(), "");
                fmsg.Packing();
                socketFIS.GetClientSocket(0).Send(fmsg.PACKET);
            }
        }

        private void buttonSendF4_Click(object sender, EventArgs e)
        {
            if (socketFIS.CLIENT_COUNT > 0)
            {
                int nSelected = comboBoxAnswerProcessResult.SelectedIndex;

                if (nSelected < 0)
                    return;

                FisMsg fmsg = new FisMsg();
                fmsg.SetValue("F4", nSelected.ToString(), "");
                fmsg.Packing();
                socketFIS.GetClientSocket(0).Send(fmsg.PACKET);
            }
        }
    }
}
