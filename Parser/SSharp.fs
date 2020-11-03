module internal Parser.SSharp
open System
open Parser.Core
type SkogixValue =
  | SkogixNull
  | SkogixBool of bool
  | SkogixString of string
  | SkogixNumber of float
  | SkogixArray of SkogixValue list
  | SkogixObject of Map<string, SkogixValue>
  | SkogixCommand of string * SkogixValue option

/// infix som kör parser, ignorerar resultatet och returnar value
let (>>%) p x = p |>> fun _ -> x
let skogixNull =
  parseString "null"
  >>% SkogixNull <?> "null"
run skogixNull "null"
run skogixNull "huhu"
let skogixBool =
  let skogixTrue =
    parseString "true"
    >>% SkogixBool true
  let skogixFalse =
    parseString "false"
    >>% SkogixBool false
  skogixTrue <|> skogixFalse <?> "bool"
let skogixStringParser =
  let chars = anyOf (['a'..'z'] @ ['A'..'Z'])
  let string =
    manyChars chars 
  string
  <?> "string parser"
let skogixQuotedString =
  let quote = parseChar '\"' <?> "quote"
  let chars = anyOf (['a'..'z'] @ ['A'..'Z'])
  quote >>. manyChars chars .>> quote
let skogixString =
  skogixQuotedString
  |>> SkogixString
  <?> "quoted string"
let skogixNumber =
  let optMinus = opt (parseChar '-')
  let zero = parseString "0"
  let digitOneToNine = satisfy (fun ch -> Char.IsDigit ch && ch <> '0') "digits 1-9"
  let digit = satisfy (fun ch -> Char.IsDigit ch) "digit"
  let dot = parseChar '.'
  let optPlusMinus = opt(parseChar '-' <|> parseChar '+')
  let nonZeroNumber =
    digitOneToNine .>>.manyChars digit
    |>> fun (first, rest) -> string first + rest
    
  let numberPart = zero <|> nonZeroNumber
  let fractionPart = dot >>. manyChars1 digit
  
  let convertToSkogixNumber ((optSign, numberPart), fractionPart) =
    let (|>?) opt f =
      match opt with
      | None -> ""
      | Some x -> f x
    let signStr =
      optSign
      |>? string // "-"
    let fractionPartStr =
      fractionPart
      |>?  (fun digits -> "." + digits) // ".123"
    (signStr + numberPart + fractionPartStr)
    |> float
    |> SkogixNumber
  
  optMinus .>>. numberPart .>>. opt fractionPart
  |>> convertToSkogixNumber
  <?> "number"
run skogixNumber "123"
run skogixNumber "-123"
run skogixNumber "123.4"
// todo vetefan hur läsa resten ska lösas
run skogixNumber "-123."
run skogixNumber "00.1"

let createParserForwardedToRef<'a>() =
  let dummyParser =
    let innerFn input : Result<'a * string> = failwith "inte satt forward parsern"
    {parseFn=innerFn;label="dno"}
  let parserRef = ref dummyParser
  let innerFn input =
    run !parserRef input
  let wrapperParser = {parseFn=innerFn;label="dno"}
  wrapperParser, parserRef
let sSharpValue, sValueRef = createParserForwardedToRef<SkogixValue>() 

let skogixArray =
  let left = parseChar '[' .>> spaces
  let right = parseChar ']' .>> spaces
  let comma = parseChar ',' .>> spaces
  let value = sSharpValue .>> spaces
  let values = separatedBy1 value comma
  between left values right
  |>> SkogixArray
  <?> "array"
// sätter potentiella forwarden till number
sValueRef := skogixNumber
run skogixArray "[1,2]"
run skogixArray "[   1,2]"
run skogixArray "[1,    2]"
run skogixArray "[1.2]"
run skogixArray "[a, 1.2]" // kan bara köra numbers atm

run skogixArray "[1,true]"
run skogixArray "[  x, 1,null]"
run skogixArray "[1, ,.,   2]" // fail med .
run skogixArray "[1.2]"
run skogixArray "[a, 1.2]" // string "a", number 1.2

let skogixObject =
  let left = parseChar '{' .>> spaces
  let right = parseChar '}' .>> spaces
  let colon = parseChar ':' .>> spaces
  let comma = parseChar ',' .>> spaces
  let key = skogixQuotedString .>> spaces
  let value = sSharpValue .>> spaces
  
  let keyValue = (key .>> colon) .>>. value
  let keyValues = separatedBy1 keyValue comma
  
  between left keyValues right
  |>> Map.ofList
  |>> SkogixObject
let skogixCommand =
  let command = skogixStringParser .>> spaces
  let argument = opt (
                       skogixNull
                       <|> skogixBool
                       <|> skogixNumber
                       <|> skogixArray
                       <|> skogixString
                       <|> skogixObject
                       
                       .>> spaces
                     )
  let skogixCommand = command .>>. argument
//  let argument = opt (sValue .>> spaces)
//  let skogixCommand = command .>>. argument
  skogixCommand
  |>>  SkogixCommand
sValueRef := choice
  [
    skogixNull
    skogixBool
    skogixNumber
    skogixObject
    skogixString
    skogixCommand
    skogixArray
  ]
let printSSharp (input:string) = printResult (run sSharpValue input)
let getSSharp (input:string) = run sSharpValue input
