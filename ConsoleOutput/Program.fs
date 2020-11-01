// Learn more about F# at http://fsharp.org

open System


[<EntryPoint>]
let main _ =
  Console.Clear()
  Console.CursorVisible <- false
  
  let gameRenderer = ()
  let debugRenderer = printfn "Debug::: %s" 
  
  let outputStream:Renderer =
    {game: gameRenderer
     debug: debugRenderer}
    
  let gameInit = gameInit outputStream inputStream
  
  gameInit.AddCommand "Add" (fun x y -> x + y)
  
  let game = gameInit.StartGame
  
  game.input "Add" 5 5
  
  
  
  
  Console.ReadLine() |> ignore
  0