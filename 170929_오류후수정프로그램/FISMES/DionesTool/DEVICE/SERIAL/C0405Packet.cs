using DionesTool.UTIL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DWORD = System.UInt32;
using WORD = System.UInt16;

namespace DionesTool.DEVICE.SERIAL.C0405
{
    public class C0405Packet
    {
        public const WORD stx = 0x0202;
        public const byte etx = 0x03;

        public WORD header = 0x0202;
        public byte tail = 0x03;

        public WORD commandSize = 0x0000;
        public byte commandID = 0x03;

        public WORD timeout = 0x07D0;

        public byte checksum = 0x00;

        public WORD startAddr = 0x0000;
        public WORD blockSize = 0x0000;
        public byte dupReadDelay = 0x00;
        public byte flagStart = 0x01;
        public byte nodeID = 0x00;
        public byte protocolChksum = 0x00;
        public byte tagType = 0x00;
        public byte commMode = 0x00;
        public byte optionsByte1 = 0x00;
        public byte optionsByte2 = 0x00;
        public byte controllerType = 0x03;
        public DWORD version = 0x00000000;

        public byte errorCode = 0x00;

        public byte[] tagID = null;
        public byte[] bytesData = null;

        public C0405Packet()
        {
        }

        public C0405Packet(C0405.COMMAND cmd)
        {
            commandID = (byte)cmd;
        }

        public void SetCommand(C0405.COMMAND cmd)
        {
            commandID = (byte)cmd;
        }

        public override string ToString()
        {
            return string.Format("command : {0}, protocolChksum : {1}", commandID, protocolChksum);
        }

        public byte[] Packing(bool flagUseChecksum = false)
        {
            // BinaryWriter는 little endian 방식이므로 short 이상부터는 big endian으로 바꾸어 입력한다.
            byte[] bytes;
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(stx);

                    switch ((C0405.COMMAND)commandID)
                    {
                        case C0405.COMMAND.FILL_TAG:
                            {
                                #region ### FILL TAG ###
                                if (bytesData == null)
                                    return null;

                                // Command Size
                                commandSize = 0x0008;

                                bytes = BitConverter.GetBytes(commandSize);
                                Array.Reverse(bytes);
                                writer.Write(bytes);

                                // Command ID
                                writer.Write(commandID);

                                // Start Addr
                                bytes = BitConverter.GetBytes(startAddr);
                                Array.Reverse(bytes);
                                writer.Write(bytes);

                                // Block Size
                                bytes = BitConverter.GetBytes(blockSize);
                                Array.Reverse(bytes);
                                writer.Write(bytes);

                                // Timeout (2초 = 2000 = 0x07D0)
                                bytes = BitConverter.GetBytes(timeout);
                                Array.Reverse(bytes);
                                writer.Write(bytes);

                                // Data Byte Value
                                writer.Write(bytesData);
                                #endregion ### FILL TAG ###
                            }
                            break;
                        case C0405.COMMAND.READ_DATA:
                            {
                                #region ### READ DATA ###
                                // Command Size
                                commandSize = 0x0007;

                                bytes = BitConverter.GetBytes(commandSize);
                                Array.Reverse(bytes);
                                writer.Write(bytes);

                                // Command ID
                                writer.Write(commandID);

                                // Start Addr
                                bytes = BitConverter.GetBytes(startAddr);
                                Array.Reverse(bytes);
                                writer.Write(bytes);

                                // Block Size
                                bytes = BitConverter.GetBytes(blockSize);
                                Array.Reverse(bytes);
                                writer.Write(bytes);

                                // Timeout (2초 = 2000 = 0x07D0)
                                bytes = BitConverter.GetBytes(timeout);
                                Array.Reverse(bytes);
                                writer.Write(bytes);
                                #endregion ### READ DATA ###
                            }
                            break;
                        case C0405.COMMAND.WRITE_DATA:
                            {
                                #region ### WRITE DATA ###
                                if (bytesData == null)
                                    return null;

                                // Command Size
                                commandSize = (ushort)(0x0007 + bytesData.Length);

                                bytes = BitConverter.GetBytes(commandSize);
                                Array.Reverse(bytes);
                                writer.Write(bytes);

                                // Command ID
                                writer.Write(commandID);

                                // Start Addr
                                bytes = BitConverter.GetBytes(startAddr);
                                Array.Reverse(bytes);
                                writer.Write(bytes);

                                // Block Size
                                bytes = BitConverter.GetBytes(blockSize);
                                Array.Reverse(bytes);
                                writer.Write(bytes);

                                // Timeout (2초 = 2000 = 0x07D0)
                                bytes = BitConverter.GetBytes(timeout);
                                Array.Reverse(bytes);
                                writer.Write(bytes);

                                // Data Byte Value
                                bytes = new byte[bytesData.Length];
                                Buffer.BlockCopy(bytesData, 0, bytes, 0, bytesData.Length);
                                Array.Reverse(bytes);
                                writer.Write(bytes);
                                #endregion ### WRITE DATA ###
                            }
                            break;
                        case C0405.COMMAND.READ_TAG_ID:
                            {
                                #region ### READ TAG ID ###
                                // Command Size
                                commandSize = 0x0003;

                                bytes = BitConverter.GetBytes(commandSize);
                                Array.Reverse(bytes);
                                writer.Write(bytes);

                                // Command ID
                                writer.Write(commandID);

                                // Timeout (2초 = 2000 = 0x07D0)
                                bytes = BitConverter.GetBytes(timeout);
                                Array.Reverse(bytes);
                                writer.Write(bytes);
                                #endregion ### READ TAG ID ###
                            }
                            break;
                        case C0405.COMMAND.TAG_SEARCH:
                            {
                                #region ### TAG SEARCH ###
                                // Command Size
                                commandSize = 0x0003;

                                bytes = BitConverter.GetBytes(commandSize);
                                Array.Reverse(bytes);
                                writer.Write(bytes);

                                // Command ID
                                writer.Write(commandID);

                                // Timeout (2초 = 2000 = 0x07D0)
                                bytes = BitConverter.GetBytes(timeout);
                                Array.Reverse(bytes);
                                writer.Write(bytes);
                                #endregion ### TAG SEARCH ###
                            }
                            break;
                        case C0405.COMMAND.START_CONTI_READ:
                            {
                                #region ### START CONTI READ ###
                                // Command Size
                                commandSize = 0x0006;

                                bytes = BitConverter.GetBytes(commandSize);
                                Array.Reverse(bytes);
                                writer.Write(bytes);

                                // Command ID
                                writer.Write(commandID);

                                // Start Addr
                                bytes = BitConverter.GetBytes(startAddr);
                                Array.Reverse(bytes);
                                writer.Write(bytes);

                                // Block Size
                                bytes = BitConverter.GetBytes(blockSize);
                                Array.Reverse(bytes);
                                writer.Write(bytes);

                                // Duplicate Read Delay (0x02 = 2 x 1 second)
                                writer.Write(dupReadDelay);
                                #endregion ### START CONTI READ ###
                            }
                            break;
                        case C0405.COMMAND.START_CONTI_READ_DATA:
                            #region ### START CONTI READ ###
                            // Command Size
                            commandSize = 0x0007;

                            bytes = BitConverter.GetBytes(commandSize);
                            Array.Reverse(bytes);
                            writer.Write(bytes);

                            // Command ID
                            writer.Write(commandID);

                            // Start Addr
                            bytes = BitConverter.GetBytes(startAddr);
                            Array.Reverse(bytes);
                            writer.Write(bytes);

                            // Block Size
                            bytes = BitConverter.GetBytes(blockSize);
                            Array.Reverse(bytes);
                            writer.Write(bytes);

                            // Duplicate Read Delay (0x02 = 2 x 1 second)
                            writer.Write(dupReadDelay);

                            // Start Flag (0x01 : Start, 0x00 : Stop)
                            writer.Write(flagStart);
                            #endregion ### START CONTI READ ###
                            break;
                        case C0405.COMMAND.RESET_CONTROLLER:
                            {
                                #region ### RESET CONTROLLER ###
                                // Command Size
                                commandSize = 0x0001;

                                bytes = BitConverter.GetBytes(commandSize);
                                Array.Reverse(bytes);
                                writer.Write(bytes);

                                // Command ID
                                writer.Write(commandID);
                                #endregion ### RESET CONTROLLER ###
                            }
                            break;
                        case C0405.COMMAND.SET_CONFIG:
                            {
                                #region ### SET CONFIG ###
                                // Command Size
                                commandSize = 0x0014;

                                bytes = BitConverter.GetBytes(commandSize);
                                Array.Reverse(bytes);
                                writer.Write(bytes);

                                // Command ID
                                writer.Write(commandID);

                                // Start Addr
                                bytes = BitConverter.GetBytes(startAddr);
                                Array.Reverse(bytes);
                                writer.Write(bytes);

                                // Block Size
                                bytes = BitConverter.GetBytes(blockSize);
                                Array.Reverse(bytes);
                                writer.Write(bytes);

                                // Duplicate Read Delay (0x02 = 2 x 1 second)
                                writer.Write(dupReadDelay);

                                // Node ID
                                writer.Write(nodeID);

                                // Reserved
                                writer.Write((byte)0x00);

                                // protocol Checksum (0x00 = without checksum, 0x01 = with checksum)
                                writer.Write(protocolChksum);

                                // Tag Type
                                writer.Write(tagType);

                                // Reserved
                                writer.Write((byte)0x00);

                                // Communication Mode
                                writer.Write(commMode);

                                // Options Byte 1
                                writer.Write(optionsByte1);

                                // Reserved
                                writer.Write((byte)0x00);

                                // Options Byte 2
                                writer.Write(optionsByte2);

                                // Controller Type (0x03 = BIS M-410, C0405)
                                writer.Write(controllerType);

                                // Software Version
                                writer.Write(version);
                                #endregion ### SET CONFIG ###
                            }
                            break;
                        case C0405.COMMAND.GET_CONFIG:
                            {
                                #region ### GET CONFIG ###
                                // Command Size
                                commandSize = 0x0002;

                                bytes = BitConverter.GetBytes(commandSize);
                                Array.Reverse(bytes);
                                writer.Write(bytes);

                                // Command ID
                                writer.Write(commandID);

                                // Controller Type (0x03 = BIS M-410, C0405)
                                writer.Write(controllerType);
                                #endregion ### GET CONFIG ###
                            }
                            break;
                        case C0405.COMMAND.GET_CONFIG_INFO:
                            break;
                        case C0405.COMMAND.ERROR:
                            break;
                        default:
                            break;
                    }

                    if (flagUseChecksum)
                        writer.Write(checksum);

                    writer.Write(etx);
                }

                // 실 byte 데이터
                byte[] data = m.ToArray();

                // 체크섬 사용 유무. 사용 시 Header, Checksum, Etx 제외한 byte 더하고 그 값을 0xFF에서 뺀다.
                if (flagUseChecksum)
                {
                    checksum = 0x00;

                    // skip header & checksum, etx
                    for (int checkIdx = 2; checkIdx < data.Length-2; checkIdx++)
                        checksum += data[checkIdx];

                    // subtract from 0xFF
                    checksum = (byte)(0xFF - checksum);

                    // replace checksum
                    data[data.Length - 2] = checksum;
                }

                return data;
            }
        }

        public static C0405Packet Parsing(byte[] data, bool flagUseChecksum = false)
        {
            C0405Packet result = new C0405Packet();
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    // Header
                    result.header = reader.ReadUInt16();
                    //CUtil.ConvertEndian(ref result.header);
                    if (result.header != 0x0202)
                    {
                        using (System.IO.StreamWriter writer = new System.IO.StreamWriter("error.txt", true))
                        {
                            writer.WriteLine("header is null");
                            writer.Close();
                        }
                        return null;
                    }

                    // Command Size
                    result.commandSize = reader.ReadUInt16();
                    Util.ConvertEndian(ref result.commandSize);

                    // responseSize와 byte 개수를 비교 확인
                    // checkLength : responseSize + header + size + etx + checksum
                    int checkLength = result.commandSize + 2 + 2 + 1;
                    if (flagUseChecksum) checkLength++;
                    if (checkLength != data.Length)
                    {
                        // parsing 실패. 확인 필요
                        Console.WriteLine("Response Parsing Failed. byte length diff.");
                        using (System.IO.StreamWriter writer = new System.IO.StreamWriter("error.txt", true))
                        {
                            writer.WriteLine(string.Format("Response Parsing Failed. byte length diff. {0} = {1} --- {2}", checkLength, data.Length, result.commandSize));
                            writer.Close();
                        }
                        return null;
                    }

                    // Command ID
                    result.commandID = reader.ReadByte();

                    switch ((C0405.COMMAND)result.commandID)
                    {
                        case C0405.COMMAND.FILL_TAG:
                            {
                                // responseSize : 1 byte
                                // Command Echo : 1 byte
                            }
                            break;
                        case C0405.COMMAND.READ_DATA:
                            {
                                // responseSize : variable byte
                                // Command Echo : 1 byte
                                // data : responseSize-1까지 (Command Echo 제외함)
                                result.bytesData = reader.ReadBytes(result.commandSize - 1);
                            }
                            break;
                        case C0405.COMMAND.WRITE_DATA:
                            {
                                // responseSize : 1 byte
                                // Command Echo : 1 byte
                            }
                            break;
                        case C0405.COMMAND.READ_TAG_ID:
                            {
                                // responseSize : 9 byte
                                // Command Echo : 1 byte
                                // Tag ID : 8 byte
                                result.tagID = reader.ReadBytes(8/*result.commandSize - 1*/);
                            }
                            break;
                        case C0405.COMMAND.TAG_SEARCH:
                            {
                                // responseSize : 1 byte
                                // Command Echo : 1 byte
                            }
                            break;
                        case C0405.COMMAND.START_CONTI_READ:
                            {
                                // responseSize : 5 byte
                                // Command Echo : 1 byte
                                // Data : 4 byte

                                // responseSize : 1 byte
                                // Command Echo : 1 byte

                                // Continuous Read Stop이라면 길이가 1이고, Start라면 길이가 1보다 크다
                                if(result.commandSize > 0x01)
                                {
                                    result.bytesData = reader.ReadBytes(result.commandSize - 1);
                                }
                                else
                                {
                                    // 중지 이벤트 완료 메시지 전송
                                    result.bytesData = null;
                                }
                            }
                            break;
                        case C0405.COMMAND.START_CONTI_READ_DATA:
                            {
                                // responseSize : variable byte (1 + 8 + blockSize byte)
                                // Command Echo : 1 byte
                                // Tag ID : 8 byte
                                // Data : responseSize-1-8까지 (Command Echo, tag ID 제외함)

                                if (result.commandSize > 0x07)
                                {
                                    // result.tagID
                                    result.tagID = reader.ReadBytes(8);

                                    // result.bytesData
                                    result.bytesData = reader.ReadBytes(result.commandSize - 1 - 8);
                                }
                                else
                                {
                                    // 중지 이벤트 완료 메시지 전송
                                    result.bytesData = null;
                                }
                            }
                            break;
                        case C0405.COMMAND.RESET_CONTROLLER:
                            {
                                // responseSize : 1 byte
                                // Command Echo : 1 byte
                            }
                            break;
                        case C0405.COMMAND.SET_CONFIG:
                            {
                                // responseSize : 1 byte
                                // Command Echo : 1 byte
                            }
                            break;
                        case C0405.COMMAND.GET_CONFIG:
                            {
                                // responseSize : 14 byte
                                // Command Echo : 1 byte
                                // Continuous Read at Power-up - Start Address : 2 byte
                                // Continuous Read at Power-up - Block Size : 2 byte
                                // Continuous Read at Power-up - Duplicate Read Delay : 1 byte
                                // Node ID : 1 byte (only RS-485)
                                // Reserved : 1 byte
                                // ABx Protocol : 1 byte
                                // Tag Type : 1 byte
                                // Reserved : 1 byte
                                // Communication Mode : 1 byte
                                // Options Byte 1 : 1 byte
                                // Reserved : 1 byte
                                // Options Byte 2 : 1 byte
                                // Controller Type : 1 byte
                                // Software Major Release Digit : 1 byte
                                // Software Minor Release Digit : 1 byte
                                // Software Correction Release Digit : 1 byte
                                // Software Point Release Digit : 1 byte

                                result.startAddr = reader.ReadUInt16();
                                result.blockSize = reader.ReadUInt16();
                                result.dupReadDelay = reader.ReadByte();
                                result.nodeID = reader.ReadByte();
                                reader.ReadByte();
                                result.protocolChksum = reader.ReadByte();
                                result.tagType = reader.ReadByte();
                                reader.ReadByte();
                                result.commMode = reader.ReadByte();
                                result.optionsByte1 = reader.ReadByte();
                                reader.ReadByte();
                                result.optionsByte2 = reader.ReadByte();
                                result.controllerType = reader.ReadByte();
                                result.version = reader.ReadUInt32();

                                // Convert Big Endian -> Little Endian
                                Util.ConvertEndian(ref result.startAddr);
                                Util.ConvertEndian(ref result.blockSize);
                            }
                            break;
                        case C0405.COMMAND.GET_CONFIG_INFO:
                            {
                                // 필요 없음
                            }
                            break;
                        case C0405.COMMAND.ERROR:
                            {
                                // responseSize : 2 byte
                                // Error Code : byte
                                result.errorCode = reader.ReadByte();
                            }
                            break;
                        default:
                            break;
                    }

                    // 체크섬 사용 유무. 사용 시 Header, Checksum, Etx 제외한 byte 더하고 그 값을 0xFF에서 뺀다.
                    if (flagUseChecksum)
                    {
                        result.checksum = reader.ReadByte();

                        byte checksum = 0x00;
                        // skip header & checksum, etx
                        for (int checkIdx = 2; checkIdx < data.Length - 2; checkIdx++)
                            checksum += data[checkIdx];

                        // subtract from 0xFF
                        checksum = (byte)(0xFF - checksum);

                        if (result.checksum != checksum)
                        {
                            // parsing 실패. 확인 필요
                            Console.WriteLine("Response Parsing Failed. checksum diff.");
                            using (System.IO.StreamWriter writer = new System.IO.StreamWriter("error.txt"))
                            {
                                writer.WriteLine("Response Parsing Failed. checksum diff.");
                                writer.Close();
                            }
                            return null;
                        }
                    }
                    
                    // ETX
                    result.tail = reader.ReadByte();
                }
            }
            return result;
        }
    }
}
