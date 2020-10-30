module GameEngine.Domain

type Result<'tSuccess, 'tFailure> =
  | Success of 'tSuccess
  | Failure of 'tFailure
let bind processFunc = function
  | Success s -> processFunc s
  | Failure f -> Failure f
let (>>=) x f = f x
