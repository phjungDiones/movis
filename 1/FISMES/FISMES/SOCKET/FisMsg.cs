using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FISMES.SOCKET
{
    public class FisMsg
    {
        protected string commandID = "";
        protected string result = "";
        protected string value = "";
        protected string reserved1 = "";
        protected string reserved2 = "";
        protected string reserved3 = "";
        protected string reserved4 = "";

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
            get { return this.packet; }
        }

        public string COMMAND_ID
        {
            get { return this.commandID; }
        }

        public string RESULT
        {
            get { return this.result; }
        }

        public string VALUE
        {
            get { return this.value; }
        }

        public void SetValue(string commandID, string result, string value)
        {
            this.commandID = commandID;
            this.result = result;
            this.value = value;
        }

        public void Packing()
        {
            StringBuilder pack = new StringBuilder();
            pack.Append((char)0x02);
            pack.Append(commandID);
            pack.Append("/");
            pack.Append(result);
            pack.Append("/");
            pack.Append(value);
            pack.Append("/");
            pack.Append(reserved1);
            pack.Append("/");
            pack.Append(reserved2);
            pack.Append("/");
            pack.Append(reserved3);
            pack.Append("/");
            pack.Append(reserved4);
            pack.Append((char)0x03);

            packet = pack.ToString();
            pack.Clear();

            valueChanged = false;
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

            commandID = arr[0];
            result = arr[1];
            value = arr[2];
            reserved1 = arr[3];
            reserved2 = arr[4];
            reserved3 = arr[5];
            reserved4 = arr[6];

            // 분할 개수가 7인지 확인
            if (arr.Length != 7)
                return false;

            return true;
        }
    }
}
