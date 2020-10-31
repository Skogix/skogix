module GameEngine.State
open Domain
open GameEngine
let StateManager init =
  let stateAgent = MailboxProcessor<World>.Start(fun inbox ->
    let rec loop (state) = async {
      let newState = inbox.Receive()
      printf "newState: %A" newState
      return! loop (state)
    }
    loop (init)
    )
  stateAgent