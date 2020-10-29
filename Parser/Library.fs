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
/// kan inte använda reduce för köra andThens för att parsea en hel string t.ex
///   string
///   |> Seq.map parseChar //gör till parsers
///   |> Seq.reduce andThen
/// för andThen har olika input mot output (tuple vs single)

// test
//let parseDigit = anyOf ['0'..'9']
//let parseThreeDigits = parseDigit .>>.parseDigit .>>.parseDigit
//run parseThreeDigits "123a" // Success ((('1', '2'), '3'), "a")
/// returnar tuples så kan inte pipea vidare och är drygt att jobba med, vill ha en string "123", "a"
///
/// i curry-innerfunktionen kör och få resultat
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
//// test
//let parseDigit = anyOf ['0'..'9']
///// returnar en parser<string>
//let parseThreeDigitsAsString =
//  let tupleParser = parseDigit .>>.parseDigit .>>.parseDigit
//  // gör om den till en string
//  let transformTuple ((c1, c2), c3) = String [|c1;c2;c3|]
//  mapParse transformTuple tupleParser
//run parseThreeDigitsAsString "123a" //val it : Result<String * string> = Success ("123", "a")
//// är mappad nu till parser<int>
//let parseThreeDigitsAsInt = mapParse int parseThreeDigitsAsString
//run parseThreeDigitsAsInt "123a"  //val it : Result<int * string> = Success (123, "a")
