module GameEngine.Domain
type Result<'yay, 'nay> =
  | Yay of 'yay 
  | Nay of 'nay
type Position = {x:int;y:int}
type Player = {
  Position: Position
}
type GameState = {
  Player: Player
}
type World =
  | Game of GameState
type Direction =
  | Up
  | Down
  | Left
  | Right
type Command =
  | Move of Direction
type InputState = Command list
type AcceptedInputs = InputState list
type OutputState = {
  World: World
  AcceptedInputs: AcceptedInputs
}