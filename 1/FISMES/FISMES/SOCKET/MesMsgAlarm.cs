using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FISMES.SOCKET
{
    /// <summary>
    /// MES 알람 메시지 정의
    /// </summary>
    class MesMsgAlarm : MesMsg
    {
        public void SetValue(bool alarmOn, string alarmCode)
        {
            this.eqpID = "";
            this.processFlag = "MA";
            this.step = alarmOn ? "S" : "E";
            this.bcrType = "";
            this.bcrNo = "";
            this.prod = "";
            this.status = "";
            this.inspSeq = "";
            this.inspData = alarmCode;
            this.tagID = "";
            this.mesResult = "";
            this.udf1 = "";
            this.udf2 = "";
            this.udf3 = "";
            this.udf4 = "";
            this.udf5 = "";
            this.mesCode = "";
            this.mesMsg = "";
        }
    }
}
