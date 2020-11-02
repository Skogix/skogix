// Learn more about F# at http://fsharp.org

open System
open Game.Init
type Player = {
  mutable Name: string
}

type ParseOutput =
  | Test
  | Skogix of string
type TestIntComponent = {num: int}
type TestStringComponent = {str: string}
[<EntryPoint>]
let main _ =
  Console.Clear()
  Console.CursorVisible <- false
//  sSharp """listOfThings [ 123.341, setXtoTrue true, "string", null, innerArrayCommand [1,true,null],{"name": "Skogix", "health": 100} ]"""
    
  let gameRenderer = printfn "GameRenderer::: %s"
  let debugRenderer = printfn "DebugRenderer::: %s" 
  
  let outputStream:Renderer = {
    Renderer.Game = gameRenderer
    Debug = debugRenderer }
  
  let gameCreator = Game.Init.gameCreator
  gameCreator.addComponentType<TestIntComponent>()
  gameCreator.addComponentType<TestStringComponent>()
  gameCreator.addOutputSource outputStream
  
  let game = Game.Init.game
  let input = game.input
  
  input "Add 5 5" 
  
  let tester = Game.Init.tester
  
  Console.ReadLine() |> ignore
  0