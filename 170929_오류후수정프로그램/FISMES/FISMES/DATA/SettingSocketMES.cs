using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FISMES.DATA
{
    public class ValueSocketMES
    {
        public string ipaddr = "10.240.183.134";
        public int port = 8080;
        public int maxClientCount = 1;
        public int timeoutSecT3 = 10;

        public string msgID = "84010";
        public string factoryID = "1000";
        public string lineID = "PM03";
        public string operID = "070";
        public string eqpPosID = "1";

        public ValueSocketMES() { }

        public void Init()
        {
            ipaddr = "127.0.0.1";
            port = 8080;
            maxClientCount = 1;
            timeoutSecT3 = 10;

            msgID = "84010";
            factoryID = "1000";
            lineID = "PM03";
            operID = "070";
            eqpPosID = "1";
        }
    }

    [Serializable]
    public sealed class SettingSocketMES
    {
        private static volatile SettingSocketMES instance;
        private static object syncRoot = new Object();

        ValueSocketMES dataValue = new ValueSocketMES();

        public string IPADDR
        {
            get { return dataValue.ipaddr; }
            set { dataValue.ipaddr = value; }
        }

        public int PORT
        {
            get { return dataValue.port; }
            set { dataValue.port = value; }
        }

        public int MAX_CLIENT_COUNT
        {
            get { return dataValue.maxClientCount; }
            set { dataValue.maxClientCount = value; }
        }

        public int TIMEOUT_SEC_T3
        {
            get { return dataValue.timeoutSecT3; }
            set { dataValue.timeoutSecT3 = value; }
        }

        public string MSG_ID
        {
            get { return dataValue.msgID; }
            set { dataValue.msgID = value; }
        }

        public string FACTORY_ID
        {
            get { return dataValue.factoryID; }
            set { dataValue.factoryID = value; }
        }

        public string LINE_ID
        {
            get { return dataValue.lineID; }
            set { dataValue.lineID = value; }
        }

        public string OPER_ID
        {
            get { return dataValue.operID; }
            set { dataValue.operID = value; }
        }

        public string EQP_POS_ID
        {
            get { return dataValue.eqpPosID; }
            set { dataValue.eqpPosID = value; }
        }

        private SettingSocketMES()
        {
            //
        }

        public static SettingSocketMES Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new SettingSocketMES();
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        public void Init()
        {
            dataValue.Init();
        }

        public override string ToString()
        {
            return string.Format("msgID : {0}, factoryID : {1}, lineID : {2}, operID : {3}, eqpPosID : {4}",
                dataValue.msgID, dataValue.factoryID, dataValue.lineID, dataValue.operID, dataValue.eqpPosID);
        }

        /// <summary>
        /// 파일에 내용을 쓴다
        /// </summary>
        /// <param name="path">파일 위치</param>
        public void Serialize(string path)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ValueSocketMES));
            using (StreamWriter wr = new StreamWriter(path))
            {
                xmlSerializer.Serialize(wr, dataValue);
            }
        }

        /// <summary>
        /// 파일에서 내용을 읽어온다
        /// </summary>
        /// <param name="path">파일 위치</param>
        /// <returns>파일의 존재 유무</returns>
        public bool Deserialize(string path)
        {
            if (!File.Exists(path))
            {
                Init();
                return false;
            }

            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ValueSocketMES));
                using (StreamReader rd = new StreamReader(path))
                {
                    dataValue = (ValueSocketMES)xmlSerializer.Deserialize(rd);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Init();
                return false;
            }

            return true;
        }

    }
}
