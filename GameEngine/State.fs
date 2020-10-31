module GameEngine.State
open Domain
open CreateGame
let stateAgent(outputStream:OutputStream) = MailboxProcessor<World>.Start(fun inbox ->
  let rec loop (state:World) = async {
    let newState = inbox.Receive()
    outputStream.Debug (sprintf "stateagent newstate: %A" newState)
    return! loop (state)
  }
  loop (WorldInit)
  )