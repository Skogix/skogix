// Learn more about F# at http://fsharp.org

open System
open Parser
open Basics


[<EntryPoint>]
let main _ =
  Console.Clear()
  let parseA = parseChar 'a'
  let parseB = parseChar 'b'
  let parseAThenB = parseA .>>. parseB
  run parseA "abc"
  
  0 // return an integer exit code
