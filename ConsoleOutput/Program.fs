// Learn more about F# at http://fsharp.org

open System
open Basics


[<EntryPoint>]
let main _ =
//  Console.Clear()
  let parseA = parseChar 'a'
  let parseB = parseChar 'b'
  let parseC = parseChar 'c'
  let parseBorC = parseB <|> parseC
  let aAndThenBorC = parseA .>>. parseBorC
  printfn "%A" (run aAndThenBorC "bac")
  printfn "%A" (run aAndThenBorC "abc")
  printfn "%A" (run aAndThenBorC "acb")
  printfn "%A" (run aAndThenBorC "cab")
  
  0 // return an integer exit code
