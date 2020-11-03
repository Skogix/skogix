module GameEngine.Domain
open Skogix.Core
type Position = {x:int;y:int}
type Player = {
  Position: Position
}
type GameState = {
  Player: Player
}
type World = {
   Game: GameState
   }
type Direction =
  | Up
  | Down
  | Left
  | Right
type InputCommand =
  | Move of Direction
  | PrintWorldState
type InputState = InputCommand list
type AcceptedInputs = InputState list
type OutputState = {
  World: World
  AcceptedInputs: AcceptedInputs
}
type OutputStream = {
  Renderer: (World -> unit)
  Debug: (string -> unit)
  InputFunctions: (InputCommand list -> unit)
}
