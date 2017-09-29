using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommTestFIS
{
    class PathManager
    {
        private static PathManager instance = null;
        private static readonly object padlock = new object();

        private string pathExeFile = string.Empty;

        private string pathSetting = "Setting";

        private string filenamenSettingSocketFIS = "setting_socket_fis.xml";
        private string filenamenSettingSocketMES = "setting_socket_mes.xml";
        private string filenameSettingC0405 = "C0405.xml";

        PathManager()
        {
        }

        public static PathManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new PathManager();
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// FIS와 통신 시 통신 설정 파일 경로
        /// </summary>
        public string PATH_SETTING_SOCKET_FIS
        {
            get { return string.Format("{0}\\{1}", GetSettingPath(), filenamenSettingSocketFIS); }
        }

        /// <summary>
        /// MES와 통신 시 통신 설정 파일 경로
        /// </summary>
        public string PATH_SETTING_SOCKET_MES
        {
            get { return string.Format("{0}\\{1}", GetSettingPath(), filenamenSettingSocketMES); }
        }

        /// <summary>
        /// C0405의 Serial 통신 설정 파일 경로
        /// </summary>
        public string PATH_SETTING_C0405
        {
            get { return string.Format("{0}\\{1}", GetSettingPath(), filenameSettingC0405); }
        }

        /// <summary>
        /// 실행 파일 위치 반환 (파일 이름 제외)
        /// </summary>
        /// <returns>실행 파일 위치</returns>
        private string GetExePath()
        {
            if (pathExeFile != string.Empty)
                return pathExeFile;

            pathExeFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            return pathExeFile;
        }

        private string GetSettingPath()
        {
            //string path = string.Format("{0}\\{1}\\", GetExePath(), pathSetting);
            string path = string.Format("{0}\\", GetExePath());

            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);

            return path;
        }
    }
}
