using System;
using System.Threading;

using Wistery.Majong;

namespace Mjai3D
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        
        static readonly bool DEBUG = true;

        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
                MajongMain majongMain = 
                    DEBUG ? 
                    new FileMajongMain("../input3.txt", new XnaGuiAI(game)) as MajongMain : 
                    new SocketMajongMain("www3257ui.sakura.ne.jp", 11600, "default", "wistery_k", new XnaGuiAI(game)) as MajongMain;

                game.Initialized += () => new Thread(new ThreadStart(majongMain.Main)).Start();
                
                game.Run();
            }
        }
    }
#endif
}

