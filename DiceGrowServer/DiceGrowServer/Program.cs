using System;

namespace DiceGrowServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("주사위 키우기 서버 시작");

            //서버 클래스 생성 및 실행
            DiceServer diceServer = new DiceServer();
            diceServer.StartServer();


            // 생성된 서버를 관리하는 코드가 필요.
            // 어디다 구현해야하는가가 문제
        }
    }
}
