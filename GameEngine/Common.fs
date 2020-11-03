module GameEngine.Common
open Domain

let inputFunctionInit: InputCommand list = [
  Move Up
  Move Down
  Move Left
  Move Right
]
let playerStateInit: Player = {
  Position = {x=3;y=3}
}
let gameStateInit: GameState = {
  Player = playerStateInit
}
let initWorld: World = {
  Game = gameStateInit
}
let debug = printfn "GameEngine::: %s"


