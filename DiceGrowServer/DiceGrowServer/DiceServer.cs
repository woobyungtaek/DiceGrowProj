using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DiceGrowServer
{
    public class AsyncStateData
    {
        public byte[] buffer;
        public Socket socket;
        // 연결된 클라이언트가 다루고있는 캐릭터도 변수로 있어야하지 않을까?
    }

    // 서버의 메인 부분
    class DiceServer
    {
        private bool mbStart; // 서버 시작 여부 확인

        public DiceServer()
        {
            mbStart = false;
        }

        /// <summary>
        /// 서버 실행 
        /// </summary>
        public void StartServer()
        {
            if(mbStart == true)
            {
                Logger.Error("이미 실행중인 서버입니다.");
                return;
            }

            mbStart = true;

            /// Main Thread를 실행
            Thread serverThread = new Thread(ServerMainFunc);
            serverThread.IsBackground = false;
            serverThread.Start();
        }

        /// <summary>
        /// 서버 메인 동작
        /// </summary>
        /// <param name="obj"></param>
        private static void ServerMainFunc(object obj)
        {
            Logger.Log("서버 메인 함수 실행");
            // 연결용 소켓
            Socket acceptSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // 끝단 설정
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 10200);
            acceptSocket.Bind(endPoint);
            acceptSocket.Listen(10);

            // Accept시작
            while(true)
            {
                Socket clientSocket = acceptSocket.Accept();

                Logger.Log(string.Format("Client가 접속하였습니다.{0}", (clientSocket.RemoteEndPoint as IPEndPoint).Address));

                // 버퍼, 소켓 묶음 class 생성
                AsyncStateData asyncStateData = new AsyncStateData();
                asyncStateData.buffer = new byte[1024];
                asyncStateData.socket = clientSocket;

                // 스레드 풀을 이용해 새 스레드를 열고 그곳에서 처리가 되야한다.
                // 리시브 시작
                clientSocket.BeginReceive(asyncStateData.buffer, 0, asyncStateData.buffer.Length, SocketFlags.None, AsyncReceiveCallBack, asyncStateData);
            }
        }

        /// <summary>
        /// 받기 콜백 함수
        /// </summary>
        /// <param name="asyncResult"></param>
        private static void AsyncReceiveCallBack(IAsyncResult asyncResult)
        {
            try
            {
                AsyncStateData data = asyncResult.AsyncState as AsyncStateData;

                int nRecv = data.socket.EndReceive(asyncResult);
                string txt = Encoding.UTF8.GetString(data.buffer, 0, nRecv);

                Logger.Log(string.Format("받은 메세지 : {0}", txt));

                byte[] sendBytes = Encoding.UTF8.GetBytes(string.Format("Echo : {0}", txt));
                data.socket.BeginSend(sendBytes, 0, sendBytes.Length, SocketFlags.None, AsyncSendCallBack, data);

                // 받은 메세지의 종류에 따라 처리가 달라져야한다.
                // 종료 또는 시스템 관련 메세지라면 여기에서
                // 게임 관련 메세지라면 게임메니저에게 보내서 알맞은 처리가 되도록해야하며
                // 특정 경우가 아니라면 다시 리시브를 걸어둬야합니다.
            }
            catch (SocketException ex)
            {
                // ex의 코드에 맞춰서 예외처리 필요
                Logger.Error(string.Format("코드 : {0} / {1}", ex.ErrorCode, ex.Message));
            }
        }

        /// <summary>
        /// 보내기 콜백 함수
        /// </summary>
        /// <param name="asyncResult"></param>
        private static void AsyncSendCallBack(IAsyncResult asyncResult)
        {
            AsyncStateData data = asyncResult.AsyncState as AsyncStateData;
            data.socket.EndSend(asyncResult);

            // 대부분의 경우 리시브를 따로 걸어줘야 할듯.
            data.socket.BeginReceive(data.buffer, 0, data.buffer.Length, SocketFlags.None, AsyncReceiveCallBack, data);
        }
    }
}
