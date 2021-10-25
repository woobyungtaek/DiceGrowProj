using System;

namespace DiceGrowClient
{
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("주사위 키우기 클라이언트 시작");

            // 클라이언트 클래스 생성 및 실행
            DiceClient diceClient = new DiceClient();
            diceClient.StartClient();

            // 생성된 클라이언트를 관리하는 코드가 필요.
        }
    }
}
