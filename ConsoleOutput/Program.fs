// Learn more about F# at http://fsharp.org

open System
open Game.Core
open Game.Domain
type Player = {
  Name:string
}
[<EntryPoint>]
let main _ =
  Console.Clear()
  
  Console.ReadKey() |> ignore
  0 // return an integer exit code
