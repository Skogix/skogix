module GameEngine.Agents
open System
open System.Runtime.InteropServices.WindowsRuntime
open GameEngine.Domain
  
type StateCommand =
  | CommandAddOneToPlayer
let addOneToPlayer = sprintf "ADDAR ON TILL PLAYER"
let updatePlayerCount (old:Player) x = {old with Count = x}
type StateManager(initState:State) =
  member this.stateAgent = MailboxProcessor<AsyncReplyChannel<State> * StateCommand>.Start(fun inbox ->
    let rec loop (state:State) = async {
      let! rc, mail = inbox.Receive()
      match mail with
      | addOneToPlayer ->
        let newPlayer = {state.Game.Player with Count = state.Game.Player.Count + 1}
        printfn "player: %A" newPlayer
        let newGame = {state.Game with Player = newPlayer}
        printfn "game: %A" newGame
        let newState = {state with Game = newGame}
        printfn "state: %A" newState
        return! loop newState
      return! loop state
    }
    loop initState
    )
 
type InputManager(stateManager:StateManager) =
  member this.inputAgent = MailboxProcessor<AsyncReplyChannel<StateCommand> * InputCommand>.Start(fun inbox ->
    let rec loop () = async {
      let! rc, mail = inbox.Receive()
      match mail with
      | InputAddOne -> rc.Reply StateCommand.CommandAddOneToPlayer
      return! loop ()
    }
    loop ()
    )
  member this.PostAndReply x = this.inputAgent.PostAndReply (fun rc -> (rc, x))
 
 