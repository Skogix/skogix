module Parser.SSharp
open Core
type SkogixValue =
  | SkogixNull
let skogixNull =
  parseString "null"
  |>> (fun _ -> SkogixNull)