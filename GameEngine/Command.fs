module GameEngine.Command

open GameEngine
open GameEngine
open GameEngine.Domain
let commands = ["Move Up"; "Move Down"]
let getCommand input =
  let tryFind =
    commands
    |> List.tryFind(fun command -> input = command)
  tryFind
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
