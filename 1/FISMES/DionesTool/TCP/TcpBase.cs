using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace DionesTool.Tcp
{
    /// <summary>
    /// Server와 Client의 기본이 되는 부모 클래스
    /// </summary>
    public class TcpBase
    {
        /// <summary>
        /// 소켓 정보
        /// </summary>
        public class SocketInfo
        {
            public IPAddress ipAddr;
            public int port;
            public int maxClientCount;
        }

        /// <summary>
        /// 클라이언트 접속 시 사용하는 데이터
        /// </summary>
        public class StateObject
        {
            // Client socket.
            public Socket socket = null;
            // Size of receive buffer.
            public const int BufferSize = 1;
            // Receive buffer.
            public byte[] buffer = new byte[BufferSize];
            // Received data string.
            public StringBuilder sb = new StringBuilder();
        }

        public const char STX = (char)0x02;
        public const char ETX = (char)0x03;

        protected SocketInfo socketInfo = new SocketInfo();
        protected StateObject stateObject = new StateObject();

        // 메시지 받을 때 구분자 (이 구분자가 들어오면 Receive Event가 호출된다)
        // Empty라면 매 번 이벤트로 들어온다
        protected string delimiterEndMsg = string.Empty;

        /// <summary>
        /// 아이피
        /// </summary>
        public IPAddress IP
        {
            get { return socketInfo.ipAddr; }
        }

        /// <summary>
        /// 포트
        /// </summary>
        public int PORT
        {
            get { return socketInfo.port; }
        }

        /// <summary>
        /// 최대 클라이언트 수
        /// </summary>
        public int MAX_CLIENT
        {
            get { return socketInfo.maxClientCount; }
        }

        /// <summary>
        /// 소켓
        /// </summary>
        public Socket SOCKET
        {
            get { return stateObject.socket; }
            set { stateObject.socket = value; }
        }

        /// <summary>
        /// 구분자를 설정합니다.
        /// 구분자를 설정하면 Receive 시 구분자가 있을 경우 delegate 리턴합니다.
        /// 현재 자릿수는 하나만 체크됩니다.
        /// </summary>
        /// <param name="delimeter"></param>
        public void SetDelimeter(string delimeter)
        {
            delimiterEndMsg = delimeter;
        }
    }
}
