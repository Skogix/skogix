module GameEngine.Common
open Domain

let inputFunctionInit: InputCommands= [
  InputAddOne
]
let playerStateInit: Player = {
  Count = 0
}
let gameInit: Game = {
  Player = playerStateInit
}
let stateInit:State = {
  Game = gameInit
}
let debug = printfn "GameEngine::: %s"


