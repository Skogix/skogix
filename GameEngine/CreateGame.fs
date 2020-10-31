module GameEngine.CreateGame
open System
open System.ComponentModel.Design
open Domain
open GameEngine
open Output
open Input
open GameEngine

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
let createGame output =
  Input.Post
//type Game (outputStream:OutputStream) =
//  static member Output = Output.Output
//  member this.Start =
//    Observable.subscribe Input.Input.Post
//  let sendInput, inputstream =
//    let event = Event<_>()
//    event.Trigger, event.Publish
  