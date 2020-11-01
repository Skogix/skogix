namespace GameEngine

open System

module Domain =

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
  type Output =
    | GameState of GameState
    | Debug of string
  type OutputStream = {
    GameState: (GameState -> unit)
    Debug: (string -> unit)
  }
