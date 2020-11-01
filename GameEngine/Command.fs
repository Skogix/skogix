module GameEngine.Command


type Result<'Ok> =
  | Ok of 'Ok
  | Fail of string
type CommandMessage<'a> = Reply of AsyncReplyChannel<'a> | Run
type Command<'a, 'b, 'c>() =
  static let mutable history = []
  static let mutable commands = Map.empty<string, ('a -> 'b -> 'c)>
  static member private agent = MailboxProcessor<CommandMessage<'c> * string * 'a * 'b>.Start(fun inbox ->
    let rec loop () = async {
      let! action, command, a, b = inbox.Receive()
      
      return! loop ()
    }
    loop())