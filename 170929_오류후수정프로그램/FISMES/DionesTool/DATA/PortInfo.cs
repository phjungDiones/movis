using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;
using System.Xml.Serialization;

namespace DionesTool.DATA
{
    /// <summary>
    /// 포트 정보를 파일에 xml형태로 저장하고 불러오는 클래스입니다.
    /// </summary>
    [Serializable]
    public class PortInfo
    {
        public delegate void DeleException(string strErr);
        public event DeleException deleException = null;

        public string PortName = "COM1";
        public int BaudRate = 9600;
        public int DataBits = 8;
        public Parity Parity = Parity.None;
        public StopBits StopBits = StopBits.One;

        /// <summary>
        /// 초기화 값입니다.
        /// </summary>
        public void Init()
        {
            PortName = "COM1";
            BaudRate = 9600;
            DataBits = 8;
            Parity = Parity.None;
            StopBits = StopBits.One;
        }

        /// <summary>
        /// 저장할 포트 정보를 파일에 입력합니다.
        /// </summary>
        /// <param name="serial"></param>
        public bool SetInfo(SerialPort serial, string path)
        {
            PortName = serial.PortName;
            BaudRate = serial.BaudRate;
            DataBits = serial.DataBits;
            Parity = serial.Parity;
            StopBits = serial.StopBits;

            return Serialize(path);
        }

        /// <summary>
        /// SerialPort에 현재 정보를 입력합니다.
        /// Deserialize 후에 호출합니다.
        /// </summary>
        /// <param name="serial"></param>
        public bool GetInfo(ref SerialPort serial, string path)
        {
            PortInfo portInfo = null;
            portInfo = Deserialize(path);

            if (portInfo == null)
                return false;

            serial.PortName = PortName = portInfo.PortName;
            serial.BaudRate = BaudRate = portInfo.BaudRate;
            serial.DataBits = DataBits = portInfo.DataBits;
            serial.Parity = portInfo.Parity;
            serial.StopBits = portInfo.StopBits;

            return true;
        }

        /// <summary>
        /// 파일에 xml형태로 정보를 저장합니다.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>성공 여부</returns>
        bool Serialize(string path)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(PortInfo));
                using (StreamWriter wr = new StreamWriter(path))
                {
                    xmlSerializer.Serialize(wr, this);
                }
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
        /// xml형태의 파일에서 정보를 불러옵니다.
        /// </summary>
        /// <param name="path">파일 경로</param>
        /// <returns>반환 객체</returns>
        PortInfo Deserialize(string path)
        {
            PortInfo portInfo = null;

            if (!File.Exists(path))
            {
                return null;
            }

            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(PortInfo));
                using (StreamReader rd = new StreamReader(path))
                {
                    portInfo = (PortInfo)xmlSerializer.Deserialize(rd);
                }
            }
            catch (Exception ex)
            {
                if (deleException != null)
                    deleException(ex.ToString());
            }

            return portInfo;
        }
    }
}
