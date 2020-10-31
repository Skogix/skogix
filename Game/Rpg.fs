module Game.Rpg

open System
open System.Diagnostics.Tracing

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
type WorldState = {
  GameState:GameState
  PausedState:PausedState
}
type OutputState =
  | Game of GameState
  | Pause of PausedState
type Direction =
  | Up
  | Down
  | Left
  | Right
type Command =
  | Move of Direction
type Input =
  | Move of Direction
  | Pause
type Amount = int
type Tile = {
  Graphic: Graphic
  Position: Position
}
type Renderer = {
  PrintTileMap: (Tile list -> unit)
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
let initWorldState: WorldState= { GameState = gameState
                                  PausedState = pauseState}
let EventManager =
  let eventAgent =
    MailboxProcessor<Event>.Start(fun inbox ->
      let rec loop state = async {
        let! event = inbox.Receive()
        printfn "event: %A" event
        do! loop state
      }
      loop initWorldState)
  eventAgent
let ( >>> ) x y = EventManager.Post (x y)
let PlayerManager =
  let playerAgent =
    MailboxProcessor<Command>.Start(fun inbox ->
      let rec loop player = async {
        let! (command:Command) = inbox.Receive()
        match command with
        | Command.Move dir ->
          printfn "command player move dir: %A" dir
          PlayerUpdate >>> player 
        return! loop player 
      }
      loop initPlayer)
  playerAgent
let InputManager =
  let inputAgent =
    MailboxProcessor<Input>.Start(fun inbox ->
      let rec loop() = async {
        let! action = inbox.Receive()
        match action with
        | Move d -> PlayerManager.Post (Command.Move d)
        | Pause -> ()
        return! loop()
      }
      loop())
  inputAgent
let StartGame (renderer:Renderer) (commandStream) =
  commandStream
  |> Observable.subscribe InputManager.Post |> ignore
  0