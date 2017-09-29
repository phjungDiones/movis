using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DionesTool.UTIL
{
    /// <summary>
    /// 유틸 관련 함수
    /// </summary>
    public class Util
    {
        /// <summary>
        /// 경로에서 파일 이름만 추출한다
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileNameInPath(string path)
        {
            return System.IO.Path.GetFileName(path);
        }

        /// <summary>
        /// string을 byte array로 변경 (ASCII)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] TranslateStringToByteArrayAscii(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        /// <summary>
        /// byte array를 string으로 변경 (ASCII)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string TranslateByteArrayToStringAscii(byte[] data)
        {
            return Encoding.ASCII.GetString(data);
        }

        /// <summary>
        /// 이미 실행 중인 프로그램이 있는지 확인
        /// </summary>
        /// <returns></returns>
        public static bool IsAppAlreadyRunning()
        {
            System.Diagnostics.Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();

            int ID = currentProcess.Id;
            string Name = currentProcess.ProcessName;

            bool isAlreadyRunnig = false;

            System.Diagnostics.Process[] processess = System.Diagnostics.Process.GetProcesses();

            foreach (System.Diagnostics.Process process in processess)
            {
                if (ID != process.Id)
                {
                    if (Name == process.ProcessName)
                    {
                        isAlreadyRunnig = true;
                        break;
                    }
                }
            }
            return isAlreadyRunnig;
        }

        /// <summary>
        /// 16bit의 Endian을 바꾼다 (little -> big, big -> little)
        /// </summary>
        /// <param name="pInt16Value"></param>
        public static void ConvertEndian(ref short pInt16Value)
        {
            byte[] temp = BitConverter.GetBytes(pInt16Value);
            Array.Reverse(temp);
            pInt16Value = BitConverter.ToInt16(temp, 0);
        }

        /// <summary>
        /// 16bit의 Endian을 바꾼다 (little -> big, big -> little)
        /// </summary>
        /// <param name="pUint16Value"></param>
        public static void ConvertEndian(ref ushort pUint16Value)
        {
            byte[] temp = BitConverter.GetBytes(pUint16Value);
            Array.Reverse(temp);
            pUint16Value = BitConverter.ToUInt16(temp, 0);
        }

        /// <summary>
        /// 32bit의 Endian을 바꾼다 (little -> big, big -> little)
        /// </summary>
        /// <param name="pInt32Value"></param>
        public static void ConvertEndian(ref int pInt32Value)
        {
            byte[] temp = BitConverter.GetBytes(pInt32Value);
            Array.Reverse(temp);
            pInt32Value = BitConverter.ToInt32(temp, 0);
        }

        /// <summary>
        /// 32bit의 Endian을 바꾼다 (little -> big, big -> little)
        /// </summary>
        /// <param name="pUint32Value"></param>
        public static void ConvertEndian(ref uint pUint32Value)
        {
            byte[] temp = BitConverter.GetBytes(pUint32Value);
            Array.Reverse(temp);
            pUint32Value = BitConverter.ToUInt32(temp, 0);
        }

        /// <summary>
        /// 64bit의 Endian을 바꾼다 (little -> big, big -> little)
        /// </summary>
        /// <param name="pInt64Value"></param>
        public static void ConvertEndian(ref long pInt64Value)
        {
            byte[] temp = BitConverter.GetBytes(pInt64Value);
            Array.Reverse(temp);
            pInt64Value = BitConverter.ToInt64(temp, 0);
        }

        /// <summary>
        /// 64bit의 Endian을 바꾼다 (little -> big, big -> little)
        /// </summary>
        /// <param name="pUint64Value"></param>
        public static void ConvertEndian(ref ulong pUint64Value)
        {
            byte[] temp = BitConverter.GetBytes(pUint64Value);
            Array.Reverse(temp);
            pUint64Value = BitConverter.ToUInt64(temp, 0);
        }

        public static void DeleteOldFile(string dirName, int dayOld)
        {
            if (dirName == "")
                return;

            if (!System.IO.Directory.Exists(dirName))
                return;

            string[] files = System.IO.Directory.GetFiles(dirName);

            foreach (string file in files)
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(file);
                if (fi.LastWriteTime < DateTime.Now.AddDays(-dayOld))
                {
                    try
                    {
                        fi.Delete();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
    }
}
