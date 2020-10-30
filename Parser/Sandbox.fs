module Parser.Sandbox

//let log p = printfn "exp: %A" p
//let loggedWorkflow =
//  let x = 42
//  log x
//  let y = 43
//  log y
//  let z = x + y
//  log z
//  z
type LoggingBuilder() =
  let log p = printfn "exp: %A" p
  member this.Bind(x, f) =
    log x
    f x
  member this.Return(x) = x 
  
let logger = LoggingBuilder()
let loggedWorkflow =
  logger {
    let! x = 42
    let! y = 43
    let! z = x + y
    return z
  }
  
  
//let divideBy bot top =
//  if bot = 0
//  then None
//  else Some(top/bot)
//let divideByWorkflow init x y z =
