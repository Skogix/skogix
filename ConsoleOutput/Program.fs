// Learn more about F# at http://fsharp.org

open System
open Game.Components
open Game.Domain
open GameEngine.GameEngine

[<EntryPoint>]
let main _ =
  Console.Clear()
  Console.CursorVisible <- false
  printfn "hello world från program"
//  sSharp """listOfThings [ 123.341, setXtoTrue true, "string", null, innerArrayCommand [1,true,null],{"name": "Skogix", "health": 100} ]"""
  let drawGameState (world:World) = printfn "State: %A" world
  let drawCommands (inputCommands:InputCommand list): InputCommand =
    printfn "Commands: %A" inputCommands
    Wait
//    
  let outputStream: OutputStream = {
    OutputStateStream = drawGameState
    OutputCommands = drawCommands
  }
  let e1 = EntityManager.CreateEntity()
  let pos = e1.add<PositionComponent>({x=3;y=3})
  pos.data.x <- 100
  
  printfn "TryGetPositionComponent: %A" (e1.tryGet<PositionComponent>())
  printfn "TryGetTempComponent: %A" (e1.tryGet<TempComponent>())
//  
//
  printfn "\n\n\n\n\n\n\n\n\n\nEnd, sover 2 sek"
  Threading.Thread.Sleep 2000
//  Console.ReadLine() |> ignore
  0