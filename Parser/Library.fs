module Basics
open System
/// option i stil med some/none
type Result<'a> =
  | Success of 'a
  | Failure of string
/// Wrapper som mappar string till result
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
/// "unwrapper" för parser, kör inre funktionen
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
  