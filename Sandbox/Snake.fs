namespace Sandbox.Snake
//module Core =
//  type Position = {x:int; y:int}
//  type Body = Position list
//  type Direction = Up | Down | Left | Right
//  type Config = {
//    startPosition: Position
//    startDirection: Direction
//  }
//  type Snake = {
//    body: Body
//    dir: Direction
//  }
//  type Renderer = {
//    GameState: (Snake -> unit)
//    Debug: (string -> unit)
//    Pause: (unit)
//    }
//  type WorldState =
//    | Running of Snake
//    | GameOver
//    | Paused of bool
//  type HasEaten = bool
//  type Action =
//    | Move
//    | ChangeDirection of Direction
//    | AddTail
//    | Die
//    | Pause 
//  type MoveHead = Position -> Direction -> Body
//  type GetTail = Body -> Body // oklart om det behövs
//  type GetHead = Body -> Position
////  kolla typen på en mailboxprocessor
////  type CreateGame = GameInit -> GameAgent
////  kolla typen på commandstream
////  borde returna någon sorts gamestate, running/gameover osv
////  type StartGame = Config -> Renderer -> CommandStream -> unit
//  
//module Game =
//  open Core
//  let getNewPos (pos:Position) (dir:Direction) =
//    match dir with
//    | Up -> {pos with y = pos.y - 1}
//    | Down -> {pos with y = pos.y + 1}
//    | Left -> {pos with x = pos.x - 1}
//    | Right -> {pos with x = pos.x + 1}
//  let getHead (snake:Body): Position = snake.Head
//  let getTail (snake:Body) (hasEaten:HasEaten): Body =
//    match hasEaten with
//    | true -> snake
//    | false -> List.take (snake.Length - 1) snake
//  let moveSnake (snake:Body) (dir:Direction) (hasEaten:HasEaten): Body =
//    let head = getHead snake
//    let tail = getTail snake hasEaten
//    let newHead =
//      match dir with
//      | Up -> getNewPos head Up
//      | Down -> getNewPos head Down
//      | Left -> getNewPos head Left
//      | Right -> getNewPos head Right
//    newHead::tail
//  let createGame (init:WorldState) (renderer:Renderer) =
//    let mutable gameState = init
//    let gameAgent =
//      MailboxProcessor.Start(fun inbox ->
//        let rec loop (state:WorldState) = async {
//          let output x =
//            match x with
//            | Running s -> do renderer.GameState s
//            do renderer.Debug (x |> string)
//          let debug = renderer.Debug
//          let! action = inbox.Receive()
//          match state with
//          | Running snake ->
//            match action with
//            | Move ->
//              let newSnake = moveSnake snake.body snake.dir false
//              let newState = (Running {snake with body=newSnake})
//              output newState
//              return! loop newState
//            | ChangeDirection newDir ->
//              return! loop (Running {snake with dir=newDir})
//            | AddTail ->
//              let newBody = moveSnake snake.body snake.dir true
//              return! loop (Running {snake with body = newBody})
//            | Die ->
//              gameState <- GameOver 
//              return ()
//            | Pause ->
//              debug "tryckte pause"
//              gameState <- Paused true
//              //return! loop state
//            | _ -> return! loop state
//          | Paused b ->
//            debug "PAUSED"
//            gameState <- Paused true
//          | GameOver ->
//            match action with
//            | _ -> return! loop state
//        }
//        loop init
//        )
//    gameAgent, gameState
//  let startGame (config:Config) (renderer:Renderer) (commandStream) =
//    /// set gameInit
//    let gameInit:WorldState = Running {
//      body = [config.startPosition]
//      dir = config.startDirection }
//    /// get gameAgent från createGame
//    let gameAgent, gameState = createGame gameInit renderer
//    /// rec loop
//    let rec gameLoop () =
//      async {
//        match gameState with
//        | Running _ ->
//          do! Async.Sleep 500
//          gameAgent.Post Move
//          return! gameLoop ()
//        | Paused b -> return ()
//        | GameOver -> return ()
//      }
//    gameLoop () |> Async.StartImmediate
//    /// run loop
//    /// subscribe commandstream till gameAgent.post
//    commandStream
//    |> Observable.subscribe gameAgent.Post |> ignore
//    
//    gameState
//  