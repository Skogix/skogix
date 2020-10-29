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
  /// parsea en char från en str och mappa till parser
  /// char -> Parser<char>
  /// med curry
  /// char -> str -> Parser<char>
  let parseChar (charToParse:char) =
    // todo: jävla fult att göra till array och tillbaka till list, kolla upp snyggare lösning
    let f (str:string) =
      str.ToCharArray()
      |> Array.toList
      |> function
        | first::rest ->
          if first = charToParse then Success(charToParse, rest.ToString())
          else Failure(sprintf "Ville ha %c, fick %c" charToParse first)
        | [] -> Failure ("Ingen mer input")
    Parser f
(*
    // currya med inre funktion
    // todo: kanske mer funktionellt att göra en match first::rest / []
    let f (str:string) =
      // om dem ska gå att chaina så kan det komma tom input
      if String.IsNullOrEmpty(str) then
        Failure "Ingen mer input"
      else
        let first = str.[0]
        if first = charToParse then
          let rest = str.[1..]
          Success (charToParse, rest)
        else
          Failure (sprintf "Ville ha %c, fick %c" charToParse first)
    Parser f
*)
  /// "unwrapper" för parser, kör i princip sett bara inre funktionen i parsern som passats
  let run (parser:Parser<'a>) (input:string): Result<'a * string> =
    // deconstructar parser precis som en (x,y) skulle deconstructa en tuple
    let (Parser runInnerFunction) = parser
    runInnerFunction input
    

