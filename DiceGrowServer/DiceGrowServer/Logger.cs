using System;
using System.Collections.Generic;
using System.Text;

namespace DiceGrowServer
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
    }
}
