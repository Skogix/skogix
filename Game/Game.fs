module Domain
open Game.Components
open System.Collections.Generic
 
type EntityId = int
type Entity(id:EntityId) =
  let mutable componentsList = List<ComponentType>()
  member this.addComponent<'a>() = componentsList.Add typedefof<'a>
  member this.components = componentsList
  member this.id = id

type EntityPool() =
  static let entities = List<Entity>()
  static member createEntity: Entity =
    let newId = entities.Count
    let newEntity = Entity(newId)
    entities.Add newEntity
    newEntity
  static member get<'componentType>() = 0
[<AbstractClass; Sealed>] 
type ComponentPool<'t>() =
  static let cs = Dictionary<EntityId, Component<'t>>()
  static member addComponent (e:Entity) (c:Component<'t>) =
    e.addComponent<'t>() 
    cs.Add (e.id, c)
  static member components = cs
  static member tryGet (entity:Entity) =
    let bool, tryGet = cs.TryGetValue entity.id
    match bool with
    | false -> None
    | true -> Some tryGet.data
