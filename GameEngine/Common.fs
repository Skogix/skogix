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
let gameStateInit: Game = {
  Player = playerStateInit
}
let initWorld: World = {
  Game = gameStateInit
  Id = 0
}
let debug = printfn "GameEngine::: %s"


