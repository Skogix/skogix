using System;
using Game;
using Microsoft.FSharp.Control;
using Microsoft.FSharp.Core;
using static Game.Init;

namespace CProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            var gameInit = Init.gameCreator;
            gameInit.addOutputSource(new Renderer(FSharpFunc<string, Unit>.FromConverter(Renderer), FSharpFunc<string, Unit>.FromConverter(Renderer) ));
            gameInit.addComponentType<TestGameComponent>();
            gameInit.addComponentType<CSharpComponent>();
            var g = game;
            var t = tester;
            var huhu = game.input;
            var output = gameEngineRenderer;
            output.Debug.Invoke("Test");
            output.Game.Invoke("Test");
            huhu.Invoke("test");
        }


        private static Unit Renderer(string input)
        {
            Console.WriteLine(input);
            return null;
        }
    }

    internal class CSharpComponent
    {
    }
}