using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using DionesTool.UTIL;

namespace FISMES
{
    public enum LOG_TYPE : int
    {
        EXCEPTION = 0,
        ERROR,
        COMM_FIS,
        COMM_MES,
        COMM_RFID,
        SEQUENCE,
        EVENT,
        MAX,
    }

    public enum LOG_LEVEL
    {
        DEBUG,
        INFO,
        WARN,
        ERROR,
        FATAL,
    }

    class LogManager
    {
        private static LogManager instance = null;
        private static readonly object padlock = new object();

        private delegate void DeleWrite(string msg);
        private delegate void DeleWriteArgs(string format, params object[] args);

        ILog[] iLog = new ILog[(int)LOG_TYPE.MAX];

        public ILog this[LOG_TYPE lType]
        {
            get { return iLog[(int)lType]; }
            set { iLog[(int)lType] = value; }
        }

        public static LogManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new LogManager();

                            instance.Init();
                        }
                    }
                }
                return instance;
            }
        }

        void Init()
        {
            string appPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string setupPath = string.Format("{0}//Log//{1}", appPath, "LogConfig.xml");
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(setupPath));

            instance[LOG_TYPE.EXCEPTION] = log4net.LogManager.GetLogger("LoggerEx");
            instance[LOG_TYPE.ERROR] = log4net.LogManager.GetLogger("LoggerError");
            instance[LOG_TYPE.COMM_FIS] = log4net.LogManager.GetLogger("LoggerFIS");
            instance[LOG_TYPE.COMM_MES] = log4net.LogManager.GetLogger("LoggerMES");
            instance[LOG_TYPE.COMM_RFID] = log4net.LogManager.GetLogger("LoggerRFID");
            instance[LOG_TYPE.SEQUENCE] = log4net.LogManager.GetLogger("LoggerSeq");
            instance[LOG_TYPE.EVENT] = log4net.LogManager.GetLogger("LoggerEvent");

            DeleteOldFile();
        }

        public void DeleteOldFile()
        {
            log4net.Repository.ILoggerRepository repository = log4net.LogManager.GetRepository();
            //get all of the appenders for the repository 
            log4net.Appender.IAppender[] appenders = repository.GetAppenders();
            //only change the file path on the 'FileAppenders' 
            foreach (log4net.Appender.IAppender appender in (from iAppender in appenders
                                                             where iAppender is log4net.Appender.FileAppender
                                                             select iAppender))
            {
                log4net.Appender.RollingFileAppender fileAppender = appender as log4net.Appender.RollingFileAppender;
                Util.DeleteOldFile(System.IO.Path.GetDirectoryName(fileAppender.File), 60);
            }
        }

        public void WriteI(LOG_TYPE lType, string msg)
        {
            instance[lType].Info(msg);
        }

        public void WriteI(LOG_TYPE lType, string format, params object[] args)
        {
            instance[lType].InfoFormat(format, args);
        }

        public void WriteD(LOG_TYPE lType, string msg)
        {
            instance[lType].Debug(msg);
        }

        public void WriteD(LOG_TYPE lType, string format, params object[] args)
        {
            instance[lType].DebugFormat(format, args);
        }

        public void Write(LOG_TYPE lType, LOG_LEVEL lLevel, string msg)
        {
            DeleWrite deleWrite = null;
            switch (lLevel)
            {
                case LOG_LEVEL.DEBUG:
                    deleWrite = instance[lType].Debug;
                    break;
                case LOG_LEVEL.INFO:
                    deleWrite = instance[lType].Info;
                    break;
                case LOG_LEVEL.WARN:
                    deleWrite = instance[lType].Warn;
                    break;
                case LOG_LEVEL.ERROR:
                    deleWrite = instance[lType].Error;
                    break;
                case LOG_LEVEL.FATAL:
                    deleWrite = instance[lType].Fatal;
                    break;
                default:
                    break;
            }

            if (deleWrite != null)
                deleWrite(msg);
        }

        public void Write(LOG_TYPE lType, LOG_LEVEL lLevel, string format, params object[] args)
        {
            DeleWriteArgs deleWrite = null;
            switch (lLevel)
            {
                case LOG_LEVEL.DEBUG:
                    deleWrite = instance[lType].DebugFormat;
                    break;
                case LOG_LEVEL.INFO:
                    deleWrite = instance[lType].InfoFormat;
                    break;
                case LOG_LEVEL.WARN:
                    deleWrite = instance[lType].WarnFormat;
                    break;
                case LOG_LEVEL.ERROR:
                    deleWrite = instance[lType].ErrorFormat;
                    break;
                case LOG_LEVEL.FATAL:
                    deleWrite = instance[lType].FatalFormat;
                    break;
                default:
                    break;
            }

            if (deleWrite != null)
                deleWrite(format, args);
        }
    }
}
