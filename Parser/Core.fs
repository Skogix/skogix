module Parser.Core
open System
/// resultatet av en parseing
type Result<'a> =
  | Success of 'a
  | Failure of string
/// wrapper till alla parsers
type Parser<'T> = Parser of (string -> Result<'T * string>)
/// parsear en char
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
/// kör en parser med input
let run p str =
  // deconstructar parser precis som en (x,y) skulle deconstructa en tuple
  let (Parser inF) = p
  inF str
/// input är diagonalt (a -> parser<b>)
/// output är horisontell (parser<a> -> parser<b>)
/// tar en f som gör en p, en p och kör
/// p och skickar output av p till f
let bindParser f p =
  let inF str =
    let result1 = run p str
    match result1 with
    | Failure err -> Failure err
    | Success (firstIn, restIn) ->
      // kör f för att få en ny parser
      let p2 = f firstIn
      // kör parsern med resten av input
      run p2 restIn
  Parser inF
/// infix av bindParser
let ( >>= ) p f = bindParser f p
/// transforma en normal value till parser, t.ex a -> parser<a>
let returnParser a =
  let inF b = Success (a, b)
  Parser inF
/// mappar a -> b till parser<a> -> parser<b>
let mapParse f = bindParser (f >> returnParser)
/// infix av mapParse
let ( <!> ) = mapParse
/// infix av mapParse men reversead för pipelineing
let ( |>> ) x f = mapParse f x
/// transformar en parser som har en funktion, t.ex parser<a->b> -> parser<a> -> parser<b>
let applyParser fP xP =
  fP >>= (fun f ->
  xP >>= (fun x ->
    returnParser (f x)))
let ( <*> ) = applyParser
// lyfter en f(a->b->c) till parsers
let lift2 f aParser bParser =
  returnParser f <*> aParser <*> bParser
let andThen p1 p2 =
  p1 >>= (fun p1Result ->
  p2 >>= (fun p2Result ->
    returnParser (p1Result, p2Result)))
/// infix för andThen
let ( .>>. ) = andThen
let orElse p1 p2 =
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
// väljer en parser från listan
let chooseOne ps =
  List.reduce (<|>) ps
// väljer en char från listan
let anyOf chars =
  chars
  |> List.map parseChar
  |> chooseOne
/// tar en lista med parsers och mappar till en parser av en lista
let rec seqParsers ps =
  // todo: hemmagjort consfunktion, kolla om det går att göra snyggae
  let splitCons first rest = first::rest
  // lyfter parser<'a> till parser<'a list>
  let consParser = lift2 splitCons
  match ps with
  | [] -> returnParser []
  | first::rest -> consParser first (seqParsers rest)
/// parsea något tills fail / kör tills något hittas eller failar
let rec parseZeroOrMore p input =
  let result1 = run p input
  match result1 with
  | Failure _ -> ([], input)
  // (valuen som parseas, resten av input 1)
  | Success (x, restIn) ->
    // (resten av alla values från innan, resten av input 2)
    let (xs, restOut) =
      // kör så länge det är success
      parseZeroOrMore p restIn
    // skicka ut nya values när det kommer hit
    let values = x::xs
    // (alla values som hittades, resten efter fail)
    (values, restOut)
/// matchar 0 eller mer av en parser
let many p =
  let rec inF str =
    // parsea input och wrappa i success
    Success(parseZeroOrMore p str)
  Parser inF
/// matchar minst en av en parser
let many1 p =
  let rec inF str =
    let result1 = run p str
    match result1 with
    | Failure err -> Failure err
    | Success (x, restIn) ->
      let(xs, restOut) =
        parseZeroOrMore p restIn
      let values = x::xs
      Success (values, restOut)
  Parser inF
/// parsear något optional -> some <|> none
let oneOrZero p =
  let some = p |>> Some
  let none = returnParser None
  some <|> none
/// behåller vänster
let (.>>) p1 p2 = p1 .>>. p2 |> mapParse (fun (a,_) -> a)
/// behåller höger
let (>>.) p1 p2 = p1 .>>. p2 |> mapParse (fun (_,b) -> b)
/// behåller mitten
let between p1 p2 p3 = p1 >>. p2 .>> p3

/// parsear 1+ av p som separeras av sep
let separatedByOne p sep =
  let sepThenParse = sep >>. p
  p .>>. many sepThenParse
  |>> fun (p, ps) -> p::ps
// parsear 0+ av p som separeras av sep
let separateBy p sep =
  separatedByOne p sep <|> returnParser []
  
  
  
  
  
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
/// parsea en int
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
