module GameEngine.CreateGame
open Domain
open GameEngine
open Input

let PlayerInit:Player = {
  Name = "Skogix"
  Position = {x=3;y=3}
  Glyph = '@'}
let GameStateInit:GameState = {
  Player = PlayerInit
}
let WorldInit: World = {
  Game = GameStateInit
}
let outputAgent(outputstream:OutputStream) =
  let outGameState = outputstream.GameState
  let outDebug = outputstream.Debug
  MailboxProcessor.Start(fun inbox ->
    let rec loop() = async {
      let! output = inbox.Receive()
      outDebug "Test från outputagent"
      return! loop()
    }
    loop()
    )
let game(outputStream:OutputStream) =
//  let Output = outputAgent(outputStream)
  let Input = inputAgent(outputStream)
  Input.Post