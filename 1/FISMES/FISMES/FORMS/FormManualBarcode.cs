using DionesTool.DEVICE.SERIAL.C0405;
using FISMES.GLOBAL;
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

namespace FISMES.FORMS
{
    public partial class FormManualBarcode : Form
    {
        public FormManualBarcode()
        {
            InitializeComponent();
        }

        private void FormManualBarcode_Load(object sender, EventArgs e)
        {
            
        }

        private void FormManualBarcode_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Visible)
            {
                GlobalVariable.DEV_BARCODE.deleException -= new DevBarcodeDm260.DeleException(DEV_BARCODE_deleException);
                GlobalVariable.DEV_BARCODE.deleReceived -= new DevBarcodeDm260.DeleReceived(DEV_BARCODE_deleReceived);
                GlobalVariable.DEV_BARCODE.deleSent -= new DevBarcodeDm260.DeleSent(DEV_BARCODE_deleSent);
            }
        }

        private void FormManualBarcode_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void FormManualBarcode_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                GlobalVariable.DEV_BARCODE.deleException += new DevBarcodeDm260.DeleException(DEV_BARCODE_deleException);
                GlobalVariable.DEV_BARCODE.deleReceived += new DevBarcodeDm260.DeleReceived(DEV_BARCODE_deleReceived);
                GlobalVariable.DEV_BARCODE.deleSent += new DevBarcodeDm260.DeleSent(DEV_BARCODE_deleSent);
            }
            else
            {
                GlobalVariable.DEV_BARCODE.deleException -= new DevBarcodeDm260.DeleException(DEV_BARCODE_deleException);
                GlobalVariable.DEV_BARCODE.deleReceived -= new DevBarcodeDm260.DeleReceived(DEV_BARCODE_deleReceived);
                GlobalVariable.DEV_BARCODE.deleSent -= new DevBarcodeDm260.DeleSent(DEV_BARCODE_deleSent);
            }
        }

        void DEV_BARCODE_deleSent(string strMsg)
        {
            AddListView(true, strMsg);
        }

        void DEV_BARCODE_deleReceived(string strBarcode)
        {
            AddListView(false, strBarcode);

            if (this.Visible)
            {
                buttonSendTrg.Enabled = true;
                GlobalVariable.DEV_BARCODE.TriggerOff();
            }
        }

        void DEV_BARCODE_deleException(string strErr)
        {
            //
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

            listViewBarcode.Items.Add(item);

            if (listViewBarcode.Items.Count > 100)
                listViewBarcode.Items.RemoveAt(0);
        }

        private void buttonSendTrg_Click(object sender, EventArgs e)
        {
            GlobalVariable.DEV_BARCODE.TriggerOn();
            buttonSendTrg.Enabled = false;
        }
    }
}
