// Learn more about F# at http://fsharp.org

open System
open System.Collections
open GameEngine
open GameEngine.State
[<EntryPoint>]
let main _ =
  Console.Clear()
//  let list = ArrayList()
//  list.Add("huh")
//  list.Add("huhu")
//  list.Add("huhu")
//  let am = ArrayListManager(list)
//  am.Print()
//  am.Update(0, 100)
//  am.Print()
  let print =
    let logger = AsyncPrintAgent()
    [1..10]
    |> List.map (fun i -> makeTask logger.Log i)
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore
  
  Console.ReadKey() |> ignore
  0 // return an integer exit code
