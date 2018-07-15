using RCLibrary;
using System;

namespace RCConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            RollerCoasterMaker game = new RollerCoasterMaker();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();

            game.BuildLoop();
            game.Back();
        }
    }
}
