using DionesTool.DATA;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DWORD = System.UInt32;
using WORD = System.UInt16;

namespace DionesTool.DEVICE.SERIAL.C0405
{
    public class C0405 : SerialPort
    {
        public delegate void DeleException(string strErr);
        public event DeleException deleException = null;

        #region ### delegate ###
        public delegate void DeleFillTag();
        public event DeleFillTag deleFillTag = null;

        public delegate void DeleReadData(byte[] data);
        public event DeleReadData deleReadData = null;

        public delegate void DeleWriteData();
        public event DeleWriteData deleWriteData = null;

        public delegate void DeleReadTagID(byte[] data);
        public event DeleReadTagID deleReadTagID = null;

        public delegate void DeleTagSearch();
        public event DeleTagSearch deleTagSearch = null;

        public delegate void DeleStartContiRead(byte[] data);
        public event DeleStartContiRead deleStartContiRead = null;

        public delegate void DeleStopContiRead();
        public event DeleStopContiRead deleStopContiRead = null;

        public delegate void DeleStartContiReadData(byte[] data, byte[] tagID);
        public event DeleStartContiReadData deleStartContiReadData = null;

        public delegate void DeleStopContiReadData();
        public event DeleStopContiReadData deleStopContiReadData = null;

        public delegate void DeleResetController();
        public event DeleResetController deleResetController = null;

        public delegate void DeleSetConfig();
        public event DeleSetConfig deleSetConfig = null;

        public delegate void DeleGetConfig(C0405Packet packet);
        public event DeleGetConfig deleGetConfig = null;

        public delegate void DeleGetConfigInfo();
        public event DeleGetConfigInfo deleGetConfigInfo = null;

        public delegate void DeleError(byte command, byte errorCode);
        public event DeleError deleError = null;
        #endregion ### delegate ###

        /// <summary>
        /// 명령어
        /// </summary>
        public enum COMMAND : byte
        {
            FILL_TAG = 0x04,
            READ_DATA = 0x05,
            WRITE_DATA = 0x06,
            READ_TAG_ID = 0x07,
            TAG_SEARCH = 0x08,
            START_CONTI_READ = 0x0D,
            START_CONTI_READ_DATA = 0x0F,
            RESET_CONTROLLER = 0x35,
            SET_CONFIG = 0x36,
            GET_CONFIG = 0x37,
            GET_CONFIG_INFO = 0x38,
            ERROR = 0xFF,
        }

        /// <summary>
        /// 에러 코드
        /// </summary>
        public enum ERROR_CODE : byte
        {
            FILL_TAG_FAILED = 0x04,
            READ_DATA_FAILED = 0x05,
            WRITE_DATA_FAILED = 0x06,
            READ_TAG_ID_FAILED = 0x07,
            INVALID_SYNTAX = 0x21,
            INVALID_TAG_TYPE = 0x23,
            LOCK_TAG_BLOCK_FAILED = 0x27,
            INTERNAL_CONTROLLER_ERROR = 0x30,
            INVALID_CONTROLLER_TYPE = 0x31,
            INVALID_PROGRAMMING_ADDRESS = 0x32,
            CRC_ERROR = 0x33,
            INVALID_SOFTWARE_VERSION = 0x34,
            INVALID_RESET = 0x35,
            SET_CONFIG_ERROR = 0x36,
            GET_CONFIG_ERROR = 0x37,
        }

        #region ### Config 관련 enum 정의 ###
        /// <summary>
        /// Config - Tag IC
        /// </summary>
        enum TAG_IC : byte
        {
            BIS_M_1_03 = 0x01,  // BIS M-1___-03__ (ISO 15693, I-CODE SLI)
            BIS_M_1_07 = 0x03,  // BIS M-1___-07__ (ISO 15693)
            BIS_M_1_10 = 0x05,  // BIS M-1___-10__ (Mifare)
            BIS_M_1_02 = 0x06,  // BIS M-1___-02__ (ISO 15693)
        }

        /// <summary>
        /// Config - Communications Mode Baud Rate
        /// </summary>
        enum COMM_MODE_BAUD_RATE : byte
        {
            BAUD_RATE_9600 = 0x00,
            BAUD_RATE_19200 = 0x01,
            BAUD_RATE_38400 = 0x02,
            BAUD_RATE_57600 = 0x03,
            BAUD_RATE_115200 = 0x04,
        }

        /// <summary>
        /// Config - Communications Mode XON/XOFF
        /// </summary>
        enum COMM_MODE_XON_XOFF : byte
        {
            XON_XOFF_DISABLE = 0x00,
            XON_XOFF_ENABLE = 0x08,
        }

        /// <summary>
        /// Config - Communications Mode Interface
        /// </summary>
        enum COMM_MODE_INTERFACE : byte
        {
            INTERFACE_RS_422 = 0x00,
            INTERFACE_RS_232_USB = 0x40,
            INTERFACE_RS485 = 0x80,
            INTERFACE_OTHERS = 0xC0,
        }

        /// <summary>
        /// Config - Options Byte 1
        /// </summary>
        [Flags]
        enum OPTIONS_BYTE1 : byte
        {
            NONE = 0x00,
            TAG_IDS = 0x01,
            RESERVED_1 = 0x02,
            RESERVED_2 = 0x04,
            RESERVED_3 = 0x08,
            RESERVED_4 = 0x10,
            TAG_IDS_CONTI_READ = 0x20,
            TAG_PRESENCE_ON = 0x40,
            TOGGLE_RF = 0x80,
        }

        /// <summary>
        /// Config - Options Byte 2
        /// </summary>
        [Flags]
        enum OPTIONS_BYTE2 : byte
        {
            NONE = 0x00,
            FOUR_BYTE_TAG_IDS = 0x01,
            LEGACY_ERROR_CODES = 0x02,
            RESERVED_2 = 0x04,
            RESERVED_3 = 0x08,
            RESERVED_4 = 0x10,
            RESERVED_5 = 0x20,
            RESERVED_6 = 0x40,
            RESERVED_7 = 0x80,
        }

        /// <summary>
        /// Config - Controller Type
        /// </summary>
        enum CONTROLLER_TYPE : byte
        {
            HF_0405 = 0x01,     // 
            HF_CNTL = 0x02,     // BIS_M_62
            C_0405 = 0x03,      // BIS_M_410
            C_1007 = 0x04,      // BIS_M_411
            HF_81X = 0x05,      //
            BSI_M_U_62 = 0x0B,  // BSI_M_U_62
        }
        #endregion ### Config 관련 enum 정의 ###


        // 포트 정보
        PortInfo portInfo = new PortInfo();

        // 설정 정보 (GetControllerConfig 시 반환 값을 저장)
        byte[] bytesConfigInfo = null;

        // 설정 정보를 가져왔는지 여부
        bool flagGetCtrlConfig = false;

        // Checksum 사용할지 여부
        bool flagUseChecksum = false;

        // 생성자
        public C0405() : base() 
        {
        }

        ~C0405()
        {
        }

        /// <summary>
        /// 포트 정보를 파일에 쓰기
        /// </summary>
        /// <param name="filePath">경로</param>
        public bool Serialize(string filePath)
        {
            return portInfo.SetInfo(this, filePath);
        }

        /// <summary>
        /// 파일에서 포트 정보 불러오기
        /// </summary>
        /// <param name="filePath">경로</param>
        public bool Deserialize(string filePath)
        {
            SerialPort port = this;
            return portInfo.GetInfo(ref port, filePath);
        }

        public void SetConfig(bool useChecksum)
        {
            flagUseChecksum = useChecksum;
        }

        /// <summary>
        /// 포트를 연다
        /// 부모 형식으로 호출하면 안됨
        /// </summary>
        /// <returns></returns>
        public new bool Open()
        {
            if (IsOpen) return true;

            try
            {
                base.Open();
            }
            catch (Exception ex)
            {
                if (deleException != null)
                    deleException(ex.ToString());
                return false;
            }

            this.DiscardInBuffer();
            this.DiscardOutBuffer();

            DataReceived += CRFIDCtrlC0405_DataReceived;

            return true;
        }

        /// <summary>
        /// 포트를 닫는다
        /// 부모 형식으로 호출하면 안됨
        /// </summary>
        public new void Close()
        {
            if (!IsOpen) return;

            DataReceived -= CRFIDCtrlC0405_DataReceived;

            base.Close();
        }

        /// <summary>
        /// 응답 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CRFIDCtrlC0405_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;

            // ReadExisting으로 하면 0xff가 잘못 들어온다. Read를 사용해서 해야함.
            //sp.ReadExisting();
            int length = sp.BytesToRead;
            byte[] buf = new byte[length];
            sp.Read(buf, 0, length);

            ParsingResponse(buf);

        }

        /// <summary>
        /// 응답 메시지 파싱
        /// </summary>
        /// <param name="data">넘어온 값</param>
        public void ParsingResponse(byte[] byteData)
        {
            try
            {
                C0405Packet packet = C0405Packet.Parsing(byteData, flagUseChecksum);

                if (packet == null)
                {
                    //
                    return;
                }

                switch ((COMMAND)packet.commandID)
                {
                    case COMMAND.FILL_TAG:
                        if (deleFillTag != null)
                            deleFillTag();
                        break;
                    case COMMAND.READ_DATA:
                        if (deleReadData != null)
                            deleReadData(packet.bytesData);
                        break;
                    case COMMAND.WRITE_DATA:
                        if (deleWriteData != null)
                            deleWriteData();
                        break;
                    case COMMAND.READ_TAG_ID:
                        if (deleReadTagID != null)
                            deleReadTagID(packet.tagID);
                        break;
                    case COMMAND.TAG_SEARCH:
                        if (deleTagSearch != null)
                            deleTagSearch();
                        break;
                    case COMMAND.START_CONTI_READ:
                        if (packet.bytesData == null)
                        {
                            if (deleStopContiRead != null)
                                deleStopContiRead();
                        }
                        else
                        {
                            if (deleStartContiRead != null)
                                deleStartContiRead(packet.bytesData);
                        }
                        break;
                    case COMMAND.START_CONTI_READ_DATA:
                        if (packet.bytesData == null)
                        {
                            if (deleStopContiReadData != null)
                                deleStopContiReadData();
                        }
                        else
                        {
                            if (deleStartContiReadData != null)
                                deleStartContiReadData(packet.bytesData, packet.tagID);
                        }
                        break;
                    case COMMAND.RESET_CONTROLLER:
                        if (deleResetController != null)
                            deleResetController();
                        break;
                    case COMMAND.SET_CONFIG:
                        if (deleSetConfig != null)
                            deleSetConfig();
                        break;
                    case COMMAND.GET_CONFIG:
                        // Set Config를 위한 플래그 설정 및 데이터 복사
                        flagGetCtrlConfig = true;
                        Buffer.BlockCopy(byteData, 0, bytesConfigInfo, 0, byteData.Length);

                        if (deleGetConfig != null)
                            deleGetConfig(packet);
                        break;
                    case COMMAND.GET_CONFIG_INFO:
                        if (deleGetConfigInfo != null)
                            deleGetConfigInfo();
                        break;
                    case COMMAND.ERROR:
                        if (deleError != null)
                            deleError(packet.commandID, packet.errorCode);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                if (deleException != null)
                    deleException(ex.ToString());
            }
            

            #region ### old code ###
            //byte[] byteData = Encoding.ASCII.GetBytes(data.ToString());

            //int nIndex = 0;

            //// Response Header
            //nIndex++;
            //nIndex++;

            //// Response Size
            //byte responseSize = byteData[nIndex++];

            //// responseSize와 byte 개수를 비교 확인
            //// checkLength : responseSize + header + etx + checksum
            //int checkLength = responseSize + 2 + 1;
            //if (flagUseChecksum) checkLength++;
            //if (checkLength != byteData.Length)
            //{
            //    // parsing 실패. 확인 필요
            //    Console.WriteLine("Response Parsing Failed. byte length diff.");
            //    return;
            //}

            //// Command Echo
            //byte commandEcho = byteData[nIndex++];

            ////////////////////////////////////
            //// byte 배열을 class나 struct로 정의하여 바로 복사할 수 있는지 테스트 필요!!!!!!!!!
            ////////////////////////////////////
            //switch ((COMMAND)commandEcho)
            //{
            //    case COMMAND.FILL_TAG:
            //        {
            //            // responseSize : 1 byte
            //            // Command Echo : 1 byte
            //        }
            //        break;
            //    case COMMAND.READ_DATA:
            //        {
            //            // responseSize : variable byte
            //            // Command Echo : 1 byte
            //            // data : nIndex부터 길이 responseSize-1까지 (Command Echo 제외함)
            //        }
            //        break;
            //    case COMMAND.WRITE_DATA:
            //        {
            //            // responseSize : 1 byte
            //            // Command Echo : 1 byte
            //        }
            //        break;
            //    case COMMAND.READ_TAG_ID:
            //        {
            //            // responseSize : 9 byte
            //            // Command Echo : 1 byte
            //            // Tag ID : 8 byte
            //        }
            //        break;
            //    case COMMAND.TAG_SEARCH:
            //        {
            //            // responseSize : 1 byte
            //            // Command Echo : 1 byte
            //        }
            //        break;
            //    case COMMAND.START_CONTI_READ:
            //        {
            //            // responseSize : 5 byte
            //            // Command Echo : 1 byte
            //            // Data : 4 byte

            //            // responseSize : 2 byte
            //            // Command Echo : 1 byte
            //        }
            //        break;
            //    case COMMAND.RESET_CONTROLLER:
            //        {
            //            // responseSize : 1 byte
            //            // Command Echo : 1 byte
            //        }
            //        break;
            //    case COMMAND.SET_CONFIG:
            //        {
            //            // responseSize : 1 byte
            //            // Command Echo : 1 byte
            //        }
            //        break;
            //    case COMMAND.GET_CONFIG:
            //        {
            //            // responseSize : 14 byte
            //            // Command Echo : 1 byte
            //            // Continuous Read at Power-up - Start Address : 2 byte
            //            // Continuous Read at Power-up - Block Size : 2 byte
            //            // Continuous Read at Power-up - Duplicate Read Delay : 1 byte
            //            // Node ID : 1 byte (only RS-485)
            //            // Reserved : 1 byte
            //            // ABx Protocol : 1 byte
            //            // Tag Type : 1 byte
            //            // Reserved : 1 byte
            //            // Communication Mode : 1 byte
            //            // Options Byte 1 : 1 byte
            //            // Reserved : 1 byte
            //            // Options Byte 2 : 1 byte
            //            // Controller Type : 1 byte
            //            // Software Major Release Digit : 1 byte
            //            // Software Minor Release Digit : 1 byte
            //            // Software Correction Release Digit : 1 byte
            //            // Software Point Release Digit : 1 byte

            //            // SetControllerConfig에서 사용하기 위해 bytesConfigInfo에 복사한다
            //            bytesConfigInfo = null;
            //            bytesConfigInfo = new byte[byteData.Length];
            //            Buffer.BlockCopy(byteData, 0, bytesConfigInfo, 0, byteData.Length);

            //            flagGetCtrlConfig = true;


            //        }
            //        break;
            //    case COMMAND.GET_CONFIG_INFO:
            //        {
            //            // 필요없음. 안해.
            //        }
            //        break;
            //    case COMMAND.ERROR:
            //        {
            //            // responseSize : 2 byte
            //            // Error Flag : 0xFF
            //            // Error Code : byte
            //            byte errorFlag = byteData[nIndex++];
            //            byte errorCode = byteData[nIndex++];
            //        }
            //        break;
            //    default:
            //        break;
            //}

            //// Checksum
            //if (flagUseChecksum)
            //{
            //    byte responseChecksum = byteData[nIndex++];

            //    byte checksum = 0x00;
            //    int startIndex = 2; // Skip header
            //    int endIndex = byteData.Length - 2; // Skip checksum & terminator(ETX : 0x03)
            //    for (int checkIdx = startIndex; checkIdx < endIndex; checkIdx++)
            //        checksum += byteData[checkIdx];
            //    checksum = (byte)(0xFF - checksum);

            //    if(checksum != responseChecksum)
            //    {
            //        // parsing 실패. 확인 필요
            //        Console.WriteLine("Response Parsing Failed. checksum diff.");
            //        return;
            //    }
            //}

            //// Command Terminator
            //byte etx = byteData[nIndex++];
            #endregion ### old code ###
        }

        /// <summary>
        /// 하나의 character를 해당 메모리에 채운다.
        /// </summary>
        /// <param name="startAddr">시작 메모리 위치</param>
        /// <param name="blockSize">채울 메모리 길이 (0이면 메모리 끝까지 채운다)</param>
        /// <param name="timeout">타임아웃 시간 (ms)</param>
        /// <returns>전송 성공 여부</returns>
        public bool FillTag(WORD startAddr = 0, WORD blockSize = 0, byte fillData = 0x41 /* 0x41 = 'A' */, WORD timeout = 2000)
        {
            C0405Packet packet = new C0405Packet(COMMAND.FILL_TAG);
            packet.startAddr = startAddr;
            packet.blockSize = blockSize;
            packet.timeout = timeout;
            packet.bytesData = new byte[1];
            packet.bytesData[0] = fillData;

            byte[] byteData = packet.Packing(flagUseChecksum);

            #region ### old code ###
            //StringBuilder msg = new StringBuilder();

            //// Header
            //msg.Append((char)0x02);
            //msg.Append((char)0x02);

            //// Command Size
            //byte[] arrByteCmdSize = BitConverter.GetBytes(8);
            //msg.Append((char)arrByteCmdSize[1]);
            //msg.Append((char)arrByteCmdSize[0]);

            //// Command ID
            //msg.Append((char)COMMAND.FILL_TAG);

            //// Start Address
            //byte[] arrByteStartAddr = BitConverter.GetBytes(startAddr);
            //msg.Append((char)arrByteStartAddr[1]);
            //msg.Append((char)arrByteStartAddr[0]);

            //// Fill Length
            //byte[] arrByteLength = BitConverter.GetBytes(blockSize);
            //msg.Append((char)arrByteLength[1]);
            //msg.Append((char)arrByteLength[0]);

            //// Timeout (2초 = 2000 = 0x07D0)
            //byte[] arrByteTimeout = BitConverter.GetBytes(timeout);
            //msg.Append((char)arrByteTimeout[1]);
            //msg.Append((char)arrByteTimeout[0]);

            //// Data Byte Value
            //msg.Append((char)fillData);

            //// Checksum
            //if (flagUseChecksum)
            //{
            //    byte checksum = 0x00;
            //    int startIndex = 2; // Skip header
            //    for (int checkIdx = startIndex; checkIdx < msg.Length; checkIdx++)
            //        checksum += (byte)msg[checkIdx];
            //    checksum = (byte)(0xFF - checksum);
            //    msg.Append((char)checksum);
            //}

            //// Terminal
            //msg.Append((char)0x03); 

            //byte[] byteData = Encoding.ASCII.GetBytes(msg.ToString());
            #endregion

            // 전송
            try
            {
                Write(byteData, 0, byteData.Length);
            }
            catch (Exception ex)
            {
                if (deleException != null)
                    deleException(ex.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// 메모리에 있는 값을 읽는다.
        /// </summary>
        /// <param name="startAddr">시작 메모리 (0~65535)</param>
        /// <param name="blockSize">읽을 메모리 길이 (0~65535)</param>
        /// <param name="timeout">타임아웃 시간(ms)</param>
        /// <returns>전송 성공 여부</returns>
        public bool ReadData(WORD startAddr = 0, WORD blockSize = 5, WORD timeout = 2000)
        {
            C0405Packet packet = new C0405Packet(COMMAND.READ_DATA);
            packet.startAddr = startAddr;
            packet.blockSize = blockSize;
            packet.timeout = timeout;

            byte[] byteData = packet.Packing(flagUseChecksum);

            #region ### old code ###
            //StringBuilder msg = new StringBuilder();

            //// Header
            //msg.Append((char)0x02);
            //msg.Append((char)0x02);

            //// Command Size
            //byte[] arrByteCmdSize = BitConverter.GetBytes(7);
            //msg.Append((char)arrByteCmdSize[1]);
            //msg.Append((char)arrByteCmdSize[0]);

            //// Command ID
            //msg.Append((char)COMMAND.READ_DATA);

            //// Start Address
            //byte[] arrByteStartAddr = BitConverter.GetBytes(startAddr);
            //msg.Append((char)arrByteStartAddr[1]);
            //msg.Append((char)arrByteStartAddr[0]);

            //// Read Length
            //byte[] arrByteLength = BitConverter.GetBytes(blockSize);
            //msg.Append((char)arrByteLength[1]);
            //msg.Append((char)arrByteLength[0]);

            //// Timeout (2초 = 2000 = 0x07D0)
            //byte[] arrByteTimeout = BitConverter.GetBytes(timeout);
            //msg.Append((char)arrByteTimeout[1]);
            //msg.Append((char)arrByteTimeout[0]);

            //// Checksum
            //if (flagUseChecksum)
            //{
            //    byte checksum = 0x00;
            //    int startIndex = 2; // Skip header
            //    for (int checkIdx = startIndex; checkIdx < msg.Length; checkIdx++)
            //        checksum += (byte)msg[checkIdx];
            //    checksum = (byte)(0xFF - checksum);
            //    msg.Append((char)checksum);
            //}

            //// Terminal
            //msg.Append((char)0x03); 

            //byte[] byteData = Encoding.ASCII.GetBytes(msg.ToString());
            #endregion

            try
            {
                Write(byteData, 0, byteData.Length);
            }
            catch (Exception ex)
            {
                if (deleException != null)
                    deleException(ex.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// 메모리에 값을 입력합니다.
        /// </summary>
        public void WriteData()
        {
            throw new NotImplementedException("WriteData 함수는 쓰이지 않을 것 같아 구현하지 않음");
        }

        /// <summary>
        /// tag ID를 읽어옵니다.
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool ReadTagID(WORD timeout = 2000)
        {
            C0405Packet packet = new C0405Packet(COMMAND.READ_TAG_ID);
            packet.timeout = timeout;

            byte[] byteData = packet.Packing(flagUseChecksum);

            #region ### old code ###
            //StringBuilder msg = new StringBuilder();

            //// Header
            //msg.Append((char)0x02);
            //msg.Append((char)0x02);

            //// Command Size
            //byte[] arrByteCmdSize = BitConverter.GetBytes(3);
            //msg.Append((char)arrByteCmdSize[1]);
            //msg.Append((char)arrByteCmdSize[0]);

            //// Command ID
            //msg.Append((char)COMMAND.READ_TAG_ID);

            //// Timeout (2초 = 2000 = 0x07D0)
            //byte[] arrByteTimeout = BitConverter.GetBytes(timeout);
            //msg.Append((char)arrByteTimeout[1]);
            //msg.Append((char)arrByteTimeout[0]);

            //// Checksum
            //if (flagUseChecksum)
            //{
            //    byte checksum = 0x00;
            //    int startIndex = 2; // Skip header
            //    for (int checkIdx = startIndex; checkIdx < msg.Length; checkIdx++)
            //        checksum += (byte)msg[checkIdx];
            //    checksum = (byte)(0xFF - checksum);
            //    msg.Append((char)checksum);
            //}

            //// Terminal
            //msg.Append((char)0x03);
            #endregion

            try
            {
                Write(byteData, 0, byteData.Length);
            }
            catch (Exception ex)
            {
                if (deleException != null)
                    deleException(ex.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// 안테나 주변에 tag가 있는지 검색합니다.
        /// </summary>
        /// <param name="timeout">타임아웃</param>
        public bool TagSearch(WORD timeout = 2000)
        {
            C0405Packet packet = new C0405Packet(COMMAND.TAG_SEARCH);
            packet.timeout = timeout;

            byte[] byteData = packet.Packing(flagUseChecksum);

            #region ### old code ###
            //StringBuilder msg = new StringBuilder();

            //// Header
            //msg.Append((char)0x02);
            //msg.Append((char)0x02);

            //// Command Size
            //byte[] arrByteCmdSize = BitConverter.GetBytes(3);
            //msg.Append((char)arrByteCmdSize[1]);
            //msg.Append((char)arrByteCmdSize[0]);

            //// Command ID
            //msg.Append((char)COMMAND.TAG_SEARCH);

            //// Timeout (2초 = 2000 = 0x07D0)
            //byte[] arrByteTimeout = BitConverter.GetBytes(timeout);
            //msg.Append((char)arrByteTimeout[1]);
            //msg.Append((char)arrByteTimeout[0]);

            //// Checksum
            //if (flagUseChecksum)
            //{
            //    byte checksum = 0x00;
            //    int startIndex = 2; // Skip header
            //    for (int checkIdx = startIndex; checkIdx < msg.Length; checkIdx++)
            //        checksum += (byte)msg[checkIdx];
            //    checksum = (byte)(0xFF - checksum);
            //    msg.Append((char)checksum);
            //}

            //// Terminal
            //msg.Append((char)0x03); 
            #endregion

            try
            {
                Write(byteData, 0, byteData.Length);
            }
            catch (Exception ex)
            {
                if (deleException != null)
                    deleException(ex.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// 상시 읽기모드를 켜고 끈다.
        /// 모드가 켜지고 범위 안에 tag가 감지되면 자동으로 읽고 값을 반환한다.
        /// Block Size를 0으로 보내면 정지된다.
        /// </summary>
        /// <param name="startAddr">시작 주소</param>
        /// <param name="blockSize">블록 사이즈(0이면 모드가 정지된다.)</param>
        /// <param name="duplicateReadDelaySec">중복 읽기 딜레이(0~60 sec)</param>
        /// <returns>전송 성공 여부</returns>
        public bool StartContiReadMode(WORD startAddr = 0, WORD blockSize = 5, byte duplicateReadDelaySec = 0x02)
        {
            C0405Packet packet = new C0405Packet(COMMAND.START_CONTI_READ);
            packet.startAddr = startAddr;
            packet.blockSize = blockSize;
            packet.dupReadDelay = duplicateReadDelaySec;

            byte[] byteData = packet.Packing(flagUseChecksum);

            #region ### old code ###
            //StringBuilder msg = new StringBuilder();

            //// Header
            //msg.Append((char)0x02);
            //msg.Append((char)0x02);

            //// Command Size
            //byte[] arrByteCmdSize = BitConverter.GetBytes(6);
            //msg.Append((char)arrByteCmdSize[1]);
            //msg.Append((char)arrByteCmdSize[0]);

            //// Command ID
            //msg.Append((char)COMMAND.START_CONTI_READ);

            //// Start Address
            //byte[] arrByteStartAddr = BitConverter.GetBytes(startAddr);
            //msg.Append((char)arrByteStartAddr[1]);
            //msg.Append((char)arrByteStartAddr[0]);

            //// Read Length
            //byte[] arrByteLength = BitConverter.GetBytes(blockSize);
            //msg.Append((char)arrByteLength[1]);
            //msg.Append((char)arrByteLength[0]);

            //// Duplicate Read Delay
            //msg.Append((char)duplicateReadDelaySec);

            //// Checksum
            //if (flagUseChecksum)
            //{
            //    byte checksum = 0x00;
            //    int startIndex = 2; // Skip header
            //    for (int checkIdx = startIndex; checkIdx < msg.Length; checkIdx++)
            //        checksum += (byte)msg[checkIdx];
            //    checksum = (byte)(0xFF - checksum);
            //    msg.Append((char)checksum);
            //}

            //// Terminal
            //msg.Append((char)0x03);

            //byte[] byteData = Encoding.ASCII.GetBytes(msg.ToString());
            #endregion

            try
            {
                Write(byteData, 0, byteData.Length);
            }
            catch (Exception ex)
            {
                if (deleException != null)
                    deleException(ex.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// 상시 읽기모드를 정지한다.
        /// </summary>
        public void StopContiReadMode()
        {
            StartContiReadMode(0, 0);
        }

        /// <summary>
        /// tag ID와 메모리를 동시에 읽는다.
        /// </summary>
        /// <returns></returns>
        public bool ReadTagID_Data()
        {
            throw new NotImplementedException("ReadTagID_Data 함수는 쓰지 않아 구현하지 않음");
        }

        /// <summary>
        /// 상시 읽기모드를 켜고 끄면서 Data를 읽는다.
        /// </summary>
        /// <returns></returns>
        public bool StartContiReadTagID_Data(WORD startAddr = 0, WORD blockSize = 5, byte duplicateReadDelaySec = 0x02, bool flagStart = true)
        {
            C0405Packet packet = new C0405Packet(COMMAND.START_CONTI_READ_DATA);
            packet.startAddr = startAddr;
            packet.blockSize = blockSize;
            packet.dupReadDelay = duplicateReadDelaySec;
            packet.flagStart = flagStart ? (byte)0x01 : (byte)0x00;

            byte[] byteData = packet.Packing(flagUseChecksum);

            try
            {
                Write(byteData, 0, byteData.Length);
            }
            catch (Exception ex)
            {
                if (deleException != null)
                    deleException(ex.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// 컨트롤러를 재부팅한다. 재부팅해도 저장된 설정 값은 초기화되지 않는다.
        /// </summary>
        /// <returns>전송 성공 여부</returns>
        public bool ResetController()
        {
            C0405Packet packet = new C0405Packet(COMMAND.TAG_SEARCH);

            byte[] byteData = packet.Packing(flagUseChecksum);

            #region ### old code ###
            //StringBuilder msg = new StringBuilder();

            //// Header
            //msg.Append((char)0x02);
            //msg.Append((char)0x02);

            //// Command Size
            //byte[] arrByteCmdSize = BitConverter.GetBytes(1);
            //msg.Append((char)arrByteCmdSize[1]);
            //msg.Append((char)arrByteCmdSize[0]);

            //// Command ID
            //msg.Append((char)COMMAND.RESET_CONTROLLER);

            //// Checksum
            //if (flagUseChecksum)
            //{
            //    byte checksum = 0x00;
            //    int startIndex = 2; // Skip header
            //    for (int checkIdx = startIndex; checkIdx < msg.Length; checkIdx++)
            //        checksum += (byte)msg[checkIdx];
            //    checksum = (byte)(0xFF - checksum);
            //    msg.Append((char)checksum);
            //}

            //// Terminal
            //msg.Append((char)0x03);

            //byte[] byteData = Encoding.ASCII.GetBytes(msg.ToString()); 
            #endregion

            try
            {
                Write(byteData, 0, byteData.Length);
            }
            catch (Exception ex)
            {
                if (deleException != null)
                    deleException(ex.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// 컨트롤러 설정을 지정한다
        /// </summary>
        /// <returns>전송 성공 여부</returns>
        public bool SetControllerConfig()
        {
            // GetControllerConfig를 먼저하지 않았을 때 false를 반환한다
            // 메뉴얼에는 GetControllerConfig를 먼저 하기를 권장
            if (flagGetCtrlConfig == false || bytesConfigInfo == null)
                return false;

            C0405Packet packet = C0405Packet.Parsing(bytesConfigInfo);
            packet.SetCommand(COMMAND.SET_CONFIG);

            // 바꿀 옵션들...무엇을 바꿀지는 가서 확인하자..

            byte[] byteData = packet.Packing(flagUseChecksum);

            try
            {
                Write(bytesConfigInfo, 0, bytesConfigInfo.Length);
            }
            catch (Exception ex)
            {
                if (deleException != null)
                    deleException(ex.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// 컨트롤러 설정을 가져온다
        /// </summary>
        /// <returns>전송 성공 여부</returns>
        public bool GetControllerConfig()
        {
            C0405Packet packet = new C0405Packet(COMMAND.TAG_SEARCH);
            packet.controllerType = (byte)CONTROLLER_TYPE.C_0405;

            byte[] byteData = packet.Packing(flagUseChecksum);

            #region ### old code ###
            //StringBuilder msg = new StringBuilder();

            //// Header
            //msg.Append((char)0x02);
            //msg.Append((char)0x02);

            //// Command Size
            //byte[] arrByteCmdSize = BitConverter.GetBytes(2);
            //msg.Append((char)arrByteCmdSize[1]);
            //msg.Append((char)arrByteCmdSize[0]);

            //// Command ID
            //msg.Append((char)COMMAND.GET_CONFIG);

            //// Controller Type
            //msg.Append((char)CONTROLLER_TYPE.C_0405);

            //// Checksum
            //if (flagUseChecksum)
            //{
            //    byte checksum = 0x00;
            //    int startIndex = 2; // Skip header
            //    for (int checkIdx = startIndex; checkIdx < msg.Length; checkIdx++)
            //        checksum += (byte)msg[checkIdx];
            //    checksum = (byte)(0xFF - checksum);
            //    msg.Append((char)checksum);
            //}

            //// Terminal
            //msg.Append((char)0x03);

            //byte[] byteData = Encoding.ASCII.GetBytes(msg.ToString()); 
            #endregion

            try
            {
                Write(byteData, 0, byteData.Length);
            }
            catch (Exception ex)
            {
                if (deleException != null)
                    deleException(ex.ToString());
                return false;
            }

            return true;
        }
    }
}
