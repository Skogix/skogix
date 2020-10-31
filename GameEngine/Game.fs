module GameEngine.CreateGame
open System
open System.ComponentModel.Design
open Domain

let PlayerInit:Player = {
  Name = "Skogix"
  Position = {x=3;y=3}
  Glyph = '@'}
let GameStateInit:GameState = {
  Player = PlayerInit
}
let WorldInit: World = {
  Game = GameStateInit
}

type Game(inputStream, outputStream:OutputStream) =
  member this.Output = outputStream
  member this.Input = inputStream
  member this.Start =
    inputStream
    |> Observable.subscribe Input.Input.Post
