module Sandbox.CommandBackup



type SkogixResult<'Ok> =
  | Ok of 'Ok
  | Fail of string
  
type CommandMessage<'a> = Reply of AsyncReplyChannel<'a> | Run
type CommandHistory<'a,'b,'c> = ('a * 'b * 'c) list
type Command<'a, 'b, 'c>() =
//  static let history = List<'a * 'b * ('a -> 'b -> 'c)>(100)
  static let mutable history = []
  static let mutable commands = Map.empty<string, ('a -> 'b -> 'c)>
  static member private Agent = MailboxProcessor<CommandMessage<'c> * string * 'a * 'b>.Start(fun inbox ->
    let rec loop () = async {
      let! action, command, a, b = inbox.Receive()
      match commands.TryFind(command) with
      | Some f ->
        history <- [(a,b,f)]::history
        match action with
        | Run -> f a b |> ignore
        | Reply rc ->
          rc.Reply (f a b)
      | None ->
        match action with
        | Run ->  printfn "Hittade inte commandet: %s" command
        // todo borde använda option
        | Reply rc ->
          rc.Reply(Unchecked.defaultof<'c>)
      return! loop ()
    } loop ())
  static member AddCommand (str:string) (f:('a -> 'b -> 'c)) = commands <- commands.Add (str, f)
  static member Post (command:string) (a:'a) (b:'b) = Command<'a,'b,'c>.Agent.Post (Run, command, a, b)
  static member PostAndReply (command:string) (a:'a) (b:'b):'c option =
    match commands.TryFind command with
    | Some x -> Some (Command<'a,'b,'c>.Agent.PostAndReply (fun rc -> (Reply rc, command, a, b)))
    | None -> None
  static member PostAndReplyAsync (command:string) (a:'a) (b:'b) =
    match commands.TryFind command with
    | Some x ->
      let huhu = Some (Command<'a,'b,'c>.Agent.PostAndAsyncReply (fun rc -> (Reply rc, command, a, b)))
      Ok (huhu)
    | None -> Fail "huhu"
type Command<'a> = Command<'a,'a,'a>
type Command<'a,'b> = Command<'a,'b, unit>
