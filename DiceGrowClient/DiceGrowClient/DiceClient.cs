using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DiceGrowClient
{
    class DiceClient
    {
        public void StartClient()
        {
            Thread clientThread = new Thread(ClientMainFunc);
            clientThread.IsBackground = false;
            clientThread.Start();
        }

        private void ClientMainFunc(object obj)
        {
            // 서버에 연결하기
            Socket connectSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            EndPoint serverEP = new IPEndPoint(IPAddress.Loopback, 10200);

            while (true)
            {
                try
                {
                    Logger.Log("연결 시도 중...");
                    connectSocket.Connect(serverEP);

                    // Connect에 성공하면 아래 실행
                    Logger.Log("연결 성공!!");
                    break;
                }
                catch (SocketException ex)
                {
                    Logger.Error(string.Format("코드 : {0} / {1}", ex.SocketErrorCode, ex.Message));

                    //연결 실패시 처리가 필요.
                }
            }

            connectSocket.Close();
        }
    }
}
