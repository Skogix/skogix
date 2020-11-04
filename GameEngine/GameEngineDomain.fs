namespace GameEngine

open System
open System.Collections

type InputCommand =
  | InputTest
type InputCommands = InputCommand list
type Component (data:'a) =
  member this.data = data
  member this.typedefof = typedefof<'a>
type EntityId = int
type Entity = {
  EntityId:EntityId
}
type World =
  | GameWorld of Entity list
type OutputStream = {
  Debug: (string -> unit)
  GameState: (World -> unit)
}
