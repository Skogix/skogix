namespace Parser
/// option i stil med some/none
type Result<'a> =
  | Success of 'a
  | Failure of string
/// en map till parser, funkar som en vanlig wrapper
/// för string -> Result<'T * string> till Parser<'T>
/// returnar antingen parsertypen + rest eller failmsg + rest
type Parser<'T> = Parser of (string -> Result<'T * string>)

module Basics =
  open System
  open Common
  /// parsea en char från en str och mappa till parser
  /// char -> Parser<char>
  /// med curry
  /// char -> str -> Parser<char>
  let parseChar (charToParse:char) =
    let f (str:string) =
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
    

