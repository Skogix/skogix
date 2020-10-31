namespace GameEngine

open System

module Domain =

  type Command = string
  type Position = {x:int;y:int}
  type Player = {
    Name: string
    Position: Position
    Glyph: char
  }
  type GameState ={
    Player: Player
  }
  type World = {
    Game: GameState
  }
  type InputStream = IObservable<string>
  type OutputStream = {
    PrintGameState: (GameState -> unit)
    PrintDebugInformation: (string -> unit)
  }