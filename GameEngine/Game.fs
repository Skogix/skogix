module GameEngine.Game
open Agents
open GameEngine
open GameEngine.Domain
open GameEngine.Common
open GameEngine.GameInit
open SkogixIO
type Skogix (skogixIO:SkogixIO.SkogixIO) =
  member this.world = WorldAgent(initWorld)
  member this.output = skogixIO.outputAgent
  member this.command = CommandAgent (skogixIO.outputStream, this.world)
  member this.Command x = this.command.agent.Post (OnlyCommand x)
  member this.CommandAndReply x = this.command.agent.PostAndReply (fun rc -> CommandAndReply (x, rc))
  member this.input = MailboxProcessor<Command>.Start(fun inbox ->
    let debug1 str x = skogixIO.debug (sprintf "Skogix::: %s %A" str x)
    let debug str = skogixIO.debug (sprintf "Skogix::: %s" str)
    let rec loop () = async {
      let! mail = inbox.Receive()
      let reply = this.CommandAndReply mail
//      this.Command mail
      skogixIO.debug reply
      return! loop ()
    }
    loop ()
    )
