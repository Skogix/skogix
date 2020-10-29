module Basics
open System
/// resultatet av en parseing, returnar det du parsear om success annars en errorstring
type Result<'a> =
  | Success of 'a
  | Failure of string
/// wrapper som mappar string till result
/// returnar antingen parsertypen + rest eller failmsg + rest
type Parser<'T> = Parser of (string -> Result<'T * string>)
/// parsea en char från en str och mappa till parser
/// har en str curryad
let parseChar charToParse =
  let f (str:string) =
    if String.IsNullOrEmpty(str) then
      Failure "Inge mer input"
    else
      let first = str.[0]
      if first = charToParse then
        let rest = str.[1..]
        Success (charToParse, rest)
      else
        Failure (sprintf "Ville ha %c, fick %c" charToParse first)
  Parser f
/// "unwrapper" för parser, kör inre funktionen med inputstring
let run (parser) (input) =
  // deconstructar parser precis som en (x,y) skulle deconstructa en tuple
  let (Parser runInnerFunction) = parser
  runInnerFunction input
/// and-combinator
/// båda måste matcha för success return
/// todo kolla bind-lösning
let andThen parser1 parser2 =
  /// if error, break
  /// elseif error, break
  /// return båda
  let f input =
    let result1 = run parser1 input
    match result1 with
    | Failure error -> Failure error
    | Success (v1, rest1) ->
      let result2 = run parser2 rest1
      match result2 with
      | Failure error -> Failure error
      | Success (v2, rest2) ->
        let newValue = (v1, v2)
        Success (newValue, rest2)
  Parser f
/// infix för andThen
let ( .>>. ) = andThen
/// or-combinator
/// a eller b måste success för return
let orElse parser1 parser2 =
  // kör 1, return om success
  // annars return 2
  let f input =
    let result1 = run parser1 input
    match result1 with
    | Success _ -> result1
    | Failure _ ->
      let result2 = run parser2 input
      result2
  Parser f
/// infix för orElse
let ( <|> ) = orElse
/// choice 
/// kör orElse över en hel lista med combinators för att få choice
///
/// reducear och lägger in orElse mellan alla parsers i listan
/// går igenom hela listan och returnar det som ger success eller
/// sista om alla failat
let choice listOfParsers =
  List.reduce (<|>) listOfParsers
/// anyOf
/// combinea alla med choice
let anyOf listOfChars =
  listOfChars
  |> List.map parseChar
  |> choice
/// map
/// if success, kör funktionen och returna ny mappad value
/// mappar a -> b till parser<a> -> parser<b>
let mapParse f parser =
  let f input =
    let result = run parser input
    match result with
    | Success (value, rest) ->
      let newValue = f value // nya valuen mappad
      Success (newValue, rest) // nya valuen och resten av input
    | Failure error -> Failure error
  Parser f
/// infix av mapParse
let ( <!> ) = mapParse
/// infix av mapParse men reversead för pipelineing
let ( |>> ) x f = mapParse f x

/// returnParser
/// transform en normal value till parser, t.ex a -> parser<a>
/// tänk en map fast för values och inte funktioner
/// applyParser
/// transformar en parser som har en funktion, t.ex parser<a->b> -> parser<a> -> parser<b>
let returnParser value =
  let f input = Success (value, input)
  Parser f
let applyParser fParser valueParser =
  // gör ett parser-par / tuple (f,value)
  (fParser .>>.valueParser)
  // mappa genom att köra f x
  |> mapParse (fun (f,v) -> f v)
let ( <*> ) = applyParser
// mapParse kan bara mappa funktioner med en parameter
// return/apply funkar som helpers, t.ex
let lift2 f xParser yParser =
  returnParser f <*> xParser <*> yParser
let lift3 f xParser yParser zParser =
  returnParser f <*> xParser <*> yParser <*> zParser
// t.ex
let addParser = lift2 (+)
let startWith (str:string) (prefix:char) = str.StartsWith(prefix)
let startsWithParser = lift2 startWith
// lyfter alla values av parsern rakt av