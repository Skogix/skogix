﻿namespace Snake
module Core =
  type Position = {x:int; y:int}
  type Snake = Position list
  type Direction = Up | Down | Left | Right
  type Config = {
    startPosition: Position
    startDirection: Direction
  }
  type Renderer = Snake -> unit
  type GameInit = {
    initSnake: Snake
    initDir: Direction
    renderer: Renderer
  }
  type HasEaten = bool
  type Action =
    | Move
    | ChangeDirection of Direction
    | AddTail
  type MoveHead = Position -> Direction -> Snake
  type GetTail = Snake -> Snake // oklart om det behövs
  type GetHead = Snake -> Position
//  kolla typen på en mailboxprocessor
//  type CreateGame = GameInit -> GameAgent
//  kolla typen på commandstream
//  borde returna någon sorts gamestate, running/gameover osv
//  type StartGame = Config -> Renderer -> CommandStream -> unit
  
module Game =
  open Core
  let getNewPos (pos:Position) (dir:Direction) =
    match dir with
    | Up -> {pos with y = pos.y - 1}
    | Down -> {pos with y = pos.y + 1}
    | Left -> {pos with x = pos.x - 1}
    | Right -> {pos with x = pos.x + 1}
  let getHead (snake:Snake): Position = snake.Head
  let getTail (snake:Snake) (hasEaten:HasEaten): Snake =
    match hasEaten with
    | true -> snake
    | false -> List.take (snake.Length - 1) snake
  let moveSnake (snake:Snake) (dir:Direction) (hasEaten:HasEaten): Snake =
    let head = getHead snake
    let tail = getTail snake hasEaten
    let newHead =
      match dir with
      | Up -> getNewPos head Up
      | Down -> getNewPos head Down
      | Left -> getNewPos head Left
      | Right -> getNewPos head Right
    newHead::tail
  let createGame (init:GameInit) =
    let gameAgent =
      MailboxProcessor.Start(fun inbox ->
        let rec loop (snake:Snake) (dir:Direction) = async {
          init.renderer snake
          let! action = inbox.Receive()
          match action with
          | Move ->
            printfn "move"
            let newSnake = moveSnake snake dir false
            return! loop newSnake dir
          | ChangeDirection newDir ->
            printfn "changedir: %A" newDir
            return! loop snake newDir
          | AddTail ->
            printfn "addtail"
            let newSnake = moveSnake snake dir true
            return! loop newSnake dir
            
        }
        loop init.initSnake init.initDir
        )
    gameAgent
  let startGame (config:Config) (renderer:Renderer) (commandStream) =
    /// set gameInit
    let gameInit = {
      initSnake = [config.startPosition]
      initDir = config.startDirection
      renderer = renderer }
    /// get gameAgent från createGame
    let gameAgent = createGame gameInit
    /// rec loop
    let rec gameLoop () =
      async {
    ///  async.sleep x
        do! Async.Sleep 500
    ///  gameAgent.post move
        gameAgent.Post Move
        return! gameLoop ()
      }
    gameLoop () |> Async.StartImmediate
    /// run loop
    /// subscribe commandstream till gameAgent.post
    commandStream
    |> Observable.subscribe gameAgent.Post
  