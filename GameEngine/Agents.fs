module GameEngine.Agents
open Common
open GameEngine
open GameEngine.Domain
open GameEngine.GameInit
type SendCommand =
  | OnlyCommand of Command
  | CommandAndReply of Command * AsyncReplyChannel<string>
let CommandAgent(debug:(string -> unit))= MailboxProcessor<SendCommand>.Start(fun inbox ->
  let rec loop () = async {
    let! mail = inbox.Receive()
    match mail with
    | OnlyCommand c ->
      debug "Fick OnlyCommand"
    | CommandAndReply (c, rc) ->
      debug "Fick CommandAndReply"
      rc.Reply "Svar på CommandAndReply"
    return! loop ()
  }
  loop ()
  )

