using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FISMES.SOCKET
{
    class MesMsgM1 : MesMsg
    {
        public void SetValue(string barcode, bool bBarcodeNg = false)
        {
            this.eqpID = "";
            this.processFlag = "M1";
            this.step = "1";
            this.bcrType = "1";
            this.bcrNo = barcode;
            this.prod = "";
            this.status = "0";// bBarcodeNg == false ? "0" : "1";
            this.inspSeq = "1";
            this.inspData = "";
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
