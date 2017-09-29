using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FISMES.SOCKET
{
    public enum MES_BCR_TYPE : byte
    {
        PCB_NO = 1,
        PALLETE_NO = 2,
        PCB_LIST = 3,
    }

    public class MesMsg
    {
        protected string msgID = "";       // 메시지 ID
        protected string eqpID = "";       // 설비 ID
        protected string processFlag = ""; // 메시지 구분 ('M1' / 'M2' / 'M3' / 'M4')
        protected string factoryID = "";   // 공장 ID (설정정보. ex : 6000)
        protected string lineID = "";      // 라인 ID (설정정보. ex : PM03)
        protected string operID = "";      // 공정 ID (설정정보. 3자리 숫자(공정번호가 100미만의 공정은 코드 앞에 0을 채운다. ex : '070') -> PM04-150 포맷으로 변경
        protected string transTime = "";   // 전송시간 ('yyyymmddHHmmss')
        protected string step = "";        // 공정 구분
        protected string eqpPosID = "";    // 설비 위치 ID (설정정보)
        protected string bcrType = "";     // 시리얼 유형 ('1' : PCB번호 / '2' : 팔레트번호 / '3' : PCB LIST(MAX 10개). ex : M1,M2 : 3  M3,M4 : 1)
        protected string bcrNo = "";       // 시리얼 번호 (바코드 스캔 정보. 제품의 시리얼 번호 | 복수 제품 시리얼 번호 전송시 구분자 '^')
        protected string tagID = "";       // SKID(RFID TAG ID) (M1,M3 : <empty string> | M2 : SKID(RFID TAG ID) 전송)
        protected string status = "";      // 설비 결과 (M1 : 무조건 '0' / M3 : 공정 OK : '0', NG : '1')
        protected string inspSeq = "";     // 검사 순번
        protected string inspData = "";    // 검사 데이터
        protected string prod = "";         // 제품코드(품번)
        protected string mesResult = "";   // MES 결과 (M1,M3 : <empty string> | M2,M4 : OK:'0', NG:'1', SKIP:'2' 전송)
        protected string udf1 = "";        // 사용자 정의 필드 #1
        protected string udf2 = "";        // 사용자 정의 필드 #2
        protected string udf3 = "";        // 사용자 정의 필드 #3
        protected string udf4 = "";        // 사용자 정의 필드 #4
        protected string udf5 = "";        // 사용자 정의 필드 #5
        protected string mesCode = "";     // MES 메시지 코드
        protected string mesMsg = "";      // MES 메시지


        //protected string model = "";       // 모델 정보 (차종 정보(PROGRAM_NAME) | M2 메시지만 전송 | M1,M3,M4 <empty string>)
        
        
        
        


        string packet = string.Empty;
        bool valueChanged = false;

        public byte[] PACKET
        {
            get
            {
                if (valueChanged) Packing();
                return Encoding.ASCII.GetBytes(packet.ToString()); 
            }
        }

        public string PACKET_STRING
        {
            get { return packet; }
        }

        public string MSG_ID
        {
            get { return msgID; }
        }

        public string BARCODE_NO
        {
            get { return bcrNo; }
        }

        public string MES_RESULT
        {
            get { return mesResult; }
        }

        public string MES_PROCESS_FLAG
        {
            get { return processFlag; }
        }

        public void Packing()
        {
            StringBuilder pack = new StringBuilder();
            pack.Append((char)0x02);
            pack.Append(msgID);
            pack.Append("/");
            pack.Append(eqpID);
            pack.Append("/");
            pack.Append(processFlag);
            pack.Append("/");
            pack.Append(factoryID);
            pack.Append("/");
            pack.Append(lineID);
            pack.Append("/");
            pack.Append(operID);
            pack.Append("/");
            pack.Append(DateTime.Now.ToString("yyyyMMddHHmmss"));
            pack.Append("/");
            pack.Append(step);
            pack.Append("/");
            pack.Append(eqpPosID);
            pack.Append("/");
            pack.Append(bcrType);
            pack.Append("/");
            pack.Append(bcrNo);
            pack.Append("/");
            pack.Append(tagID);
            pack.Append("/");
            pack.Append(status);
            pack.Append("/");
            pack.Append(inspSeq);
            pack.Append("/");
            pack.Append(inspData);
            pack.Append("/");
            pack.Append(prod);
            pack.Append("/");
            pack.Append(mesResult);
            pack.Append("/");
            pack.Append(udf1);
            pack.Append("/");
            pack.Append(udf2);
            pack.Append("/");
            pack.Append(udf3);
            pack.Append("/");
            pack.Append(udf4);
            pack.Append("/");
            pack.Append(udf5);
            pack.Append("/");
            pack.Append(mesCode);
            pack.Append("/");
            pack.Append(mesMsg);
            pack.Append((char)0x03);

            packet = pack.ToString();
            pack.Clear();

            valueChanged = false;
        }
        public void Packing_false()
        {
            StringBuilder pack = new StringBuilder();
            pack.Append((char)0x02);
            pack.Append(msgID);
            pack.Append("/");
            pack.Append(eqpID);
            pack.Append("/");
            pack.Append(processFlag);
            pack.Append("/");
            pack.Append(factoryID);
            pack.Append("/");
            pack.Append(lineID);
            pack.Append("/");
            pack.Append(operID);
            pack.Append("/");
            pack.Append(DateTime.Now.ToString("yyyyMMddHHmmss"));
            pack.Append("/");
            pack.Append(step);
            pack.Append("/");
            pack.Append(eqpPosID);
            pack.Append("/");
            pack.Append(bcrType);
            pack.Append("/");
            pack.Append("0");
            pack.Append("/");
            pack.Append(tagID);
            pack.Append("/");
            pack.Append(status);
            pack.Append("/");
            pack.Append(inspSeq);
            pack.Append("/");
            pack.Append(inspData);
            pack.Append("/");
            pack.Append(prod);
            pack.Append("/");
            pack.Append(mesResult);
            pack.Append("/");
            pack.Append(udf1);
            pack.Append("/");
            pack.Append(udf2);
            pack.Append("/");
            pack.Append(udf3);
            pack.Append("/");
            pack.Append(udf4);
            pack.Append("/");
            pack.Append(udf5);
            pack.Append("/");
            pack.Append(mesCode);
            pack.Append("/");
            pack.Append(mesMsg);
            pack.Append((char)0x03);

            packet = pack.ToString();
            pack.Clear();

            valueChanged = false;
        }

        /// <summary>
        /// 설정 값 지정
        /// </summary>
        public void SetSettings(string msgID, string factoryID, string lineID, string operID, string eqpPosID)
        {
            this.msgID = msgID;
            this.factoryID = factoryID;
            this.lineID = lineID;
            this.operID = operID;//.PadLeft(3, '0');
            this.eqpPosID = eqpPosID;

            valueChanged = true;
        }

        public bool Parsing(string pack)
        {
            this.packet = pack;

            char[] delimeter = { '/' };

            // 내용이 있는지 확인
            if (this.packet.Length < 2)
                return false;

            // packet의 첫번째 STX(0x02), 마지막 ETX(0x03)을 확인
            if (this.packet[0] != 0x02 || this.packet[this.packet.Length - 1] != 0x03)
                return false;

            // STX, ETX를 제거
            string contents = this.packet.Substring(1, this.packet.Length - 2);

            // 내용을 분할
            string[] arr = contents.Split(delimeter);

            // 분할 개수가 24인지 확인
            if (arr.Length != 24)
                return false;

            int nIndex = 0;
            msgID       = arr[nIndex++];
            eqpID       = arr[nIndex++];
            processFlag = arr[nIndex++];
            factoryID   = arr[nIndex++];
            lineID      = arr[nIndex++];
            operID      = arr[nIndex++];
            transTime   = arr[nIndex++];
            step        = arr[nIndex++];
            eqpPosID    = arr[nIndex++];
            bcrType     = arr[nIndex++];
            bcrNo       = arr[nIndex++];
            tagID       = arr[nIndex++];
            status      = arr[nIndex++];
            inspSeq     = arr[nIndex++];
            inspData    = arr[nIndex++];
            prod        = arr[nIndex++];
            mesResult   = arr[nIndex++];
            udf1        = arr[nIndex++];
            udf2        = arr[nIndex++];
            udf3        = arr[nIndex++];
            udf4        = arr[nIndex++];
            udf5        = arr[nIndex++];
            mesCode     = arr[nIndex++];
            mesMsg      = arr[nIndex++];

            return true;
        }
    }
}
