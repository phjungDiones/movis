using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FISMES.DATA
{
    [Serializable]
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
    public class SettingSocketFIS
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

        public override string ToString()
        {
            return string.Format("ipAddr : {0}, port : {1}, maxClientCount : {2}",
                dataValue.ipaddr, dataValue.port, dataValue.maxClientCount);
        }

        /// <summary>
        /// 초기화
        /// </summary>
        public void Init()
        {
            dataValue.Init();
        }

        public void Serialize(string path)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ValueSocketFIS));
                using (StreamWriter wr = new StreamWriter(path))
                {
                    xmlSerializer.Serialize(wr, dataValue);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

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
