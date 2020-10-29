module Basics
/// option i stil med some/none
type Result<'a> =
  | Success of 'a
  | Failure of string
/// en map till parser, funkar som en vanlig wrapper
/// för string -> Result<'T * string> till Parser<'T>
/// returnar antingen parsertypen + rest eller failmsg + rest
type Parser<'T> = Parser of (string -> Result<'T * string>)
/// parsea en char från en str och mappa till parser
/// char -> Parser<char>
/// med curry
/// char -> str -> Parser<char>
let parseChar charToParse =
  let f (str:string) =
    // todo: kolla upp om det är något att vinna på implicit currying
    // för just nu gör det bara att det är svårt att se på signatur vad som händer
    // och behöver en unwrapper extra
    match Seq.toList str with
      | first::rest ->
        if first = charToParse then
          Success(charToParse, rest |> string)
        else
          Failure(sprintf "Ville ha %c, fick %c" charToParse first)
      | [] -> Failure ("Ingen mer input")
  Parser f
/// "unwrapper" för parser, kör i princip sett bara inre funktionen i parsern som passats
let run (parser:Parser<'a>) (input:string): Result<'a * string> =
  // deconstructar parser precis som en (x,y) skulle deconstructa en tuple
  let (Parser runInnerFunction) = parser
  runInnerFunction input

/// todo
/// vill kunna >> två parsers, typ parseA >> parseB
/// så se till att output och input är samma och "låta fails" gå förbi
/// todo kolla bind-lösning
///
/// kör första parsern
///   om fail, return
/// kör andra parsern med rest
///   om fail, return
/// om ingen fail, returna en tuple med båda parseade values

let andThen (parser1:Parser<'a>) (parser2:Parser<'a>) =
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
  f
/// infix för andThen
let ( .>>. ) = andThen

