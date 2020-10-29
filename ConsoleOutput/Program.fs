// Learn more about F# at http://fsharp.org

open System
open Basics


[<EntryPoint>]
let main _ =
//  Console.Clear()
  let parseLowercase = anyOf['a'..'z']
  let parseDigit = anyOf['0'..'9']
  
  printfn "%A" (run parseLowercase "aBC")
  printfn "%A" (run parseLowercase "Abc")
  printfn "%A" (run parseDigit "abc123")
  printfn "%A" (run parseDigit "123abc")
  
  printfn "%A" (run parseLowercase "@abc123")
  printfn "%A" (run parseDigit "@123abc")
  0 // return an integer exit code
