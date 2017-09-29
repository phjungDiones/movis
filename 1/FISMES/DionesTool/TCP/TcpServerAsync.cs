using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace DionesTool.Tcp
{
    /// <summary>
    /// 비동기 서버 클래스
    /// </summary>
    public class TcpServerAsync : TcpBase
    {
        public delegate void DeleException(string strErr);
        public event DeleException deleException = null;

        // Thread signal.
        public ManualResetEvent allDone = new ManualResetEvent(false);

        Thread threadServer = null;
        bool flagStartServer = false;

        // Client 연결 시 delegate
        public delegate void DeleConnected(bool bConnected, string msg = "");
        public event DeleConnected deleConnected = null;

        // Client 연결 종료 시 delegate
        public delegate void DeleDisconnected(string msg = "");
        public event DeleDisconnected deleDisconnected = null;

        // Client에 Receive 시 delegate
        public delegate void DeleReceived(Socket sw, string msg);
        public event DeleReceived deleReceived = null;

        // Client에 Send 시 delegate
        public delegate void DeleSent(Socket sw, int sentLen, string msg);
        public event DeleSent deleSent = null;

        // Client Socket List
        List<Socket> listClientSocket = new List<Socket>();

        // 클라이언트가 여러 개면 꼬일 수 있다. 추 후 바꾸자.
        string sendMessage = "";

        public bool IsAlive
        {
            get
            {
                return SOCKET != null && !(SOCKET.Poll(1, SelectMode.SelectRead) && SOCKET.Available == 0);
            }
        }

        public int CLIENT_COUNT
        {
            get { return listClientSocket.Count; }
        }

        public TcpServerAsync()
        {
        }

        ~TcpServerAsync()
        {
            Stop();
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="ipAddr">IPAddress</param>
        /// <param name="port">Port</param>
        /// <param name="maxClientCount">최대 클라이언트 수</param>
        public TcpServerAsync(IPAddress ipAddr, int port, int maxClientCount = 100)
        {
            SetServerInfo(ipAddr, port, maxClientCount);
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="port">Port</param>
        /// <param name="maxClientCount">최대 클라이언트 수</param>
        public TcpServerAsync(int port, int maxClientCount = 100)
        {
            SetServerInfo(port, maxClientCount);
        }

        /// <summary>
        /// 서버 설정
        /// </summary>
        /// <param name="ipAddr">IPAddress</param>
        /// <param name="port">Port</param>
        /// <param name="maxClientCount">최대 클라이언트 수</param>
        public void SetServerInfo(IPAddress ipAddr, int port, int maxClientCount = 100)
        {
            socketInfo.ipAddr = ipAddr;
            socketInfo.port = port;
            socketInfo.maxClientCount = maxClientCount;
        }

        /// <summary>
        /// 서버 설정
        /// </summary>
        /// <param name="port">Port</param>
        /// <param name="maxClientCount">최대 클라이언트 수</param>
        public void SetServerInfo(int port, int maxClientCount = 100)
        {
            SetServerInfo(IPAddress.Any, port, maxClientCount);
        }

        public Socket GetClientSocket(int index)
        {
            if (index < 0 || index >= listClientSocket.Count)
                return null;

            return listClientSocket[index];
        }

        /// <summary>
        /// 서버 시작
        /// </summary>
        public void Start()
        {
            flagStartServer = true;

            if (threadServer == null)
                threadServer = new Thread(new ParameterizedThreadStart(StartListening));

            threadServer.Name = "TCP SERVER THREAD";
            if (threadServer.IsAlive == false)
                threadServer.Start(this);
        }

        /// <summary>
        /// 서버 종료
        /// </summary>
        /// <param name="millisecondsTimeout">서버 강제 종료 대기 시간(Thread 종료 대기 시간)</param>
        public void Stop(int millisecondsTimeout = 5000)
        {
            flagStartServer = false;
            allDone.Set();

            // 기존 연결된 클라이언트 소켓 종료
            while (listClientSocket.Count > 0)
            {
                try
                {
                    Socket client = listClientSocket.ElementAt(listClientSocket.Count - 1);
                    client.Close();
                }
                catch (Exception)
                {
                }
                finally
                {
                    listClientSocket.RemoveAt(listClientSocket.Count - 1);

                    if (deleDisconnected != null)
                        deleDisconnected();
                }
            }

            try
            {
                // 서버 소켓 종료
                if (stateObject.socket != null)
                {
                    stateObject.socket.Close();
                    stateObject.socket = null;
                }
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                if (deleException != null)
                    deleException(ex.ToString());
            }

            // Thread 종료
            if (threadServer != null)
            {
                if (threadServer.Join(millisecondsTimeout) == false)
                {
                    System.Diagnostics.Debug.WriteLine("Join Timeout");
                }

                threadServer.Abort();
                threadServer = null;
            }
        }

        /// <summary>
        /// 클라이언트 접속을 대기한다
        /// Thread 함수
        /// </summary>
        /// <param name="obj">CTcpServerAsync 객체</param>
        public void StartListening(object obj)
        {
            TcpServerAsync sv = (TcpServerAsync)obj;

            // Establish the local endpoint for the socket.
            IPEndPoint localEndPoint = new IPEndPoint(sv.IP, sv.PORT);

            // Create a TCP/IP socket.
            sv.SOCKET = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket listener = sv.SOCKET;

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(sv.socketInfo.maxClientCount);

                while (flagStartServer)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }
            }
            catch (Exception ex)
            {
                if (deleException != null)
                    deleException(ex.ToString());
            }

            System.Diagnostics.Debug.WriteLine("End Thread");
        }

        /// <summary>
        /// Client 접속 후 callback 함수
        /// </summary>
        /// <param name="ar"></param>
        public void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;

            try
            {
                // Client 연결 Socket
                Socket handler = listener.EndAccept(ar);

                if (listClientSocket.Count >= socketInfo.maxClientCount)
                {
                    // 기존 연결 해제
                    try
                    {
                        Socket client = listClientSocket.ElementAt(listClientSocket.Count - 1);
                        client.Close();
                    }
                    catch (Exception)
                    {
                    }
                    finally
                    {
                        listClientSocket.RemoveAt(listClientSocket.Count - 1);

                        if (deleDisconnected != null)
                            deleDisconnected();
                    }

                }

                //if (listClientSocket.Count < socketInfo.maxClientCount)
                {
                    // Client Socket List에 추가
                    listClientSocket.Add(handler);

                    // Client 연결 delegate 호출
                    if (deleConnected != null)
                        deleConnected(true);

                    // 비동기 Receive 시작
                    StateObject state = new StateObject();
                    state.socket = handler;
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReadCallback), state);
                }
                //else
                //{
                //    // 최대 접속자 수를 넘어서면 연결 해제
                //    handler.Disconnect(false);
                //    handler.Close();
                //}
                
            }
            catch (ObjectDisposedException ex)
            {
                if (deleException != null)
                    deleException(ex.ToString());

                if (deleConnected != null)
                    deleConnected(false);
            }
            catch (Exception ex)
            {
                if (deleException != null)
                    deleException(ex.ToString());

                if (deleConnected != null)
                    deleConnected(false, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ar"></param>
        public void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.socket;

            try
            {
                // Read data from the client socket. 
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There  might be more data, so store the data received so far.
                    state.sb.Append(Encoding.ASCII.GetString(
                        state.buffer, 0, bytesRead));

                    // Check for end-of-file tag. If it is not there, read 
                    // more data.
                    content = state.sb.ToString();
                    if (delimiterEndMsg == string.Empty || content.IndexOf(delimiterEndMsg) > -1)
                    {
                        // All the data has been read from the 
                        // client. Display it on the console.
                        Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                            content.Length, content);

                        if (deleReceived != null)
                            deleReceived(handler, content);

                        state.sb.Clear();
                    }

                    // Not all data received. Get more.
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReadCallback), state);
                }
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                if (deleException != null)
                    deleException(ex.ToString());

                // Error Code 10054 : Client 연결 끊김
                if (ex.ErrorCode == 10054)
                {
                    listClientSocket.Remove(handler);

                    if (deleDisconnected != null)
                        deleDisconnected(ex.Message);
                }
            }
            catch (Exception ex)
            {
                if (deleException != null)
                    deleException(ex.ToString());
            }
        }

        public void Send(Socket socket, String data)
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            Send(socket, byteData);
        }

        public void Send(Socket socket, byte[] byteData)
        {
            sendMessage = Encoding.ASCII.GetString(byteData, 0, byteData.Length);

            try
            {
                // Begin sending the data to the remote device.
                socket.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), socket);
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                if (deleException != null)
                    deleException(ex.ToString());

                // Error Code 10054 : Client 연결 끊김
                if (ex.ErrorCode == 10054)
                {
                    if (deleDisconnected != null)
                        deleDisconnected(ex.Message);
                }
            }
            catch (Exception ex)
            {
                if (deleException != null)
                    deleException(ex.ToString());
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            // Retrieve the socket from the state object.
            Socket handler = (Socket)ar.AsyncState;

            try
            {
                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                if (deleSent != null)
                    deleSent(handler, bytesSent, sendMessage);
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                if (deleException != null)
                    deleException(ex.ToString());

                // Error Code 10054 : Client 연결 끊김
                if (ex.ErrorCode == 10054)
                {
                    listClientSocket.Remove(handler);

                    if (deleDisconnected != null)
                        deleDisconnected(ex.Message);
                }
            }
            catch (Exception ex)
            {
                if (deleException != null)
                    deleException(ex.ToString());
            }
        }
    }
}
