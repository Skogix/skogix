module ConsoleOutput.Ui

open GameEngine.Domain

let GameState (input:GameState) = printfn "%A" input
let Debug (input:string) = printfn "Debug: %A" input
