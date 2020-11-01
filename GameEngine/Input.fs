module GameEngine.Input
open GameEngine.Domain

let inputAgent(outputStream:OutputStream) = MailboxProcessor.Start(fun inbox ->
  let output = outputStream
  let rec loop () = async {
    let! input = inbox.Receive()
    output.Debug (sprintf "input: %A" input)
    
    return! loop ()
  }
  loop ()
  )
