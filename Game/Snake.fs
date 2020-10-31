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
    initSnake: Snake
    initDir: Direction
  }
  type Action =
    | Move
    | ChangeDirection
  type Renderer = Snake -> unit
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
  let getPos (pos:Position) (dir:Direction) = pos
  let getHead (snake:Snake): Position = snake.Head
  let moveSnake (snake:Snake) (dir:Direction): Snake = snake
  let createGame (init:GameInit) =
    /// gameagent -> gameagent
    let gameAgent =
      MailboxProcessor.Start(fun inbox ->
        let rec loop (snake:Snake) (dir:Direction) = async {
          let! action = inbox.Receive()
          match action with
          | Move ->
            printfn "Move"
            let newSnake = moveSnake snake dir
    ///   rec newHead::tail-1 dir
            return! loop newSnake dir
    ///  todo changeDirection dir
          | _ -> return! loop snake dir
        }
        loop init.initSnake init.initDir
        )
    gameAgent
  let startGame (config:Config) (renderer:Renderer) (commandStream) =
    /// set gameInit
    let gameInit = {
      initSnake = [config.startPosition]
      initDir = config.startDirection
    }
    /// get gameAgent från createGame
    let gameAgent = createGame gameInit
    /// rec loop
    let rec gameLoop () =
      async {
    ///  async.sleep x
        do! Async.Sleep 1000
    ///  gameAgent.post move
        printfn "Skickar move"
        gameAgent.Post Move
        return! gameLoop ()
      }
    gameLoop () |> Async.StartImmediate
    /// run loop
    /// subscribe commandstream till gameAgent.post
    0
  