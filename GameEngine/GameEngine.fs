module GameEngine.GameEngine

open System.Collections.Generic
open System.Diagnostics.CodeAnalysis

type ComponentId = int
type EntityId = int

type EntityManager() =
  static let entities = List<Entity>()
  static member CreateEntity() =
    let newEntity = Entity(entities.Count)
    entities.Add newEntity
    newEntity
and Entity(id:EntityId) =
  member this.Id = id
type ComponentPoolCounter() =
  static let mutable componentCount = 0
  static member getNewComponentId: ComponentId =
    componentCount <- componentCount + 1
    componentCount
type ComponentPool<'t>() =
  static let mutable components = Dictionary<EntityId,'t>()
  static member addComponentToPool = components.Add
  static member getComponentByEntityId = components.TryGetValue
type Component<'t>(e:Entity, c:'t) =
  do ComponentPool<'t>.addComponentToPool(e.Id,c)
  let mutable componentData: 't = c
  member this.componentType = typedefof<'t>
  member this.owner = e
  member this.updateData x = componentData <- x
  member this.data = componentData
  member this.id = ComponentPoolCounter.getNewComponentId
type Game() =
  static member huhu = 0
type Entity with
  member this.tryGet<'t>(): Skogix.Core.Result<'t, string> =
    let bool, tryResult = ComponentPool<'t>.getComponentByEntityId this.Id
    match bool with
    | true -> Skogix.Core.Yay tryResult
    | false -> Skogix.Core.Nay "kunde inte hitta component"
  member this.add<'t>(c:'t) =
    let newComponent = Component<'t>(this, c)
    newComponent