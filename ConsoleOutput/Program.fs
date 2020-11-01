// Learn more about F# at http://fsharp.org

open System


[<EntryPoint>]
let main _ =
  Console.Clear()
  Console.CursorVisible <- false
//  Command<int>.AddCommand "add" (fun x y -> x + y)
//  Command<int>.Agent.Post (Run, "add", 5, 5)
////  let huhu = Command<int>.Agent.PostAndReply (fun rc -> (Reply rc, "add",5,5))
//  let huhu = Command<int>.Agent.PostAndReply (fun rc -> (Reply rc, "add",5,5))
//  let wawa = Command<int>.Agent.PostAndAsyncReply (fun rc -> (Reply rc, "add",5,5))
//  printfn "%A" (wawa.GetType)
//  let reply a b c = fun rc -> (Reply rc, a, b, c )
//  let foo = Command<int>.Agent.PostAndAsyncReply (reply "add" 5 5)
//  printfn "%A" wawa

  
  
  
  
  
  
  
  
//  Command<int>.AddCommand "Add" (fun x y -> x + y)
//  Command<int>.AddCommand "Subtract" (fun x y -> x - y)
//  Command<int>.Post "Add" 5 5
//  Command<int>.Post "Add" 5 2
//  Command<int>.Post "Add" 5 4
//  Command<int>.Post "Add" 5 4
//  let reply = Command<int>.PostAndReply "Add" 5 5
//  let reply2 = Command<int>.PostAndReply "huhu" 5 5
//  match reply2 with
//  | Some x -> printfn "hittade x"
//  | None -> printfn "hittade inte x"
//  printfn "%A" reply
//  printfn "%A" reply2
//  let huhu = Command<int>.PostAndReplyAsync "Add" 5 5
//  
//  match Command<int>.PostAndReplyAsync "Ad" 5 5 with
//  | Ok x ->
//    x
//    |> Async.RunSynchronously
//    |> printfn "%i"
//  | Fail x -> printfn x
//  
//  let huhu x =
//    match x with
//    | x when x % 2 = 0 -> Ok (sprintf "Gick bra%i" x)
//    | _ -> Fail "gick inte bra"
//  printfn "%A" (huhu 10)
//  
  
  
//  let wawa = add 10 15
//  printfn "%A" wawa
  
  
  
  
//  let outputStream:OutputStream = {
//    GameState = Ui.GameState
//    Debug = Ui.Debug}
//  let sendInput, inputstream =
//    let event = Event<_>()
//    event.Trigger, event.Publish
//  let input = CreateGame.game outputStream
//  StartGame.StartGame outputStream inputstream
//  input "Move Up"
//  let rec getInput() =
//    let consoleInput = Console.ReadLine()
//    input consoleInput
//    getInput()
//  getInput()
//

//  let input = game.Input
//  let output = game.Output
//  let huhu = game.Start
//  
//  let rec getInputCommand() =
//    let input = Console.ReadKey(true).KeyChar
//    let command = Keybinds.getCommand input
//    match command with
//    | " " -> ()
//    | _ -> command |> game.input
//    getInputCommand()
//    
//  getInputCommand()
    
  Console.ReadKey() |> ignore
  0
//  let showTileMap x =
////    Console.Clear()
//    printfn "printtilemap: %A" x
//    
//  let showDebug = printfn "%A"
//  let renderer:Renderer = { PrintTileMap = showTileMap
//                            PrintDebug = showDebug}
//  let sendCommand, commandStream =
//    let e = Event<_>()
//    e.Trigger, e.Publish
//  let huhu = StartGame renderer commandStream
//  let rec getInput() =
//    match Console.ReadKey(true).KeyChar with
//    | ',' -> MovePlayer Up |> sendCommand
//    | 'o' -> MovePlayer Down |> sendCommand
//    | 'a' -> MovePlayer Left |> sendCommand
//    | 'e' -> MovePlayer Right |> sendCommand
//    | 'p' -> Pause |> sendCommand
//    | _ -> ()
//    getInput()
//  getInput()
//  Console.ReadKey(true) |> ignore
//  0
//  
//  
//  
//  let config = { startPosition = {x=3;y=3}
//                 startDirection = Right}
//  let sendCommand, commandstream =
//    let e = Event<_>()
//    e.Trigger, e.Publish
//  let drawChar (pos:Position) =
//    Console.SetCursorPosition(pos.x, pos.y)
//    Console.Write('#')
//    
//  let drawSnake (snake:Body) =
//    snake
//    |> List.iter drawChar
//  let printGameState s =
////    drawSnake s.body
//    printfn "%A" s
//  let printDebug = printfn "%s"
//  let printPauseMenu =
//    printfn "Paused"
//    
//  let renderer:Renderer =
//    match output with
//    | Debug -> {
//        GameState = fun s -> ()
//        Debug = printDebug
//        Pause = ()
//      }
//    | _ ->
//      { GameState = printGameState
//        Debug = printDebug
//        Pause = printPauseMenu}
//  let game = Snake.Game.startGame config renderer commandstream
//  Move |> sendCommand
//  let rec getInput() =
//    match Console.ReadKey(true).KeyChar with
//    | ',' -> ChangeDirection Up |> sendCommand
//    | 'o' -> ChangeDirection Down |> sendCommand
//    | 'a' -> ChangeDirection Left |> sendCommand
//    | 'e' -> ChangeDirection Right |> sendCommand
//    | ' ' -> AddTail |> sendCommand
//    | 'x' -> Die |> sendCommand
//    | 'p' -> Pause |> sendCommand
//    | _ -> ()
//    getInput()
//  getInput()
//  0 // return an integer exit code
