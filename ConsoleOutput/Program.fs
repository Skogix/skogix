// Learn more about F# at http://fsharp.org

open System
open Snake.Core
[<EntryPoint>]
let main _ =
  Console.Clear()
  let config: Snake.Core.Config = { startPosition = {x=0;y=0}
                                    startDirection = Snake.Core.Direction.Right}
  let renderer: Renderer=
    let redraw (s:Snake) = printfn "%A" s
    redraw
  let commandstream = [Snake.Core.Action.ChangeDirection]
  let game = Snake.Game.startGame config renderer commandstream
  printfn "%A" game
  Console.ReadKey() |> ignore
  0 // return an integer exit code
