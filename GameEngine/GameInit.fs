module GameEngine.GameInit

open Domain
open GameEngine
open Common
open SkogixIO


let gameInit (outputStream:OutputStream) =
  let input, inputStream =
    let event = Event<Command>()
    event.Trigger, event.Publish
  let skogixIO = SkogixIO outputStream
  (skogixIO, input)
  