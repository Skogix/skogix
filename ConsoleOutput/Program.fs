﻿// Learn more about F# at http://fsharp.org

open System
open System.Diagnostics
open System.Net.Mail
open GameEngine
open Sandbox.Rpg1

type Output =
  | Debug
  | Playing
let output = Debug

[<EntryPoint>]
let main _ =
  Console.Clear()
  Console.CursorVisible <- false
  
  
  Console.ReadKey() |> ignore
  0
//  let showTileMap x =
////    Console.Clear()
//    printfn "printtilemap: %A" x
//    
//  let showDebug = printfn "%A"
//  let renderer:Renderer = { PrintTileMap = showTileMap
//                            PrintDebug = showDebug}
//  let sendCommand, commandStream =
//    let e = Event<_>()
//    e.Trigger, e.Publish
//  let huhu = StartGame renderer commandStream
//  let rec getInput() =
//    match Console.ReadKey(true).KeyChar with
//    | ',' -> MovePlayer Up |> sendCommand
//    | 'o' -> MovePlayer Down |> sendCommand
//    | 'a' -> MovePlayer Left |> sendCommand
//    | 'e' -> MovePlayer Right |> sendCommand
//    | 'p' -> Pause |> sendCommand
//    | _ -> ()
//    getInput()
//  getInput()
//  Console.ReadKey(true) |> ignore
//  0
//  
//  
//  
//  let config = { startPosition = {x=3;y=3}
//                 startDirection = Right}
//  let sendCommand, commandstream =
//    let e = Event<_>()
//    e.Trigger, e.Publish
//  let drawChar (pos:Position) =
//    Console.SetCursorPosition(pos.x, pos.y)
//    Console.Write('#')
//    
//  let drawSnake (snake:Body) =
//    snake
//    |> List.iter drawChar
//  let printGameState s =
////    drawSnake s.body
//    printfn "%A" s
//  let printDebug = printfn "%s"
//  let printPauseMenu =
//    printfn "Paused"
//    
//  let renderer:Renderer =
//    match output with
//    | Debug -> {
//        GameState = fun s -> ()
//        Debug = printDebug
//        Pause = ()
//      }
//    | _ ->
//      { GameState = printGameState
//        Debug = printDebug
//        Pause = printPauseMenu}
//  let game = Snake.Game.startGame config renderer commandstream
//  Move |> sendCommand
//  let rec getInput() =
//    match Console.ReadKey(true).KeyChar with
//    | ',' -> ChangeDirection Up |> sendCommand
//    | 'o' -> ChangeDirection Down |> sendCommand
//    | 'a' -> ChangeDirection Left |> sendCommand
//    | 'e' -> ChangeDirection Right |> sendCommand
//    | ' ' -> AddTail |> sendCommand
//    | 'x' -> Die |> sendCommand
//    | 'p' -> Pause |> sendCommand
//    | _ -> ()
//    getInput()
//  getInput()
//  0 // return an integer exit code
