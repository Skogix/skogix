﻿module Basics
open System
open System.Collections.Concurrent
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
  let inF str =
    if String.IsNullOrEmpty(str) then
      Failure "Inge mer input"
    else
      let first = str.[0]
      if first = char then
        let rest = str.[1..]
        Success (char, rest)
      else
        Failure (sprintf "Ville ha %c, fick %c" char first)
  Parser inF
/// "unwrapper" för parser, kör inre funktionen med inputstring
let run p str =
  // deconstructar parser precis som en (x,y) skulle deconstructa en tuple
  let (Parser inF) = p
  inF str
/// and-combinator
/// båda måste matcha för success return
/// todo kolla bind-lösning
let andThen p1 p2 =
  /// if error, break
  /// elseif error, break
  /// return båda
  let inF str =
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
  Parser inF
/// infix för andThen
let ( .>>. ) = andThen
/// or-combinator
/// a eller b måste success för return
let orElse p1 p2 =
  // kör 1, return om success
  // annars return 2
  let inF str =
    let result1 = run p1 str
    match result1 with
    | Success _ -> result1
    | Failure _ ->
      let result2 = run p2 str
      result2
  Parser inF
/// infix för orElse
let ( <|> ) = orElse
/// choice 
/// kör orElse över en hel lista med combinators för att få choice
///
/// reducear och lägger in orElse mellan alla parsers i listan
/// går igenom hela listan och returnar det som ger success eller
/// sista om alla failat
let chooseOne ps =
  List.reduce (<|>) ps
/// anyOf
/// combinea alla med choice
let anyOf chars =
  chars
  |> List.map parseChar
  |> chooseOne
/// map
/// if success, kör funktionen och returna ny mappad value
/// mappar a -> b till parser<a> -> parser<b>
let mapParse f p =
  let inF str =
    let result = run p str
    match result with
    | Success (x, rest) ->
      let newX = f x // nya valuen mappad
      Success (newX, rest) // nya valuen och resten av input
    | Failure error -> Failure error
  Parser inF
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
  let inF b = Success (a, b)
  Parser inF
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

/// tar en lista med parsers och mappar till en stor parser
let rec seqParsers ps =
  // todo: hemmagjort consfunktion, kolla om det går att göra snyggae
  let splitCons first rest = first::rest
  // lyfter parser<'a> till parser<'a list>
  let consParser = lift2 splitCons
  match ps with
  | [] -> returnParser []
  | first::rest -> consParser first (seqParsers rest)

/// mappar string -> parser
let parseString (str:seq<char>) =
  let mapCharsToStr cs = String(List.toArray cs)
  // seq<char>
  str
  // char list
  |> List.ofSeq
  // parser<char>
  |> List.map parseChar
  // parser<char list>
  |> seqParsers
  // parser<string>
  |> mapParse mapCharsToStr

/// parsea något tills fail / kör tills något hittas eller failar
/// t.ex läsa in siffror till en viss char typ ./, eller failar att hitta fler
/// en för "ingen eller fler" och en för "minst en"
///
/// kör parsern
/// if fail -> [] så är aldrig failure
/// if success loopa

let rec parseMoreThanOne p input =
  let result1 = run p input
  match result1 with
  | Failure _ -> ([], input)
  // (valuen som parseas, resten av input 1)
  | Success (x, restIn) ->
    // (resten av alla values från innan, resten av input 2)
    let (xs, restOut) =
      // kör så länge det är success
      parseMoreThanOne p restIn
    // skicka ut nya values när det kommer hit
    let values = x::xs
    // (alla values som hittades, resten efter fail)
    (values, restOut)
/// many är bara en wrapper
let many p =
  let rec inF str =
    // parsea input och wrappa i success
    Success(parseMoreThanOne p str)
  Parser inF
/// kör, if fail -> fail
/// if succ kör parsemorethanone
/// consa och returna
let many1 p =
  let rec inF str =
    let result1 = run p str
    match result1 with
    | Failure err -> Failure err
    | Success (x, restIn) ->
      let(xs, restOut) =
        parseMoreThanOne p restIn
      let values = x::xs
      Success (values, restOut)
  Parser inF
let oneOrZero p =
  let some = p |>> Some
  let none = returnParser None
  some <|> none
/// parsea en int
/// gör en parser
/// kör many1 för att få en lista
/// mappa listan med digts till string sen till int
let parseInt =
  let resultToInt (sign, cs) =
    let i = String(List.toArray cs) |> int
    match sign with
    | Some _ -> -i // gör negativ
    | None -> i
  let digit = anyOf ['0'..'9']
  let digits = many1 digit
  oneOrZero (parseChar '-') .>>. digits
  |>> resultToInt
/// kasta iväg resultat
/// "xxx" behöver inte ha "", samma med slutar med ;, mellanslag osv
/// >>. behåller höger, .>> behåller vänster
let (.>>) p1 p2 = p1 .>>. p2 |> mapParse (fun (a,_) -> a)
let (>>.) p1 p2 = p1 .>>. p2 |> mapParse (fun (_,b) -> b)
let between p1 p2 p3 = p1 >>. p2 .>> p3
/// listor
/// gör en parser av [ , osv + vad man letar efter sen kasta bort separator
/// leta med many
/// kombinera resultaten
let separatedByOne p sep =
  let sepThenParse = sep >>. p
  p .>>. many sepThenParse
  |>> fun (p, ps) -> p::ps
let separateBy p sep =
  separatedByOne p sep <|> returnParser []
//test
let dot = parseChar ','
let digit = anyOf ['0'..'9']

let zeroOrMoreDigitList = separateBy digit dot
let oneOrMoreDigitList = separatedByOne digit dot
run oneOrMoreDigitList "1;"      // Success (['1'], ";")
run oneOrMoreDigitList "1,2;"    // Success (['1'; '2'], ";")
run oneOrMoreDigitList "1,2,3;"  // Success (['1'; '2'; '3'], ";")
run oneOrMoreDigitList "Z;"      // Failure "Expecting '9'. Got 'Z'"

run zeroOrMoreDigitList "1;"     // Success (['1'], ";")
run zeroOrMoreDigitList "1,2;"   // Success (['1'; '2'], ";")
run zeroOrMoreDigitList "1,2,3;" // Success (['1'; '2'; '3'], ";")
run zeroOrMoreDigitList "Z;"     // Success ([], "Z;")