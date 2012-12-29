using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace Mjai3D
{
    class Log
    {
        public static void Write(string s)
        {
            File.AppendAllText("log.txt", s);
        }

        public static void WriteLine()
        {
            Write("\n");
        }

        public static void WriteLine(string s)
        {
            Write(s + "\n");
        }
    }
}
