module Sandbox.Rpg1


type Position = {x:int;y:int}
type Name = string
type Graphic = char
type Player = {
  Name: Name
  Glyph: Graphic
  Position: Position
}
type GameState = {
  Player: Player
}
type PausedState = {
  Message: string
}
type OutputState =
  | GameOutput of GameState
  | PauseOutput of PausedState
type WorldState = {
  GameState:GameState
  PausedState:PausedState
  OutputState:OutputState
}
type Direction =
  | Up
  | Down
  | Left
  | Right
type Command =
  | Move of Direction
type Input =
  | MovePlayer of Direction
  | Pause
type Amount = int
type Tile = {
  Graphic: Graphic
  Position: Position
}
type Renderer = {
  PrintTileMap: (GameState -> unit)
  PrintDebug: (string -> unit)
}
let initPlayer:Player = { Name = "Skogix"
                          Glyph = '@'
                          Position = {x=3;y=3}}
type Event =
  | PlayerUpdate of Player
let gameState:GameState =
  let initPlayer: Player = { Name = "Skogix"
                             Glyph = '@'
                             Position = {x=3;y=3}}
  { Player = initPlayer }
let pauseState:PausedState = { Message = "Paused" }
let initWorldState: WorldState = { GameState = gameState
                                   PausedState = pauseState
                                   OutputState = GameOutput gameState}
let createGame renderer =
  let OutputManager =
    let outputAgent =
      MailboxProcessor<OutputState>.Start(fun inbox ->
        let rec loop() = async {
          let! output = inbox.Receive()
          match output with
          | GameOutput state -> renderer.PrintTileMap state
          | PauseOutput message -> ()
          return! loop()
        }
        loop()
        )
    outputAgent
  let output (output:OutputState) = OutputManager.Post (output)
  let StateManager =
    let stateAgent =
      MailboxProcessor.Start(fun inbox ->
        let rec loop (state:WorldState) = async {
          let! (event:Event) = inbox.Receive()
          match event with
          | PlayerUpdate p ->
            let newGameState = {state.GameState with Player = p}
            let newState = {state with GameState = newGameState }
            output (GameOutput newState.GameState)
            return! loop newState
          do! loop state
        }
        loop initWorldState)
    stateAgent
  let EventManager =
    let eventAgent =
      MailboxProcessor<Event>.Start(fun inbox ->
        let rec loop ()= async {
          let! event = inbox.Receive()
          StateManager.Post event
          do! loop ()
        }
        loop ())
    eventAgent
  let ( -.- ) x y = EventManager.Post (x y)
  let getPos pos dir =
    match dir with
    | Up -> {pos with y=pos.y-1}
    | Down -> {pos with y=pos.y+1}
    | Left -> {pos with x=pos.x-1}
    | Right -> {pos with x=pos.x+1}
  let PlayerManager =
    let playerAgent =
      MailboxProcessor<Command>.Start(fun inbox ->
        let rec loop (player:Player) = async {
          let! (command) = inbox.Receive()
          match command with
          | Move dir ->
            let newPos = getPos player.Position dir
            let newPlayer = {player with Position=newPos}
            PlayerUpdate -.- newPlayer
            return! loop newPlayer 
        }
        loop initPlayer)
    playerAgent
  let InputManager =
    let inputAgent =
      MailboxProcessor<Input>.Start(fun inbox ->
        let rec loop() = async {
          let! action = inbox.Receive()
          match action with
          | MovePlayer d -> PlayerManager.Post (Command.Move d)
          | Pause -> ()
          return! loop()
        }
        loop())
    inputAgent
  InputManager
let StartGame (renderer:Renderer) (commandStream) =
  let inputManager = createGame renderer
  
  commandStream
  |> Observable.subscribe inputManager.Post |> ignore
  0
