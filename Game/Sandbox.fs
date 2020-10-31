module Game.Sandbox

open System.Collections.Generic
open System.Security.Cryptography

type CounterMessage =
  | Increment
  | Decrement
/// bara sparar i loopen
//let createProcessor initState =
//  MailboxProcessor<CounterMessage>.Start(fun inbox ->
//    let rec loop state = async {
//      let! message = inbox.Receive()
//      match message with
//      | Increment ->
//        let newState = state + 1
//        printfn "plus, state: %A" newState
//        return! loop newState
//      | Decrement ->
//        let newState = state - 1
//        printfn "minus, state: %A" newState
//        return! loop newState
//    }
//    loop initState
//    )

//let processor = createProcessor 0
//[ async { processor.Post(Increment) }
//  async { processor.Post(Decrement) }
//  async { processor.Post(Decrement) }
//  async { processor.Post(Decrement) } ]
//|> Async.Parallel
//|> Async.RunSynchronously

/// mutable state, inte threadsafe
//let createProcessor initState =
//  MailboxProcessor<CounterMessage>.Start(fun inbox ->
//    let mutable state = initState
//    let rec loop () = async {
//      let! mail = inbox.Receive()
//      match mail with
//      | Increment ->
//        state <- state + 1
//        printfn "plus, state: %i" state
//        return! loop()
//      | Decrement ->
//        state <- state - 1
//        printfn "minus, state: %i" state
//        return! loop()
//    }
//    loop()
//    )
//let processor = createProcessor 0
//[ async { processor.Post(Increment) }
//  async { processor.Post(Increment) }
//  async { processor.Post(Decrement) }
//  async { processor.Post(Decrement) } ]
//|> Async.Parallel
//|> Async.RunSynchronously |> ignore
