module Game.Core
open System.Collections.Generic
open GameEngine.Domain
open Domain
type InputManager () =
  static let agent = MailboxProcessor.Start(fun inbox ->
    let rec loop counter = async {
      let! mail = inbox.Receive()
      printfn "counter: %i msg:%A" counter mail
      return! loop (counter+1)
    }
    loop 0
    )
  static member Move (dir:Direction) = agent.Post (Move dir)
  
type GenericListStateManager<'a> () =
  static let agent = MailboxProcessor<'a>.Start(fun inbox ->
    let rec loop (items:List<'a>) = async {
      let! mail = inbox.Receive()
      items.Add mail
      printfn "mail: %A\nState: %A" mail items
      return! loop (items)
    }
    loop (List<'a>())
    )
  static member Log<'a> msg = agent.Post msg
type Entity = int
type World = {
  Entities: Entity list
}
type WorldEvent =
  | UpdateState of (World -> World)
  | ResetState of World
  | EndGame
  | GetState 
type WorldManager(initState: World) =
  let agent = MailboxProcessor.Start(fun inbox ->
    let rec loop state = async {
      let! mail = inbox.Receive()
      match mail with
      | UpdateState updateFunc -> return! loop (updateFunc state)
      | ResetState newState -> return! loop newState
      | EndGame -> return ()
      | GetState -> return! loop state
    }
    loop initState
    )
  member this.Update(updateFunc) = agent.Post (UpdateState updateFunc)
  member this.ResetState(newState) = agent.Post (ResetState newState)
  member this.Update() = agent.Post EndGame
  member this.GetState(rc) = agent.PostAndReply rc
  
