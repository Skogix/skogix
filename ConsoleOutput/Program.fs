// Learn more about F# at http://fsharp.org

open System
open Parser
open Basics

[<EntryPoint>]
let main _ =
  Console.Clear()
  let a = 'a'
  let b = 'b'
  let c = 'c'
  
  let parseA = parseChar a
  let parseB = parseChar b
  let parseC = parseChar c
  
  let abc = "abc"
  let baba = "baba"
  
  printfn "char: %c input: %s output: %A" a abc (run parseA abc)
  printfn "char: %c input: %s output: %A" b abc (run parseB abc)
  printfn "char: %c input: %s output: %A" c abc (run parseC abc)
  printfn "char: %c input: %s output: %A" b baba (run parseB baba)
  0 // return an integer exit code
