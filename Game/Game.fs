namespace Game
open System.Collections
open GameEngine
open GameEngine.IO

module ComponentsModule =
  type Position = {x:int; y:int}
  type Player = {name:string}
type GameWorld = {
  Player: ComponentsModule.Player
}
type World =
  | GameWorld of GameWorld
module SystemsModule =
  open ComponentsModule
  type Direction =
    | Up
    | Down
    | Left
    | Right
  let moveSystem dir pos =
    match dir with
    | Up -> {pos with y = pos.y - 1}
    | Down -> {pos with y = pos.y + 1}
    | Left -> {pos with x = pos.x - 1}
    | Right -> {pos with x = pos.x + 1}
  let getSystem str =
    match str with
    | "Move Up" -> (moveSystem Up)
    | "Move Down" -> (moveSystem Down)
    | "Move Right" -> (moveSystem Right)
    | "Move Left" -> (moveSystem Left)
module InitModule =
  open ComponentsModule
  let playerComponent = {name = "Skogix"}
  let gameDebug: OutputStream= { Debug = printfn "%A"
                                 GameState = printfn "%A"}
  let gameInit = GameEngine.IO.getInit(gameDebug)
  let move (pos:Position): Position =
    printfn "PosSystem: %A" pos 
    pos
  gameInit.addComponentType<Position>()
  gameInit.addComponentType<Player>()
  gameInit.addWorld<GameWorld>()
//  gameInit.addSystem<Position, Position>("name", move)
  printfn "Components: %A" gameInit.components
  printfn "Worlds: %A" gameInit.worlds
//  gameInit.addComponent<Position>()
//  gameInit.addComponent<Player>()
//  gameInit.addSystem("HelloWorld", (fun x -> printfn "%A" x))
  // adda skit spelet behöver till skogix
module GameModule =
  type Game(init:Init) =
    member this.huhu = init.worlds
    member this.sendInputToEngine = IO.IO(init).input
    member this.input<'component> (str:string) =
      let huhu = SystemsModule.getSystem str
      huhu
  let getGame(init:Init) = Game(init)
module Input =
  let getInput(outputStream:OutputStream) = 0

