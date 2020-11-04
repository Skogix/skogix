// Learn more about F# at http://fsharp.org

open System
open GameEngine
type Player = {
  mutable Name: string
}
type TestIntComponent = {num: int}
type TestStringComponent = {str: string}
[<EntryPoint>]
let main _ =
  Console.Clear()
  Console.CursorVisible <- false
  printfn "hello world från program"
//  sSharp """listOfThings [ 123.341, setXtoTrue true, "string", null, innerArrayCommand [1,true,null],{"name": "Skogix", "health": 100} ]"""

  let gameRenderer world = printfn "ProgramGameRender::: %A" world
  let debugRenderer string = printfn "ProgramDebugRenderer::: %s" string
//  let mutable availableCommands:InputCommand list = []
//  let getInputs (commands:InputCommand list) =
//    availableCommands <- commands
//    printfn "InputFunctions: %A" availableCommands
    
  let gameInit = Game.InitModule.gameInit
  let outputStream:OutputStream = { GameState = gameRenderer
                                    Debug = debugRenderer }
//  
//  let input = Game.Input.getInput(outputStream)
  let game = Game.GameModule.getGame(gameInit)
  game.input<Player> "Move Up"
//  let skogix = IO.getSkogix outputStream
//  skogix.input InputCommand.InputTest
//  
//  let game = Skogix(skogixIO)
//  game.agent.Post InputAddOne
//  game.agent.Post InputAddOne
//  game.input.Post (Move Up)
//  Threading.Thread.Sleep 2000
//  printfn "sover 2 sek för output"
//  game.input.Post (InputCommand.PrintWorldState)
//  game.input.Post (PrintWorldState)
  
  
  printfn "\n\n\n\n\n\n\n\n\n\nEnd, sover 2 sek"
  Threading.Thread.Sleep 2000
//  Console.ReadLine() |> ignore
  0