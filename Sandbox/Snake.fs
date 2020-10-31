namespace Snake
module Core =
  type Position = {x:int; y:int}
  type Snake = Position list
  type Direction = Up | Down | Left | Right
  type Config = {
    startPosition: Position
    startDirection: Direction
  }
  type GameInit = {
    initPos: Position
    initDir: Direction
  }
  type Action =
    | Move
    | ChangeDirection
  type Renderer = {redraw: Snake -> unit}
  type MoveHead = Position -> Direction -> Snake
  type GetTail = Snake -> Snake // oklart om det behövs
//  kolla typen på en mailboxprocessor
//  type CreateGame = GameInit -> GameAgent
//  kolla typen på commandstream
//  borde returna någon sorts gamestate, running/gameover osv
//  type StartGame = Config -> Renderer -> CommandStream -> unit
  
module Game =
  open Core
  let createGame (init:GameInit) =
    /// gameagent -> gameagent
    /// mailbox action
    /// rec loop snake dir
    /// matcha action
    ///  move
    ///   tail = snake
    ///   newHead = moveHead pos dir
    ///   rec newSnake::tail-1 dir
    ///  changeDirection dir
    ///   rec dir
    0
  let startGame (config:Config) (renderer:Renderer) (commandStream) =
    /// set gameInit
    /// get gameAgent från createGame
    /// rec loop
    ///  async.sleep x
    ///  gameAgent.post move
    /// run loop
    /// subscribe commandstream till gameAgent.post
    0
  