module Game.Domain

open GameEngine.GameEngine


type Direction =
  | Up
  | Down
  | Left
  | Right
type InputCommand =
  | Wait
  | Move of Direction

type GameState = {
  Entities: Entity list
}
type World =
  | GameWorld of GameState
type OutputStream = {
  OutputStateStream: (World -> unit)
  OutputCommands: (InputCommand list -> InputCommand)
}
