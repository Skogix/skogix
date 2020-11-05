// Learn more about F# at http://fsharp.org

open System
open Game
open Domain
open Game
open Game.Components
[<EntryPoint>]
let main _ =
  Console.Clear()
  Console.CursorVisible <- false
  printfn "hello world från program"
//  sSharp """listOfThings [ 123.341, setXtoTrue true, "string", null, innerArrayCommand [1,true,null],{"name": "Skogix", "health": 100} ]"""
  let positionComponent1 = Component<Position>({x=3;y=3})
  let name1 = Component<Player>({name = "Skogix"})
  let positionComponent2 = Component<Position>({x=8;y=5})
  let entity1 = EntityPool.createEntity
  let entity2 = EntityPool.createEntity
  ComponentPool<Position>.addComponent entity1 positionComponent1
  ComponentPool<Position>.addComponent entity2 positionComponent2
  ComponentPool<Player>.addComponent entity1 name1
//  ComponentPool<Position>.addComponent(2, positionComponent2)
  
  let tryGetPositionForE1 = ComponentPool<Position>.tryGet entity1
  match tryGetPositionForE1 with
  | Some x -> printfn "%A" x
  | None -> printfn "finns inte"
  printfn "entity1: %A" entity1.components
  printfn "entity2: %A" entity2.components
  printfn "%A" ComponentPool<Position>.components
  

  printfn "\n\n\n\n\n\n\n\n\n\nEnd, sover 2 sek"
  Threading.Thread.Sleep 2000
//  Console.ReadLine() |> ignore
  0