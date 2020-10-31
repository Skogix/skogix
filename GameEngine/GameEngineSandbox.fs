module GameEngine.GameEngineSandbox


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
        replyChannel.Reply (sprintf "svarar på: %s" msg)
        do! loop ()
      }
    loop ())
// måste få ett Hello för att svara
let whileTrueAgentWithScan =
  MailboxProcessor.Start(fun inbox ->
    async {
      while true do
        do! inbox.Scan (fun hello ->
          match hello with
          | "Hello" -> Some(async{printfn "hello back"})
          | _ -> None
          )
        let! msg = inbox.Receive()
        printfn "vanligt msg: %s" msg
    }
    )
let createTimer interval eventHandler =
  let timer = new System.Timers.Timer(float interval)
  timer.AutoReset <- true
  timer.Elapsed.Add eventHandler
  async {
    timer.Start()
    do! Async.Sleep 10000
    timer.Stop()
  }
let createTimerAndObservable interval =
  let timer = new System.Timers.Timer(float interval)
  timer.AutoReset <- true
  let observable = timer.Elapsed
  let task = async {
    timer.Start()
    do! Async.Sleep 10000
    timer.Stop()
  }
  (task, observable)
