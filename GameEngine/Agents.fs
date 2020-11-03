module GameEngine.Agents
open Common
open GameEngine
open GameEngine.Domain
open GameEngine.GameInit
  
type WorldCommand =
  | Get
  | UpdatePlayer of Player
type WorldAgent(initWorld:World) =  
  member this.agent = MailboxProcessor<WorldCommand * AsyncReplyChannel<World>>.Start(fun inbox ->
    let rec loop (state:World) = async {
      let! mail, rc = inbox.Receive()
      match mail with
      | Get -> rc.Reply(state)
      | UpdatePlayer x ->
        printfn "Upddaterar player"
        printfn "från: %A" state
        let newGameState = {state.Game with Player = x}
        let newWorld = {state with Game = newGameState}
        
        printfn "till %A" newWorld
        rc.Reply(newWorld)
        return! loop (newWorld)
      return! loop (state)
    }
    loop initWorld
    )

let skogixMove dir pos =
  match dir with
  | Up -> {pos with y = pos.y - 1}
  | Down -> {pos with y = pos.y + 1} 
  | Left -> {pos with x = pos.x - 1}
  | Right -> {pos with x = pos.x + 1}
type CommandAgent(output:OutputStream, worldAgent:WorldAgent)=
  let worldpostandreply x = worldAgent.agent.PostAndReply (fun rc -> (x, rc))
  member this.agent = MailboxProcessor<InputCommand * AsyncReplyChannel<World>>.Start(fun inbox ->
    let rec loop () = async {
      let! mail, rc = inbox.Receive()
      match mail with
        | PrintWorldState ->
          let reply = worldAgent.agent.PostAndReply (fun rc -> (Get, rc))
          rc.Reply(reply)
        | Move dir ->
          let world = worldpostandreply Get
          let newPos = skogixMove dir world.Game.Player.Position
          let newPlayer = {world.Game.Player with Position = newPos}
          let newWorld = worldpostandreply (UpdatePlayer newPlayer)
          rc.Reply(newWorld)
      return! loop ()
    }
    loop ()
    )


