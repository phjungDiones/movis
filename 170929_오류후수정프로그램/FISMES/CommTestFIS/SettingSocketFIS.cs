using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CommTestFIS
{
    public class ValueSocketFIS
    {
        public string ipaddr = "10.240.183.134";
        public int port = 8080;
        public int maxClientCount = 1;

        public ValueSocketFIS() { }

        public void Init()
        {
            ipaddr = "127.0.0.1";
            port = 8080;
            maxClientCount = 1;
        }
    }

    [Serializable]
    public sealed class SettingSocketFIS
    {
        private static volatile SettingSocketFIS instance;
        private static object syncRoot = new Object();

        ValueSocketFIS dataValue = new ValueSocketFIS();

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

        private SettingSocketFIS()
        {
            //
        }

        public static SettingSocketFIS Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new SettingSocketFIS();
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

        /// <summary>
        /// 파일에 내용을 쓴다
        /// </summary>
        /// <param name="path">파일 위치</param>
        public void Serialize(string path)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ValueSocketFIS));
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
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ValueSocketFIS));
                using (StreamReader rd = new StreamReader(path))
                {
                    dataValue = (ValueSocketFIS)xmlSerializer.Deserialize(rd);
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
