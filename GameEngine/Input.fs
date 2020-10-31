module GameEngine.Input
open GameEngine.Domain

let Input =
  let inputAgent = MailboxProcessor<Command>.Start(fun inbox ->
    let rec loop () = async {
      let! input = inbox.Receive()
      printfn "Input: %A" input
      return! loop ()
    }
    loop ()
    )
  inputAgent

