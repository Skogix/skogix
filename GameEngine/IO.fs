module GameEngine.IO

open System
open System.Collections
open System.Collections.Generic
type ComponentAgent() =
  let mutable components = new List<Type>()
  member this.componentList = components
type WorldAgent() =
  let mutable worlds = new List<Type>()
  member this.worldList = worlds
type Init(outputStream:OutputStream) =
  let mutable componentAgent = ComponentAgent()
  let mutable worldAgent = WorldAgent()
  member this.addComponentType<'componentType>() = componentAgent.componentList.Add typedefof<'componentType>
  member this.addWorld<'worldType>() = worldAgent.worldList.Add typedefof<'worldType>
  member this.outputstream = outputStream
  member this.components = componentAgent.componentList
  member this.worlds = worldAgent.worldList
  
type IO(init:Init) =
  let InputAgent = MailboxProcessor<string>.Start(fun inbox ->
    let rec loop () = async {
      let! mail = inbox.Receive()
      printfn "InputAgent: %A" mail
      return! loop ()
    }
    loop ()
    )
  let OutputAgent = MailboxProcessor<string>.Start(fun inbox ->
    let rec loop () = async {
      let!  mail = inbox.Receive()
      printfn "OutputAgent: %A" mail
      return! loop ()
    }
    loop ()
    )
//  member this.input = inputTrigger
  member this.input = InputAgent.Post
  member this.outputStream = init.outputstream
  member this.outputAgent = OutputAgent
let getInit(outputStream:OutputStream) = Init(outputStream)
  
