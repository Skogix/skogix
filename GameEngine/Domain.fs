module GameEngine.Domain
type Player = {
  Count: int
}
type Game = {
  Player: Player
}
type InputCommand =
  | InputAddOne 
type InputCommands = InputCommand List
type OutputStream = {
  Game: (Game -> unit)
  Debug: (string -> unit)
  InputFunctions: (InputCommands -> unit)
}
type State = {
  Game: Game
}