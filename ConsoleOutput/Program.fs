// Learn more about F# at http://fsharp.org

open System
open Game.Init
open Parser
open Parser.Core

type ParseOutput =
  | Test
  | Skogix of string
type TestIntComponent = {num: int}
type TestStringComponent = {str: string}
[<EntryPoint>]
let main _ =
  Console.Clear()
  Console.CursorVisible <- false
  let p str = printfn "%A" str
  let p1 str a = printfn "%s: %A" str a
  let testCommands = ["test"; "skogix"]
  
  let createParsers ps: Parser<string> list =
    testCommands
    |> List.map Parser.Core.parseString
  let parsers = createParsers testCommands
  printfn "%A" parsers
  
  
  
  
  
//  let parseTest =
//    parseString "test"
//    |>> (fun _ -> Test)
//  let parseSkogix =
//    parseString "skogix"
//    |>> (fun _ -> Skogix "huhu")
//  // ska få tillbaka Parseoutput.Test
//  printfn "%A" (run parseTest "test")
//  // ska få tillbaka Skogix med huhu
//  printfn "%A" (run parseSkogix "skogix")

//  p (huhu "skogix")
//  p (huhu "teeest")
//  p (huhu "wawa")
//  
//  let gameRenderer = printfn "GameRenderer::: %s"
//  let debugRenderer = printfn "DebugRenderer::: %s" 
//  
//  let outputStream:Renderer = {
//    Renderer.Game = gameRenderer
//    Debug = debugRenderer }
//  
//  let gameCreator = Game.Init.gameCreator
//  gameCreator.addComponentType<TestIntComponent>()
//  gameCreator.addComponentType<TestStringComponent>()
//  gameCreator.addOutputSource outputStream
//  
//  let game = Game.Init.game
//  let input = game.input
//  
//  input "Add 5 5" 
//  
//  let tester = Game.Init.tester
  
  
  
  
  
//  initInput.AddCommand "Add" (fun x y -> x + y)
//  
//  let game = initInput.StartGame
//  
//  game.input "Add" 5 5
  
  
  
  
  Console.ReadLine() |> ignore
  0