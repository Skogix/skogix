// Learn more about F# at http://fsharp.org

open System
open Game.Init

type TestIntComponent = {num: int}
type TestStringComponent = {str: string}
[<EntryPoint>]
let main _ =
  Console.Clear()
  Console.CursorVisible <- false
  
  let gameRenderer = printfn "GameRenderer::: %s"
  let debugRenderer = printfn "DebugRenderer::: %s" 
  
  let outputStream:Renderer = {
    Renderer.Game = gameRenderer
    Debug = debugRenderer }
  let input, inputStream =
    let event = Event<_>()
    event.Trigger, event.Publish
  
  let gameCreator = Game.Init.gameCreator
  gameCreator.addComponentType<TestIntComponent>()
  gameCreator.addComponentType<TestStringComponent>()
  gameCreator.addOutputSource outputStream
  
  let game = Game.Init.game
  let input = game.input
  
  input "test"
  
  let tester = Game.Init.tester
  
  
  
  
  
//  initInput.AddCommand "Add" (fun x y -> x + y)
//  
//  let game = initInput.StartGame
//  
//  game.input "Add" 5 5
  
  
  
  
  Console.ReadLine() |> ignore
  0