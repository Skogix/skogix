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
let parseChar char =
  let f str =
    if String.IsNullOrEmpty(str) then
      Failure "Inge mer input"
    else
      let first = str.[0]
      if first = char then
        let rest = str.[1..]
        Success (char, rest)
      else
        Failure (sprintf "Ville ha %c, fick %c" char first)
  Parser f
/// "unwrapper" för parser, kör inre funktionen med inputstring
let run p str =
  // deconstructar parser precis som en (x,y) skulle deconstructa en tuple
  let (Parser f) = p
  f str
/// and-combinator
/// båda måste matcha för success return
/// todo kolla bind-lösning
let andThen p1 p2 =
  /// if error, break
  /// elseif error, break
  /// return båda
  let f str =
    let result1 = run p1 str
    match result1 with
    | Failure error -> Failure error
    | Success (x1, rest1) ->
      let result2 = run p2 rest1
      match result2 with
      | Failure error -> Failure error
      | Success (x2, rest2) ->
        let newValue = (x1, x2)
        Success (newValue, rest2)
  Parser f
/// infix för andThen
let ( .>>. ) = andThen
/// or-combinator
/// a eller b måste success för return
let orElse p1 p2 =
  // kör 1, return om success
  // annars return 2
  let f str =
    let result1 = run p1 str
    match result1 with
    | Success _ -> result1
    | Failure _ ->
      let result2 = run p2 str
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
let choice ps =
  List.reduce (<|>) ps
/// anyOf
/// combinea alla med choice
let anyOf chars =
  chars
  |> List.map parseChar
  |> choice
/// map
/// if success, kör funktionen och returna ny mappad value
/// mappar a -> b till parser<a> -> parser<b>
let mapParse f p =
  let f str =
    let result = run p str
    match result with
    | Success (x, rest) ->
      let newX = f x // nya valuen mappad
      Success (newX, rest) // nya valuen och resten av input
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
let returnParser a =
  let f b = Success (a, b)
  Parser f
let applyParser fParser xParser =
  // gör ett parser-par / tuple (f,value)
  (fParser .>>. xParser)
  // mappa genom att köra f x
  |> mapParse (fun (f,x) -> f x)
let ( <*> ) = applyParser
// mapParse kan bara mappa funktioner med en parameter
// return/apply funkar som helpers, t.ex
let lift2 f aParser bParser =
  returnParser f <*> aParser <*> bParser
