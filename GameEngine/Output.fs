module GameEngine.Output

open GameEngine.Domain

let Output(outputStream: OutputStream) =
  let outputAgent =
    MailboxProcessor<OutputStream>.Start(fun inbox ->
      let rec loop() = async {
        let! output = inbox.Receive()
        printfn "output: %A" output
        return! loop()
      }
      loop()
      )
  outputAgent