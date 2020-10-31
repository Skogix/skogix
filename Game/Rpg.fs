module Game.Rpg

open System

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
type WorldState =
  | Running of GameState
  | Paused
type Direction =
  | Up
  | Down
  | Left
  | Right
type Input =
   | Move of Direction
type Amount = int
type Tile = {
  Graphic: Graphic
  Position: Position
}
type Renderer = {
  PrintTileMap: (Tile list -> unit)
  PrintDebug: (string -> unit)
}

let InputManager =
  let inputAgent =
    MailboxProcessor<Input>.Start(fun inbox ->
      let rec loop() = async {
        let! action = inbox.Receive()
        match action with
        | Move d ->
          printfn "move: %A" d
          return! loop()
      }
      loop()
      )
  inputAgent
let StartGame (renderer:Renderer) (commandStream) =
  let initPlayer = { Name="Skogix"
                     Glyph = '@'
                     Position = {x=3;y=3}}
  commandStream
  |> Observable.subscribe InputManager.Post |> ignore
  0