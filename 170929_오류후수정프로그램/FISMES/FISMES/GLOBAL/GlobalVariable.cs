using DionesTool.DEVICE.SERIAL.C0405;
using DionesTool.Tcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISMES.SOCKET;
using FISMES.ProgramThread;
using FISMES.DATA;

namespace FISMES.GLOBAL
{
    public static class GlobalVariable
    {
        // 바코드 리더기 장치에서 읽은 바코드 값
        public static string BARCODE
        {
            get;
            set;
        }

        public static bool FLAG_RECV_BARCODE
        {
            get;
            set;
        }

        /// <summary>
        /// MES 통신으로 받은 바코드 정보
        /// </summary>
        public static string BARCODE_NO
        {
            get;
            set;
        }

        public static MesMsg MES_MSG
        {
            get;
            set;
        }

        public static bool FLAG_RECV_MES
        {
            get;
            set;
        }

        public static FisMsg FIS_MSG
        {
            get;
            set;
        }

        public static bool FLAG_RECV_FIS
        {
            get;
            set;
        }

        public static TcpServerAsync SOCKET_MES = new TcpServerAsync();
        public static TcpServerAsync SOCKET_FIS = new TcpServerAsync();
        //public static C0405 DEV_C0405 = new C0405();
        public static DevBarcodeDm260 DEV_BARCODE = new DevBarcodeDm260();

        public static AutoRun autoRun = new AutoRun();

        public static string M2_RESULT = "";
        public static bool RESPONSE_F2_ONLY_ZERO = false;

        static GlobalVariable()
        {
            BARCODE = "";
            FLAG_RECV_BARCODE = false;
            BARCODE_NO = "";
            FLAG_RECV_MES = false;
            FLAG_RECV_FIS = false;
        }
    }
}
