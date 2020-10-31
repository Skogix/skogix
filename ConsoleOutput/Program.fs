// Learn more about F# at http://fsharp.org

open System
open Snake.Core
[<EntryPoint>]
let main _ =
  Console.Clear()
  let config = { startPosition = {x=3;y=3}
                 startDirection = Right}
  let sendCommand, commandstream =
    let e = Event<_>()
    e.Trigger, e.Publish
  let drawChar (pos:Position) =
    Console.SetCursorPosition(pos.x, pos.y)
    Console.Write('#')
    
  let drawSnake (s:Snake) =
    printfn "drawsnake"
    s
    |> List.iter drawChar 
  let renderer s =
//    Console.Clear()
//    drawSnake s
    printfn "%A" s
//    printfn "render snake"
  let game = Snake.Game.startGame config renderer commandstream
  let rec getInput () =
    match Console.ReadKey(true).KeyChar with
    | ',' -> sendCommand (ChangeDirection Up)
    | 'o' -> sendCommand (ChangeDirection Down)
    | 'a' -> sendCommand (ChangeDirection Left)
    | 'e' -> sendCommand (ChangeDirection Right)
    getInput ()
  getInput()
  0 // return an integer exit code
