module GameEngine.World

//type WorldManager(init) =
//  let agent = MailboxProcessor.Start(fun inbox ->
//    let rec loop counter = async {
//      let! mail = inbox.Receive()
//      printfn "counter: %i msg: %A" counter mail
//      return! loop (counter + 1)
//    }
//    loop (init)
//    )
//  member this.Log msg = agent.Post msg
