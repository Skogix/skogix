module GameEngine.Domain
open Skogix.Core
type Position = {x:int;y:int}
type Player = {
  Position: Position
}
type GameState = {
  Player: Player
}
type World =
  | GameState of GameState
type Direction =
  | Up
  | Down
  | Left
  | Right
type Command =
  | Move of Direction
  | Print of string
type InputState = Command list
type AcceptedInputs = InputState list
type OutputState = {
  World: World
  AcceptedInputs: AcceptedInputs
}
type OutputStream = {
  Renderer: (World -> unit)
  Debug: (string -> unit)
  InputFunctions: (Command list -> unit)
}
