// Learn more about F# at http://fsharp.org

open System
open Basics


[<EntryPoint>]
let main _ =
//  Console.Clear()
  let parseA = parseChar 'a'
  let parseB = parseChar 'b'
  let parseAThenB = parseA .>>. parseB
  printfn "%A" (run parseA "abc")
  printfn "%A" (run parseA "bbb")
  printfn "%A" (run parseAThenB "bac")
  
  0 // return an integer exit code
