using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FISMES
{
    class ExceptionManager : IDisposable
    {
        private static ExceptionManager instance = null;
        private static readonly object padlock = new object();

        bool isInit = false;

        public static ExceptionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new ExceptionManager();

                            instance.Init();
                        }
                    }
                }
                return instance;
            }
        }

        public void Init()
        {
            if (isInit)
                return;

            isInit = true;

            //GLOBAL.GlobalVariable.DEV_C0405.deleException += deleException;
            GLOBAL.GlobalVariable.SOCKET_FIS.deleException += deleException;
            GLOBAL.GlobalVariable.SOCKET_MES.deleException += deleException;
        }

        void deleException(string strErr)
        {
            LogManager.Instance.WriteI(LOG_TYPE.EXCEPTION, strErr);
        }

        public void Dispose()
        {
            //GLOBAL.GlobalVariable.DEV_C0405.deleException -= deleException;
            GLOBAL.GlobalVariable.SOCKET_FIS.deleException -= deleException;
            GLOBAL.GlobalVariable.SOCKET_MES.deleException -= deleException;
        }
    }
}
