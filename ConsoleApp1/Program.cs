using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            RollerCoasterMaker game = new RollerCoasterMaker();

            game.BuildUpward();
            game.BuildDownward();
            game.BuildLoop();
            game.Back();
            game.BuildStright();
            game.BuildStright();
            game.BuildStright();
        }
    }
}
