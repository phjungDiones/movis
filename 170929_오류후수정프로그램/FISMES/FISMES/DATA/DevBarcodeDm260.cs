using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Cognex.DataMan.SDK;
using System.Threading;
using Cognex.DataMan.SDK.Discovery;
using Cognex.DataMan.SDK.Utils;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FISMES.GLOBAL;
using System.Net.Sockets;
using FISMES.DATA;
using FISMES.SOCKET;
using System.Threading.Tasks;
using FISMES.DATA;

namespace FISMES.DATA
{
    public class DevBarcodeDm260
    {
        public bool bGetxml = false;
        public string result = "";
        private DataManSystem m_System = null;
        private object _currentResultInfoSyncLock = new object();
        SynchronizationContext _syncContext = null;
        private ResultCollector _results;
        SerSystemConnector m_Connector = null;

        public delegate void DeleException(string strErr);
        public event DeleException deleException = null;

        public delegate void DeleReceived(string strBarcode);
        public event DeleReceived deleReceived = null;

        public delegate void DeleSent(string strMsg);
        public event DeleSent deleSent = null;

        /// <summary>
        /// 바코드 읽기 상태 ON
        /// </summary>
        /// <returns></returns>
        public bool TriggerOn()
        {
            if (!IsConnected) return false;

            LogManager.Instance.WriteI(LOG_TYPE.COMM_RFID, "_system.SendCommand(TRIGGER ON);");
            try
            {
                m_System.SendCommand("TRIGGER ON");

                if (deleSent != null)
                    deleSent("TRIGGER ON");
            }
            catch (Exception e)
            {
                LogManager.Instance.WriteI(LOG_TYPE.EXCEPTION, e.ToString());
                return false;
            }
            return true;
        }

        /// <summary>
        /// 바코드 읽기 상태 OFF
        /// </summary>
        public bool TriggerOff()
        {
            if (!IsConnected) return false;

            try
            {
            LogManager.Instance.WriteI(LOG_TYPE.COMM_RFID, "_system.SendCommand(TRIGGER OFF);");
            m_System.SendCommand("TRIGGER OFF");
            
            
                if (deleSent != null)
                    deleSent("TRIGGER OFF");
            }
            catch (Exception e)
            {
                LogManager.Instance.WriteI(LOG_TYPE.EXCEPTION, e.ToString());
                return false;
            }
            return true;
        }

        /// <summary>
        /// 연결 상태
        /// </summary>
        public bool IsConnected
        {
            get { return m_System != null && m_System.State == Cognex.DataMan.SDK.ConnectionState.Connected; }
        }

        /// <summary>
        /// 연결 해제
        /// </summary>
        public void Disconnect()
        {
            if (new SystemConnectedHandler(OnSystemConnected) != null)
            {//종료버튼 클릭시 오류떠서 주석처리

                //m_System.SystemConnected -= new SystemConnectedHandler(OnSystemConnected);
                //m_System.SystemWentOnline -= new SystemWentOnlineHandler(OnSystemWentOnline);
                //m_System.SystemWentOffline -= new SystemWentOfflineHandler(OnSystemWentOffline);
                //m_System.KeepAliveResponseMissed -= new KeepAliveResponseMissedHandler(OnKeepAliveResponseMissed);
                //m_System.BinaryDataTransferProgress -= new BinaryDataTransferProgressHandler(OnBinaryDataTransferProgress);
                //m_System.OffProtocolByteReceived -= new OffProtocolByteReceivedHandler(OffProtocolByteReceived);
                //m_System.AutomaticResponseArrived -= new AutomaticResponseArrivedHandler(AutomaticResponseArrived);

                //LogManager.Instance.WriteI(LOG_TYPE.COMM_RFID, string.Format("Disconnect(); {0}", Cognex.DataMan.SDK.ConnectionState.Connected));
                //if (m_System.State == Cognex.DataMan.SDK.ConnectionState.Connected)
                //    m_System.Disconnect();

                //m_System.SystemDisconnected -= new SystemDisconnectedHandler(OnSystemDisconnected);
            }
        }

        /// <summary>
        /// 연결
        /// </summary>
        public void Connect()
        {
            _syncContext = WindowsFormsSynchronizationContext.Current;

            //SerSystemDiscoverer serSystemDiscoverer = new SerSystemDiscoverer();
            //serSystemDiscoverer.SystemDiscovered += new SerSystemDiscoverer.SystemDiscoveredHandler(serSystemDiscoverer_SystemDiscovered);

            //LogManager.Instance.WriteI(LOG_TYPE.COMM_RFID, "Connect -> Discover();");
            //serSystemDiscoverer.Discover();

            m_Connector = new SerSystemConnector("COM7", 115200);




            m_System = new DataManSystem(m_Connector);



            LogManager.Instance.WriteI(LOG_TYPE.COMM_RFID, string.Format("System -> {0}", m_System.ToString()));
            m_System.DefaultTimeout = 5000;

            LogManager.Instance.WriteI(LOG_TYPE.COMM_RFID, "Event Registered");
            m_System.SystemConnected += new SystemConnectedHandler(OnSystemConnected);
            m_System.SystemDisconnected += new SystemDisconnectedHandler(OnSystemDisconnected);
            m_System.SystemWentOnline += new SystemWentOnlineHandler(OnSystemWentOnline);
            m_System.SystemWentOffline += new SystemWentOfflineHandler(OnSystemWentOffline);
            m_System.KeepAliveResponseMissed += new KeepAliveResponseMissedHandler(OnKeepAliveResponseMissed);
            m_System.BinaryDataTransferProgress += new BinaryDataTransferProgressHandler(OnBinaryDataTransferProgress);
            m_System.OffProtocolByteReceived += new OffProtocolByteReceivedHandler(OffProtocolByteReceived);
            m_System.AutomaticResponseArrived += new AutomaticResponseArrivedHandler(AutomaticResponseArrived);

            // Subscribe to events that are signalled when the device sends auto-responses.
            ResultTypes requested_result_types = ResultTypes.ReadXml | ResultTypes.Image | ResultTypes.ImageGraphics;
            _results = new ResultCollector(m_System, requested_result_types);
            _results.ComplexResultCompleted += Results_ComplexResultCompleted;
            _results.SimpleResultDropped += Results_SimpleResultDropped;

            m_System.SetKeepAliveOptions(true, 3000, 1000);

            try
            {
                m_System.Connect();
                m_System.SetResultTypes(requested_result_types);

                LogManager.Instance.WriteI(LOG_TYPE.COMM_RFID, "Success connected");
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteI(LOG_TYPE.COMM_RFID, ex.ToString());
            }
        }

        void serSystemDiscoverer_SystemDiscovered(SerSystemDiscoverer.SystemInfo systemInfo)
        {
            if (systemInfo == null) return;

            LogManager.Instance.WriteI(LOG_TYPE.COMM_RFID, systemInfo.ToString());
            LogManager.Instance.WriteI(LOG_TYPE.COMM_RFID, string.Format("SystemInfo PortName : {0}, BaudRate : {1}", systemInfo.PortName, systemInfo.Baudrate));

        
                m_Connector = new SerSystemConnector(systemInfo.PortName, systemInfo.Baudrate);
      



                m_System = new DataManSystem(m_Connector);


                
            LogManager.Instance.WriteI(LOG_TYPE.COMM_RFID, string.Format("System -> {0}", m_System.ToString()));
            m_System.DefaultTimeout = 5000;

            LogManager.Instance.WriteI(LOG_TYPE.COMM_RFID, "Event Registered");
            m_System.SystemConnected += new SystemConnectedHandler(OnSystemConnected);
            m_System.SystemDisconnected += new SystemDisconnectedHandler(OnSystemDisconnected);
            m_System.SystemWentOnline += new SystemWentOnlineHandler(OnSystemWentOnline);
            m_System.SystemWentOffline += new SystemWentOfflineHandler(OnSystemWentOffline);
            m_System.KeepAliveResponseMissed += new KeepAliveResponseMissedHandler(OnKeepAliveResponseMissed);
            m_System.BinaryDataTransferProgress += new BinaryDataTransferProgressHandler(OnBinaryDataTransferProgress);
            m_System.OffProtocolByteReceived += new OffProtocolByteReceivedHandler(OffProtocolByteReceived);
            m_System.AutomaticResponseArrived += new AutomaticResponseArrivedHandler(AutomaticResponseArrived);

            // Subscribe to events that are signalled when the device sends auto-responses.
            ResultTypes requested_result_types = ResultTypes.ReadXml | ResultTypes.Image | ResultTypes.ImageGraphics;
            _results = new ResultCollector(m_System, requested_result_types);
            _results.ComplexResultCompleted += Results_ComplexResultCompleted;
            _results.SimpleResultDropped += Results_SimpleResultDropped;

            m_System.SetKeepAliveOptions(true, 3000, 1000);

            try
            {
                m_System.Connect();
                m_System.SetResultTypes(requested_result_types);

                LogManager.Instance.WriteI(LOG_TYPE.COMM_RFID, "Success connected");
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteI(LOG_TYPE.COMM_RFID, ex.ToString());
            }
        }

        private string GetReadStringFromResultXml(string resultXml)
        {
            try
            {
                XmlDocument doc = new XmlDocument();

                doc.LoadXml(resultXml);
                //MessageBox.Show(resultXml.ToString());

                XmlNode full_string_node = doc.SelectSingleNode("result/general/full_string");

                if (full_string_node != null && m_System != null && m_System.State == Cognex.DataMan.SDK.ConnectionState.Connected)
                {
                    //MessageBox.Show("full_string_node != null && _system != null && _system.State == ConnectionState.Connected");
                    XmlAttribute encoding = full_string_node.Attributes["encoding"];
                    if (encoding != null && encoding.InnerText == "base64")
                    {
                        //MessageBox.Show("XmlAttribute encoding != null && encoding.InnerText");
                        if (!string.IsNullOrEmpty(full_string_node.InnerText))
                        {
                            //MessageBox.Show("!string.IsNullOrEmpty(full_string_node.InnerText)");
                            byte[] code = Convert.FromBase64String(full_string_node.InnerText);
                            result = full_string_node.InnerText;
                            return m_System.Encoding.GetString(code, 0, code.Length);
                        }
                        else
                        {
                            return "";
                        }
                    }

                    return full_string_node.InnerText;
                }
            }
            catch
            {
            }

            return "";
        }

        private void ShowResult(ComplexResult complexResult)
        {
            LogManager.Instance.WriteI(LOG_TYPE.COMM_RFID, string.Format("결과값 플레그여부 : ", GlobalVariable.FLAG_RECV_BARCODE.ToString()));
            List<Image> images = new List<Image>();
            List<string> image_graphics = new List<string>();
            string read_result = null;
            int result_id = -1;
            ResultTypes collected_results = ResultTypes.None;

            // Take a reference or copy values from the locked result info object. This is done
            // so that the lock is used only for a short period of time.
            lock (_currentResultInfoSyncLock)
            {
                foreach (var simple_result in complexResult.SimpleResults)
                {
                    collected_results |= simple_result.Id.Type;

                    switch (simple_result.Id.Type)
                    {
                        case ResultTypes.Image:
                            //MessageBox.Show(" ResultTypes.Image:");
                            Image image = ImageArrivedEventArgs.GetImageFromImageBytes(simple_result.Data);
                            //MessageBox.Show(simple_result.Data.ToString());
                            if (image != null)
                                images.Add(image);
                            break;

                        case ResultTypes.ImageGraphics:
                            image_graphics.Add(simple_result.GetDataAsString());
                            //MessageBox.Show(" ResultTypes.ImageGraphics");
                            //MessageBox.Show(simple_result.GetDataAsString());
                            break;

                        case ResultTypes.ReadXml:
                            LogManager.Instance.WriteI(LOG_TYPE.COMM_RFID, string.Format("result xml : ", simple_result.GetDataAsString()));
                            read_result = GetReadStringFromResultXml(simple_result.GetDataAsString());
                            LogManager.Instance.WriteI(LOG_TYPE.COMM_RFID, string.Format("result barcode : ", simple_result.GetDataAsString()));
                            result_id = simple_result.Id.Id;
                           

                            if (deleReceived != null)
                                deleReceived(read_result);

                            break;

                        case ResultTypes.ReadString:
                            read_result = simple_result.GetDataAsString();
                            result_id = simple_result.Id.Id;
                            break;
                    }
                }
            }

            //AddListItem(string.Format("Complex result arrived: resultId = {0}, read result = {1}", result_id, read_result));
            //Log("Complex result contains", string.Format("{0}", collected_results.ToString()));
        }

        private void Results_ComplexResultCompleted(object sender, ComplexResult e)
        {
            ShowResult(e);
        }

        private void AddListItem(object item)
        {
           
        }

        private void RefreshGui()
        {
            //bool system_connected = m_System != null && m_System.State == Cognex.DataMan.SDK.ConnectionState.Connected;
            //bool system_ready_to_connect = m_System == null || m_System.State == Cognex.DataMan.SDK.ConnectionState.Disconnected;
            //bool gui_ready_to_connect = listBoxDetectedSystems.SelectedIndex != -1 && listBoxDetectedSystems.Items.Count > listBoxDetectedSystems.SelectedIndex;

            //btnConnect.Enabled = system_ready_to_connect && gui_ready_to_connect;
            //btnDisconnect.Enabled = system_connected;
            //btnTrigger.Enabled = system_connected;
            //cbLiveDisplay.Enabled = system_connected;
        }

        private void OnSystemConnected(object sender, EventArgs args)
        {
            _syncContext.Post(
                delegate
                {
                    AddListItem("System connected");
                    RefreshGui();
                },
                null);
        }

        private void OnSystemDisconnected(object sender, EventArgs args)
        {
            //_syncContext.Post(
            //    delegate
            //    {
            //        AddListItem("System disconnected");
            //        bool reset_gui = false;

            //        if (!_closing && _autoconnect && cbAutoReconnect.Checked)
            //        {
            //            frmReconnecting frm = new frmReconnecting(this, _system);

            //            if (frm.ShowDialog() == DialogResult.Cancel)
            //                reset_gui = true;
            //        }
            //        else
            //        {
            //            reset_gui = true;
            //        }

            //        if (reset_gui)
            //        {
            //            btnConnect.Enabled = true;
            //            btnDisconnect.Enabled = false;
            //            btnTrigger.Enabled = false;
            //            cbLiveDisplay.Enabled = false;

            //            picResultImage.Image = null;
            //            lbReadString.Text = "";
            //        }
            //    },
            //    null);
        }

        private void OnSystemWentOnline(object sender, EventArgs args)
        {
            _syncContext.Post(
                delegate
                {
                    AddListItem("System went online");
                },
                null);
        }

        private void OnSystemWentOffline(object sender, EventArgs args)
        {
            _syncContext.Post(
                delegate
                {
                    AddListItem("System went offline");
                },
                null);
        }

        private void OnKeepAliveResponseMissed(object sender, EventArgs args)
        {
            LogManager.Instance.WriteI(LOG_TYPE.EXCEPTION, "Keep alive missed"); 
        }

        private void OnBinaryDataTransferProgress(object sender, BinaryDataTransferProgressEventArgs args)
        {
            //Log("OnBinaryDataTransferProgress", string.Format("{0}: {1}% of {2} bytes (Type={3}, Id={4})", args.Direction == TransferDirection.Incoming ? "Receiving" : "Sending", args.TotalDataSize > 0 ? (int)(100 * (args.BytesTransferred / (double)args.TotalDataSize)) : -1, args.TotalDataSize, args.ResultType.ToString(), args.ResponseId));
        }

        private void OffProtocolByteReceived(object sender, OffProtocolByteReceivedEventArgs args)
        {
            //Log("OffProtocolByteReceived", string.Format("{0}", (char)args.Byte));
        }

        private void AutomaticResponseArrived(object sender, AutomaticResponseArrivedEventArgs args)
        {
            //Log("AutomaticResponseArrived", string.Format("Type={0}, Id={1}, Data={2} bytes", args.DataType.ToString(), args.ResponseId, args.Data != null ? args.Data.Length : 0));
        }

        private void Results_SimpleResultDropped(object sender, SimpleResult e)
        {
            //MessageBox.Show("Results_SimpleResultDropped");
            _syncContext.Post(
                delegate
                {
                    ReportDroppedResult(e);
                },
                null);
        }

        private void ReportDroppedResult(SimpleResult result)
        {
            AddListItem(string.Format("Partial result dropped: {0}, id={1}", result.Id.Type.ToString(), result.Id.Id));
        }

    }
}
