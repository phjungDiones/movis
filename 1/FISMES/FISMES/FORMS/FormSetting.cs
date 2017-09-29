using FISMES.DATA;
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

namespace FISMES.FORMS
{
    public partial class FormSetting : Form
    {
        public FormSetting()
        {
            InitializeComponent();
        }

        private void FormSetting_Load(object sender, EventArgs e)
        {

        }

        private void FormSetting_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void buttonSaveSetting_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("설정을 저장하시겠습니까?", "설정 저장", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                int port = 8080;
                if(!int.TryParse(textBoxPortMES.Text, out port))
                {
                    MessageBox.Show("Port는 정수로 입력해야 합니다.");
                    return;
                }
                SettingSocketMES.Instance.PORT = port;

                SettingSocketMES.Instance.MSG_ID = textBoxMsgIdMES.Text;
                SettingSocketMES.Instance.FACTORY_ID = textBoxFactoryIdMES.Text;
                SettingSocketMES.Instance.LINE_ID = textBoxLineIdMES.Text;
                SettingSocketMES.Instance.OPER_ID = textBoxOperIdMES.Text;
                SettingSocketMES.Instance.EQP_POS_ID = textBoxEqpIdMES.Text;

                SettingSocketMES.Instance.Serialize(PathManager.Instance.PATH_SETTING_SOCKET_MES);


                port = 8080;
                if (!int.TryParse(textBoxPortFIS.Text, out port))
                {
                    MessageBox.Show("Port는 정수로 입력해야 합니다.");
                    return;
                }
                SettingSocketFIS.Instance.PORT = port;

                SettingSocketFIS.Instance.Serialize(PathManager.Instance.PATH_SETTING_SOCKET_FIS);

                MessageBox.Show("설정이 저장되었습니다. Port 변경 시에는 프로그램을 다시 실행하셔야 합니다.");
            }
        }

        private void FormSetting_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                textBoxPortMES.Text = SettingSocketMES.Instance.PORT.ToString();
                textBoxMsgIdMES.Text = SettingSocketMES.Instance.MSG_ID;
                textBoxFactoryIdMES.Text = SettingSocketMES.Instance.FACTORY_ID;
                textBoxLineIdMES.Text = SettingSocketMES.Instance.LINE_ID;
                textBoxOperIdMES.Text = SettingSocketMES.Instance.OPER_ID;
                textBoxEqpIdMES.Text = SettingSocketMES.Instance.EQP_POS_ID;

                textBoxPortFIS.Text = SettingSocketFIS.Instance.PORT.ToString();
            }
        }

        private void toggleSwitchF2OnlyZero_CheckedChanged(object sender, EventArgs e)
        {
            GlobalVariable.RESPONSE_F2_ONLY_ZERO = toggleSwitchF2OnlyZero.Checked;
        }
    }
}
