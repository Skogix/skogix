module GameEngine.SandBox

// mest basic agent som går att göra
let whileTrueAgent =
  MailboxProcessor.Start(fun inbox ->
    async{
      while true do
        let! msg = inbox.Receive()
        printfn "msg: %s" msg
    })
type AsyncResponseMsg = string * AsyncReplyChannel<string>
let replyAsyncAgent =
  MailboxProcessor<AsyncResponseMsg>.Start(fun inbox ->
    let rec loop () =
      async {
        let! (msg, replyChannel) = inbox.Receive()
        replyChannel.Reply (sprintf "tog emot: %s" msg)
        do! loop ()
      }
    loop ())
