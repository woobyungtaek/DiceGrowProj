using System;
using System.Collections.Generic;
using System.Text;

namespace DiceGrowClient
{
    public static class Logger
    {
        public static void Log(string str)
        {
            Console.WriteLine(string.Format("[Log] {0}",str));
        }

        public static void Error(string str)
        {
            Console.WriteLine(string.Format("[Err] {0}", str));
        }
        public static void Sys(string str)
        {
            Console.WriteLine(string.Format("[Sys] {0}", str));
        }
    }
}
