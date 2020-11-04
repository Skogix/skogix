module GameEngine.Game
open System.Threading
open GameEngine
open GameEngine.Agents
open GameEngine.Domain
open GameEngine.SkogixIO
open GameEngine.Common

type Skogix (skogixIO:SkogixIO) =
  member this.skogixIO = skogixIO
  member this.stateManager = StateManager(stateInit)
  member this.sendInputCommandToInputManager x = InputManager(this.stateManager).inputAgent.PostAndReply (fun rc -> (rc, x))
  member this.sendStateCommandToStateManager x = this.stateManager.stateAgent.PostAndReply (fun rc -> (rc, x))
  member this.agent = MailboxProcessor<InputCommand>.Start(fun inbox ->
    let rec loop () = async {
      let! mail = inbox.Receive()
      mail
      |> this.sendInputCommandToInputManager
      |> this.sendStateCommandToStateManager
      |> skogixIO.outputAgent.Post
      return! loop ()
    }
    loop ()
    )
//type Skogix (skogixIO:SkogixIO.SkogixIO) =
//  member this.world = WorldAgent(initWorld)
//  member this.output = skogixIO.outputAgent
//  member this.command = CommandAgent (skogixIO.outputStream, this.world)
//  member this.CommandAndReply x = this.command.agent.PostAndReply (fun rc -> (x, rc))
//  member this.input = MailboxProcessor<InputCommand>.Start(fun inbox ->
//    let debug1 str x = skogixIO.debug (sprintf "Skogix::: %s %A" str x)
//    let debug str = skogixIO.debug (sprintf "Skogix::: %s" str)
//    let rec loop () = async {
//      let! mail = inbox.Receive()
//      mail
//      |> this.CommandAndReply
//      |> Async.RunSynchronously
//      |> printfn "%A"
//      let reply = this.CommandAndReply mail
////      this.Command mail
//      skogixIO.outputStream.Renderer reply
//      return! loop ()
//    }
//    loop ()
//    )
