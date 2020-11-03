module GameEngine.SkogixIO

open GameEngine.Domain

type SkogixIO(outputStream:OutputStream) =
  let OutputAgent = MailboxProcessor.Start(fun inbox ->
    let rec loop () = async {
      let! mail = inbox.Receive()
      printfn "Output: %A" mail
      return! loop ()
    }
    loop ()
    )
  member this.outputStream = outputStream
  member this.outputAgent = OutputAgent
  member this.debug = outputStream.Debug
let getSkogixIO(outputStream:OutputStream) = SkogixIO(outputStream)