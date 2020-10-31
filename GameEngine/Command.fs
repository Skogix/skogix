module GameEngine.Command

open GameEngine
open GameEngine
open GameEngine.Domain

type Direction =
  | Up
  | Down
  | Left
  | Right
let MoveUp = printfn "Kör funktionen moveup"
let commands = [("Move Up", MoveUp)]
let getCommand str =
  commands
  |> List.tryFind (fun cmd -> str.ToString() = fst cmd)
// todo till imorgon, använd id istället för position
// todo verkar bli entities istället ändå -.-
let commandAgent =
  MailboxProcessor<Command>.Start(fun inbox ->
    let rec loop() = async {
      let! input = inbox.Receive()
      match getCommand input with
      | Some command -> printfn "hittade command: %A" command
      | None -> ()     
      return! loop()
    }
    loop()
    )
