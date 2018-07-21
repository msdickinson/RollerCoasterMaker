using RCLibrary;
using System;
using System.Collections.Generic;

namespace RCConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            RollerCoasterMaker game = new RollerCoasterMaker();

            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();
            game.BuildLeft();

            game.BuildLeft();
            game.BuildLeft();

            game.BuildFinsh();

            game.Back();
            game.Back();
        }
    }
}
