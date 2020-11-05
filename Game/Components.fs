module Game.Components
open System
type Position = {x:int;y:int}
type Player = {name:string}




type ComponentId = int
type ComponentType = Type
type Component<'data>(data:'data) =
  member this.componentId = ComponentId
  member this.data = data
  member this.typedefof = typedefof<'data>
