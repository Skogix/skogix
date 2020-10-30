module GameEngine.State

open System
open System.Collections
open GameEngine.Common

let makeTask logger taskId = async {
  let name = sprintf "Task%i" taskId
  for i in [1..10] do
    let msg = sprintf "%s:loop%i;" name i
    logger msg
}

type IntState = {
  State: int
}
type IntEvent =
  | Add of int
  | Print
type WorldManager(initWorld:IntState) =
  let agent = MailboxProcessor.Start(fun inbox ->
    let rec loop worldState = async {
      let! mail = inbox.Receive()
      match mail with
      | Print ->
        printfn "%A" worldState
        return! loop worldState
      | Add i ->
        return! loop {worldState with State = worldState.State+i}
    }
    loop (initWorld))
  member this.Print() = agent.Post(Print)
  member this.Add(i) = agent.Post(Add i)
type ArrayListEvent =
  | Update of int * int
  | Print
let updateState (state:ArrayList) (position) (data) =
  state.[position] <- data
  state
type ArrayListManager(init) =
  let agent = MailboxProcessor.Start(fun inbox ->
    let rec loop (state:ArrayList) = async {
      let! mail = inbox.Receive()
      match mail with
      | Update (position, data)->
        return! loop(updateState state position data)
      | Print ->
        printfn "%A" state
        return! loop (state)
    }
    loop (init)
    )
  member this.Print() = agent.Post Print
  member this.Update(pos, data) = agent.Post (Update (pos, data))
type PrintAgent() =
  member this.Log msg = slowConsoleWrite msg
type AsyncPrintAgent() =
  let agent = MailboxProcessor.Start(fun inbox ->
    let rec loop () = async {
      let! mail = inbox.Receive()
      slowConsoleWrite mail
      return! loop ()
    }
    loop ())
  member this.Log msg = agent.Post msg
  