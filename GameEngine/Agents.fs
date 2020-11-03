module GameEngine.Agents
open Common
open GameEngine
open GameEngine.Domain
open GameEngine.GameInit
type SendCommand =
  | OnlyCommand of Command
  | CommandAndReply of Command * AsyncReplyChannel<string>
let cmd (command:Command) =
  match command with
  | Print x -> x
  | Move x -> (sprintf "MoveCommand %A körs" x)
  
  
  
type WorldAgent(initWorld:World) =  
  member this.agent = MailboxProcessor<string * AsyncReplyChannel<string>>.Start(fun inbox ->
    let rec loop (state:World) = async {
      let! mail, rc = inbox.Receive()
      match mail with
      | "print" ->
        rc.Reply(state.ToString())
      | _ ->
        rc.Reply(state.ToString())
      return! loop state 
    }
    loop initWorld
    )


type CommandAgent(output:OutputStream, worldAgent:WorldAgent)=
  member this.agent = MailboxProcessor<SendCommand>.Start(fun inbox ->
    let rec loop () = async {
      let! mail = inbox.Receive()
      match mail with
      | OnlyCommand c -> 
        output.Debug "Fick OnlyCommand"
      | CommandAndReply (c, rc) ->
        output.Debug "Fick CommandAndReply"
        match c with
        | PrintWorldState ->
          let reply = worldAgent.agent.PostAndReply (fun rc -> ("print", rc))
          rc.Reply(reply)
        | Print x ->
          rc.Reply(cmd (Print x))
        | Move x ->
          rc.Reply(cmd (Move x))
      return! loop ()
    }
    loop ()
    )


