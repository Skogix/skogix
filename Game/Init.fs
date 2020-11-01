module Game.Init

open System
open Parser.Core

  
type OutputStream = string
type Renderer = {
  Game: (string -> unit)
  Debug: (string -> unit)
}
let internal OutputAgent(outputStream:Renderer) = MailboxProcessor<OutputStream>.Start(fun inbox ->
  let output = outputStream
  let rec loop () = async {
    let! mail = inbox.Receive()
    output.Debug ("Från outputagent:" + mail)
    return! loop ()
  }
  loop ()
  )

type TestGameComponent = {Name:string}
let internal gameEngineRenderer:Renderer = {
  Game = printfn "Game::: %A"
  Debug = printfn "Debug::: %A"}
let debug str = gameEngineRenderer.Debug str
let debug1 str x  = gameEngineRenderer.Debug (sprintf "%s: %A" str x)
type GameCreator() =
  let mutable outputSources = [gameEngineRenderer]
  let mutable componentTypes = [typedefof<TestGameComponent>]
  member public this.addOutputSource (outputStream:Renderer) =
    outputStream::outputSources
    |> ignore
  member public this.addComponentType<'t> () = componentTypes <- (typedefof<'t>)::componentTypes
  
  member internal this.outputs = outputSources
  member internal this.components = componentTypes
let gameCreator = GameCreator()
gameEngineRenderer.Debug "skapar gamecreator"

type InputStream = string
let internal InputAgent = MailboxProcessor<InputStream>.Start(fun inbox ->
  let rec loop () = async {
    let! input = inbox.Receive()
    debug1 "inputagent" input
    return! loop ()
  }
  loop ()
  )
type ComponentStream = string
let internal ComponentAgent = MailboxProcessor<ComponentStream>.Start(fun inbox ->
  let rec loop () = async {
    let! mail = inbox.Receive()
    printfn "ComponentAgent: %A" mail
    return! loop ()
  }
  loop ()
  )
type Game() =
  member this.input = InputAgent.Post
  member this.outputs = gameCreator.outputs
  member this.components = gameCreator.components
let game = Game()
debug "skapar game"
  
type Tester() =
  member this.test = 0
let tester = Tester()
