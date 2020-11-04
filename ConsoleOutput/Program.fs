// Learn more about F# at http://fsharp.org

open System
open GameEngine.Domain
open GameEngine.Game
open GameEngine.GameInit
type Player = {
  mutable Name: string
}
type TestIntComponent = {num: int}
type TestStringComponent = {str: string}
[<EntryPoint>]
let main _ =
  Console.Clear()
  Console.CursorVisible <- false
//  sSharp """listOfThings [ 123.341, setXtoTrue true, "string", null, innerArrayCommand [1,true,null],{"name": "Skogix", "health": 100} ]"""

  // skicka in outputsorces, få tillbaka en init
  // skicka in init till game, få tillbaka inputstream
  
  let gameRenderer world = printfn "ProgramGameRender::: %A" world
  let debugRenderer string = printfn "ProgramDebugRenderer::: %s" string
  let mutable availableCommands:InputCommand list = []
  let getInputs (commands:InputCommand list) =
    availableCommands <- commands
    printfn "InputFunctions: %A" availableCommands
    
  
  let outputStream:OutputStream = { Game = gameRenderer
                                    InputFunctions = getInputs
                                    Debug = debugRenderer }
  
  let skogixIO, huhu = gameInit outputStream
  
  let game = Skogix(skogixIO)
  game.agent.Post InputAddOne
  game.agent.Post InputAddOne
//  game.input.Post (Move Up)
//  Threading.Thread.Sleep 2000
//  printfn "sover 2 sek för output"
//  game.input.Post (InputCommand.PrintWorldState)
//  game.input.Post (PrintWorldState)
  
//  let init = new gameInit(outputStream)
//  let game = new GameEngine.Game.Game(init)
//  game.input availableCommands.Head
//  game.input (Move Up)
  
//  gameCreator.addComponentType<TestStringComponent>()
//  gameCreator.addOutputSource outputStream
//  
//  let game = Game.Init.game
//  let input = game.input
//  
//  input "Add 5 5" 
//  
//  let tester = Game.Init.tester
  
  printfn "\n\n\n\n\n\n\n\n\n\nEnd, sover 2 sek"
  Threading.Thread.Sleep 2000
//  Console.ReadLine() |> ignore
  0