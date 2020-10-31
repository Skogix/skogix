// Learn more about F# at http://fsharp.org

open System
open System.Diagnostics
open Snake.Core

type Output =
  | Debug
  | Playing
let output = Debug
[<EntryPoint>]
let main _ =
  Console.Clear()
  Console.CursorVisible <- false
  
  
  
  let config = { startPosition = {x=3;y=3}
                 startDirection = Right}
  let sendCommand, commandstream =
    let e = Event<_>()
    e.Trigger, e.Publish
  let drawChar (pos:Position) =
    Console.SetCursorPosition(pos.x, pos.y)
    Console.Write('#')
    
  let drawSnake = List.iter drawChar
  let printGameState = drawSnake
  let printDebug = printfn "%s"
  let printPauseMenu =
    Console.Clear()
    printfn "Paused"
  let renderer:Renderer =
    { GameState = printGameState
      Debug = printDebug
      Pause = printPauseMenu}
  let game = Snake.Game.startGame config renderer commandstream
  let rec getInput() =
    match Console.ReadKey(true).KeyChar with
    | ',' -> ChangeDirection Up |> sendCommand
    | 'o' -> ChangeDirection Down |> sendCommand
    | 'a' -> ChangeDirection Left |> sendCommand
    | 'e' -> ChangeDirection Right |> sendCommand
    | ' ' -> AddTail |> sendCommand
    | 'x' -> Die |> sendCommand
    | 'p' -> Pause |> sendCommand
    getInput()
  getInput()
  0 // return an integer exit code
