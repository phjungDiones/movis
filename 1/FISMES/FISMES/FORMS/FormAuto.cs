using DionesTool.DEVICE.SERIAL.C0405;
using FISMES.DATA;
using FISMES.GLOBAL;
using FISMES.SOCKET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FISMES.FORMS
{
    public enum COMM_TARGET
    {
        FIS,
        BARCODE,
        MES,
    }

    public partial class FormAuto : Form
    {
        public delegate void DeleChangeAutoMode(bool bAutoMode);
        public event DeleChangeAutoMode OnChangeAutoMode = null;

        public FormAuto()
        {
            InitializeComponent();
        }

        private void FormAuto_Load(object sender, EventArgs e)
        {
            listViewFIS.Items.Clear();
            listViewBarcode.Items.Clear();
            listViewMES.Items.Clear();
        }

        public void AddListView(COMM_TARGET target, bool flagSend, string msg)
        {
            AddListView(target, flagSend ? "->" : "<-", msg);
        }

        public void AddListView(COMM_TARGET target, string sendMsg, string msg)
        {
            ListViewItem item = new ListViewItem(sendMsg);
            item.SubItems.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            item.SubItems.Add(msg);

            ListView listView = null;
            switch (target)
            {
                case COMM_TARGET.FIS:
                    listView = listViewFIS;
                    break;
                case COMM_TARGET.BARCODE:
                    listView = listViewBarcode;
                    break;
                case COMM_TARGET.MES:
                    listView = listViewMES;
                    break;
                default:
                    break;
            }
            listView.Items.Add(item);

            if (listView.Items.Count > 100)
                listView.Items.RemoveAt(0);
        }

        public void TryConnect(COMM_TARGET target)
        {
            JCS.ToggleSwitch toggleSwitch = null;
            switch (target)
            {
                case COMM_TARGET.FIS:
                    toggleSwitch = toggleSwitchFIS;
                    break;
                case COMM_TARGET.BARCODE:
                    toggleSwitch = toggleSwitchBarcode;
                    break;
                case COMM_TARGET.MES:
                    toggleSwitch = toggleSwitchMES;
                    break;
                default:
                    return;
            }

            toggleSwitch.OnText = "Connecting";
            toggleSwitch.Checked = true;
            toggleSwitch.Enabled = false;
        }

        public void Connected(COMM_TARGET target)
        {
            JCS.ToggleSwitch toggleSwitch = null;
            switch (target)
            {
                case COMM_TARGET.FIS:
                    toggleSwitch = toggleSwitchFIS;
                    break;
                case COMM_TARGET.BARCODE:
                    toggleSwitch = toggleSwitchBarcode;
                    break;
                case COMM_TARGET.MES:
                    toggleSwitch = toggleSwitchMES;
                    break;
                default:
                    return;
            }

            toggleSwitch.OnText = "Online";
            toggleSwitch.Checked = true;
            toggleSwitch.Enabled = true;
            //toggleSwitch.AllowUserChange = false; //!toggleSwitchAuto.Checked; // 연결되면 무조건 연결 끊기 안되게
        }

        public void Disconnected(COMM_TARGET target)
        {
            JCS.ToggleSwitch toggleSwitch = null;
            switch (target)
            {
                case COMM_TARGET.FIS:
                    toggleSwitch = toggleSwitchFIS;
                    break;
                case COMM_TARGET.BARCODE:
                    toggleSwitch = toggleSwitchBarcode;
                    break;
                case COMM_TARGET.MES:
                    toggleSwitch = toggleSwitchMES;
                    break;
                default:
                    return;
            }

            toggleSwitch.Checked = false;
            //toggleSwitch.AllowUserChange = true;

            //SetAutoMode(false);
        }

        private void toggleSwitchAuto_CheckedChanged(object sender, EventArgs e)
        {
            //toggleSwitchFIS.AllowUserChange = !toggleSwitchAuto.Checked;
            //toggleSwitchRFID.AllowUserChange = !toggleSwitchAuto.Checked;
            //toggleSwitchMES.AllowUserChange = !toggleSwitchAuto.Checked;

            SetAutoMode(toggleSwitchAuto.Checked);

            Console.WriteLine("toggleSwitchAuto_CheckedChanged : " + toggleSwitchAuto.Checked.ToString());
        }

        public void SetAutoMode(bool autoMode)
        {
            if (toggleSwitchAuto.Checked != autoMode)
                toggleSwitchAuto.Checked = autoMode;

            if (MachineStatus.AUTO_MODE != toggleSwitchAuto.Checked)
            {
                MachineStatus.AUTO_MODE = toggleSwitchAuto.Checked;

                if (OnChangeAutoMode != null)
                    OnChangeAutoMode(autoMode);
            }
        }

        public void RefreshInfoFIS(string strDate, string strResult, string strValue)
        {
            textBoxDateFIS.Text = strDate;
            textBoxResultFIS.Text = strResult;
            textBoxValueFIS.Text = strValue;
        }

        public void RefreshInfoBarcode(string strDate, string strRFID)
        {
            textBoxDateBarcode.Text = strDate;
            textBoxBarcode.Text = strRFID;
        }

        public void RefreshInfoMES(string strDate, string strBarcode)
        {
            textBoxDateMES.Text = strDate;
            textBoxRecvBarcode.Text = strBarcode;
        }
    }
}
