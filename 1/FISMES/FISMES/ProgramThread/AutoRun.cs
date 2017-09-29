using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FISMES.GLOBAL;
using System.Net.Sockets;
using FISMES.DATA;
using FISMES.SOCKET;
using System.Diagnostics;

namespace FISMES.ProgramThread
{
    public class AutoRun : IDisposable
    {
        Thread threadRun;

        bool flagThreadAlive = false;

        Stopwatch m_swSendBarcode = new Stopwatch();

        public AutoRun()
        {
            StartStopAutoRunThread(true);
        }

        ~AutoRun()
        {
            //
        }

        public void Dispose()
        {
            StartStopAutoRunThread(false);
        }

        public void ResetCmd()
        {
        }

        public void InitSeqNum()
        {
        }


        void StartStopAutoRunThread(bool bStart)
        {
            if (threadRun != null)
            {
                flagThreadAlive = false;
                threadRun.Join(500);
                threadRun.Abort();
                threadRun = null;
            }

            if (bStart)
            {
                flagThreadAlive = true;
                threadRun = new Thread(new ParameterizedThreadStart(ThreadRun));
                threadRun.Name = "Auto Run THREAD";
                if (threadRun.IsAlive == false)
                    threadRun.Start(this);
            }
        }

        void ThreadRun(object obj)
        {
            AutoRun autoRun = obj as AutoRun;

            while (autoRun.flagThreadAlive)
            {
                //if (MachineStatus.AUTO_MODE == false) continue;

                Thread.Sleep(10);

                Run();
            }

            autoRun.flagThreadAlive = false;
        }

        public void Run()
        {
            Socket clientSockMes = GlobalVariable.SOCKET_MES.GetClientSocket(0);
            Socket clientSockFis = GlobalVariable.SOCKET_FIS.GetClientSocket(0);

            if (clientSockMes == null) return;
            if (clientSockFis == null) return;
            if (GlobalVariable.DEV_BARCODE.IsConnected == false) return;

            // F1 -> RFID -> M1 -> M2 -> F2 -> F3 -> M3 -> M4 -> F4
            // Temp Comment
            if (MachineStatus.AUTO_MODE)
            {
                // Received FIS F1 -> Read RFID
                if (GlobalVariable.FLAG_RECV_FIS &&
                    GlobalVariable.FIS_MSG != null &&
                    GlobalVariable.FIS_MSG.COMMAND_ID == "F1")
                {
                    GlobalVariable.FLAG_RECV_FIS = false;
                    m_swSendBarcode.Restart();
                    // BARCODE 읽으라고 신호를 줌
                    GlobalVariable.DEV_BARCODE.TriggerOn();
                    LogManager.Instance.WriteI(LOG_TYPE.SEQUENCE, "Barcode Trigger On Previous F1");
                }

                // Barcode Send 후 응답 없을 때
                if (m_swSendBarcode.IsRunning)
                {
                    if (m_swSendBarcode.Elapsed.TotalSeconds > 10)
                    {
                        LogManager.Instance.WriteI(LOG_TYPE.SEQUENCE, "Barcode Timeout");
                        // 타임아웃..
                        // 빈값을 읽었다고 인식하고 MES에 NG 처리한다
                        GlobalVariable.BARCODE = "";
                        GlobalVariable.FLAG_RECV_BARCODE = true;
                    }
                }




                

                //System.Threading.Thread.Sleep(3000);
                // Received RFID -> SEND MES M1
                // RFID 값을 받았다면 MES에 M1을 보냄
                if (GlobalVariable.FLAG_RECV_BARCODE)
                {
                    LogManager.Instance.WriteI(LOG_TYPE.SEQUENCE, "GlobalVariable.FLAG_RECV_BARCODE ON");
                    m_swSendBarcode.Stop();
                    GlobalVariable.DEV_BARCODE.TriggerOff();
                    GlobalVariable.FLAG_RECV_BARCODE = false;

                    MesMsgM1 msgM1 = new MesMsgM1();
                    msgM1.SetSettings(SettingSocketMES.Instance.MSG_ID,
                                    SettingSocketMES.Instance.FACTORY_ID,
                                    SettingSocketMES.Instance.LINE_ID,
                                    SettingSocketMES.Instance.OPER_ID,
                                    SettingSocketMES.Instance.EQP_POS_ID);

                    // RFID 읽은 값을 넣어주는 대신 바코드 리더기로 읽은 값을 넣어준다
                    // 바코드 정상적으로 읽었는지 결과를 2번째 인자로 넘겨준다
                    //msgM1.SetValue(GlobalVariable.RFID);
                    LogManager.Instance.WriteI(LOG_TYPE.SEQUENCE, "M1 Bacde Set" + GlobalVariable.BARCODE.ToString());
                    msgM1.SetValue(GlobalVariable.BARCODE, GlobalVariable.BARCODE == "");

                    if (clientSockMes == null)
                    {
                        LogManager.Instance.WriteI(LOG_TYPE.SEQUENCE, "AutoRun : MES Client is not connected.");
                        //MessageBox.Show("MES is not connected.");
                        return;
                    }

                    // 바코드 읽기를 실패했을 때 설비에 알람을 발생시키기 위해 F2 실패 메시지를 전달
                    // 설비에서 F3를 주면 안된다
                    if (GlobalVariable.BARCODE == "")
                    {
                        LogManager.Instance.WriteI(LOG_TYPE.SEQUENCE, "barcode not read");
                        FisMsg fmsg = new FisMsg();
                        fmsg.SetValue("F2", "1", "");
                        fmsg.Packing();

                        if (clientSockFis == null)
                        {
                            LogManager.Instance.WriteI(LOG_TYPE.SEQUENCE, "AutoRun : FIS Client is not connected.");
                            //MessageBox.Show("FIS is not connected.");
                            return;
                        }

                        GlobalVariable.SOCKET_FIS.Send(clientSockFis, fmsg.PACKET);
                    }
                    else
                    {
                        // MES M1
                        GlobalVariable.SOCKET_MES.Send(clientSockMes, msgM1.PACKET);       
                    }


                }

                //M2 -> d
                // Received MES M2 -> SEND FIS F2
                if (GlobalVariable.FLAG_RECV_MES && 
                    GlobalVariable.MES_MSG != null &&
                    GlobalVariable.MES_MSG.MES_PROCESS_FLAG == "M2")
                {
                    GlobalVariable.FLAG_RECV_MES = false;

                    GlobalVariable.BARCODE_NO = GlobalVariable.MES_MSG.BARCODE_NO;
                    GlobalVariable.M2_RESULT = GlobalVariable.MES_MSG.MES_RESULT;

                    FisMsg fmsg = new FisMsg();
                    if (GlobalVariable.RESPONSE_F2_ONLY_ZERO)
                    {
                        fmsg.SetValue("F2", "0", "");
                    }
                    else
                    {
                        fmsg.SetValue("F2", GlobalVariable.MES_MSG.MES_RESULT, ""); // mes 온메세지 -. 0 에러 
                    }
                    fmsg.Packing();

                    if (clientSockFis == null)
                    {
                        LogManager.Instance.WriteI(LOG_TYPE.SEQUENCE, "AutoRun : FIS Client is not connected.");
                        return;
                    }

                    GlobalVariable.SOCKET_FIS.Send(clientSockFis, fmsg.PACKET);
                }

                // Received FIS F3 -> SEND MES M3
                if (GlobalVariable.FLAG_RECV_FIS &&
                    GlobalVariable.FIS_MSG != null &&
                    GlobalVariable.FIS_MSG.COMMAND_ID == "F3")
                {
                    GlobalVariable.FLAG_RECV_FIS = false;

                    // 현재 성공일 때만 메시지를 전달한다
                    // Send M3 when M2 Message Result is 0
                    if (GlobalVariable.FIS_MSG.RESULT == "0")
                    {
                        MesMsgM3 msgM3 = new MesMsgM3();
                        msgM3.SetSettings(SettingSocketMES.Instance.MSG_ID,
                                SettingSocketMES.Instance.FACTORY_ID,
                                SettingSocketMES.Instance.LINE_ID,
                                SettingSocketMES.Instance.OPER_ID,
                                SettingSocketMES.Instance.EQP_POS_ID);
                        double dCoatingValue = 0.0;
                        double.TryParse(GlobalVariable.FIS_MSG.VALUE, out dCoatingValue);
                        // 기존 RFID를 넘겨서 MES에서 Barcode를 받았지만 현재는 바로 Barcode를 읽는다
                        //msgM3.SetValue(GlobalVariable.BARCODE_NO, true, dCoatingValue);
                        msgM3.SetValue(GlobalVariable.BARCODE, true, dCoatingValue);

                        if (clientSockMes == null)
                        {
                            LogManager.Instance.WriteI(LOG_TYPE.SEQUENCE, "AutoRun : FIS Client is not connected.");
                            //MessageBox.Show("MES is not connected.");
                            return;
                        }
                        GlobalVariable.SOCKET_MES.Send(clientSockMes, msgM3.PACKET);
                    }
                    // 실패일 경우 F4 응답을 바로 준다
                    else
                    {
                        FisMsg fmsg = new FisMsg();
                        fmsg.SetValue("F4", GlobalVariable.MES_MSG.MES_RESULT, "");
                        fmsg.Packing();

                        if (clientSockFis == null)
                        {
                            LogManager.Instance.WriteI(LOG_TYPE.COMM_FIS, "AutoRun : FIS Client is not connected.");
                            //MessageBox.Show("MES is not connected.");
                            return;
                        }

                        GlobalVariable.SOCKET_FIS.Send(clientSockFis, fmsg.PACKET);
                    }
                }

                // Received MES M4 -> SEND FIS F4
                if (GlobalVariable.FLAG_RECV_MES &&
                    GlobalVariable.MES_MSG != null &&
                    GlobalVariable.MES_MSG.MES_PROCESS_FLAG == "M4")
                {
                    GlobalVariable.FLAG_RECV_MES = false;

                    FisMsg fmsg = new FisMsg();
                    fmsg.SetValue("F4", GlobalVariable.MES_MSG.MES_RESULT, "");
                    fmsg.Packing();

                    if (clientSockFis == null)
                    {
                        LogManager.Instance.WriteI(LOG_TYPE.COMM_FIS, "AutoRun : FIS Client is not connected.");
                        //MessageBox.Show("MES is not connected.");
                        return;
                    }

                    GlobalVariable.SOCKET_FIS.Send(clientSockFis, fmsg.PACKET);
                }

                // FA
                if (GlobalVariable.FLAG_RECV_FIS &&
                    GlobalVariable.FIS_MSG != null &&
                    GlobalVariable.FIS_MSG.COMMAND_ID == "FA")
                {
                    GlobalVariable.FLAG_RECV_FIS = false;

                    MesMsgAlarm msgMA = new MesMsgAlarm();
                    msgMA.SetSettings(SettingSocketMES.Instance.MSG_ID,
                            SettingSocketMES.Instance.FACTORY_ID,
                            SettingSocketMES.Instance.LINE_ID,
                            SettingSocketMES.Instance.OPER_ID,
                            SettingSocketMES.Instance.EQP_POS_ID);
                    msgMA.SetValue(GlobalVariable.FIS_MSG.RESULT == "S", GlobalVariable.FIS_MSG.VALUE);

                    if (clientSockMes == null)
                    {
                        LogManager.Instance.WriteI(LOG_TYPE.COMM_MES, "AutoRun : FIS Client is not connected.");
                        //MessageBox.Show("MES is not connected.");
                        return;
                    }
                    GlobalVariable.SOCKET_MES.Send(clientSockMes, msgMA.PACKET);
                }
            }
            else
            {
                // FA
                if (GlobalVariable.FLAG_RECV_FIS &&
                    GlobalVariable.FIS_MSG != null &&
                    GlobalVariable.FIS_MSG.COMMAND_ID == "FA")
                {
                    GlobalVariable.FLAG_RECV_FIS = false;

                    MesMsgAlarm msgMA = new MesMsgAlarm();
                    msgMA.SetSettings(SettingSocketMES.Instance.MSG_ID,
                            SettingSocketMES.Instance.FACTORY_ID,
                            SettingSocketMES.Instance.LINE_ID,
                            SettingSocketMES.Instance.OPER_ID,
                            SettingSocketMES.Instance.EQP_POS_ID);
                    msgMA.SetValue(GlobalVariable.FIS_MSG.RESULT == "S", GlobalVariable.FIS_MSG.VALUE);

                    if (clientSockMes == null)
                    {
                        LogManager.Instance.WriteI(LOG_TYPE.COMM_MES, "AutoRun : FIS Client is not connected.");
                        //MessageBox.Show("MES is not connected.");
                        return;
                    }
                    GlobalVariable.SOCKET_MES.Send(clientSockMes, msgMA.PACKET);
                }
            }
        }
    }
}
