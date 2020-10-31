module GameEngine.StartGame
open CreateGame
open GameEngine.Domain

//let startGame inputStream (outputStream:OutputStream) =
//  let inputManager = createGame outputStream
//  
//  inputStream
//  |> Observable.subscribe inputManager.Post |> ignore
let StartGame (output:OutputStream) inputStream =
  let inputManager = CreateGame.createGame output
  inputStream
  |> Observable.subscribe inputManager.Post |> ignore
  0
