__parseChar__ `char -> Parser<char>`<br />
grundfunktionen som skapar en parser från en char

__run__ `Parser<'a> -> string -> Result<'a * string>`<br />
kör inre curryade funktionen hos en parser

__.>>.__ `Parser<'a> -> Parser<'b> -> Parser<'a * 'b>`<br />
__andThen__ : and-combinator

__<|>__ `Parser<'a> -> Parser<'a> -> Parser<'a>`<br />
__orElse__ : or-combinator

__<!>__ `('a -> 'b) -> Parser<'a> -> Parser<'b>`<br />
__mapParse__ : kör en funktion (a->b) som transformar parser<a> -> parser<br />

__|>>__ `Parser<'a> -> ('a -> 'b) -> Parser<'b>`<br />
__mapParse__ : reversed för enklare pipeing

__chooseOne__ `Parser<a'> list -> Parser<'a>`<br />
returnar första success som hittas

__anyOf__ `char list -> Parser<char>`<br />
__parseChar__ >> __chooseOne__

__returnParser__ `a -> Parser<'a>`<br />
höjer en value till en parser

__<*>__ `Parser<('a -> 'b)> -> Parser<'a> -> Parser<'b>`<br />
__applyParser__ : kör en (a->b)-parser på en value 

######ExempelFunktioner
```f# script
let parseDigit = anyOf ['0'..'9']
let parseThreeDigitsAsStr =
    (parseDigit .>>. parseDigit .>>. parseDigit)
    |>> fun ((c1, c2), c3) -> String [|c1;c2;c3|]
let parseThreeDigitsAsInt = mapParse int parseThreeDigitsAsStr
// ('a -> 'b -> 'c) -> Parser<'a> -> Parser<'b> -> Parser<'c>
let lift2 f aValue bValue = returnParser f <*> aValue <*> bValue
// ('a -> 'b -> 'c -> 'd) -> Parser<'a> -> Parser<'b> -> Parser<'c> -> Parser<'d>
let lift3 f aValue bValue cValue = returnParser f <*> aValue <*> bValue <*> cValue
// Parser<int> -> Parser<int> -> Parser<int>
let addParser = lift2 (+)
// string -> char -> bool
let startWith (str:string) (prefix:char) = str.StartsWith(prefix)
// Parser<string> -> Parser<char> -> Parser<bool>
let startsWithParser = lift2 startWith
// Parser<char>
let whitespaceChar = anyOf [' '; '\t'; '\n']
// Parser<char list>
let whitespace = many whitespaceChar

```
