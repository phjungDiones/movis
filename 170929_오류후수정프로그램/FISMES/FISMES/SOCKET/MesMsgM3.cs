using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FISMES.SOCKET
{
    class MesMsgM3 : MesMsg
    {
        public void SetValue(string barcodeNo, bool processResult, double dCoatingQuantity = 0.0)
        {
            this.eqpID = "";
            this.processFlag = "M3";
            this.step = "1";
            this.bcrType = "1";
            this.bcrNo = barcodeNo;
            this.prod = "";
            this.status = processResult ? "0" : "1";
            this.inspSeq = "1";
            this.inspData = dCoatingQuantity.ToString("0.000");
            this.tagID = tagID;
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
