__parseChar__ `char -> Parser<char>`<br />
grundfunktionen som skapar en parser från en char

__run__ `Parser<'a> -> string -> Result<'a * string>`<br />
kör inre curryade funktionen hos en parser

__.>>.__ `Parser<'a> -> Parser<'b> -> Parser<'a * 'b>`<br />
__andThen__ : and-combinator

__<|>__ `Parser<'a> -> Parser<'a> -> Parser<'a>`<br />
__orElse__ : or-combinator

__<!>__ `('a -> 'b) -> Parser<'a> -> Parser<'b>`<br />
__mapParse__ : kör en funktion (a->b) som transformar parser<a> -> parser<b>

__|>>__ `Parser<'a> -> ('a -> 'b) -> Parser<'b>`<br />
__mapParse__ : reversed för enklare pipeing

__choice__ `Parser<a'> list -> Parser<'a>`<br />
returnar första success som hittas

__anyOf__ `char list -> Parser<char>`<br />
__parseChar__ >> __choice__

__returnParser__ `a -> Parser<'a>`<br />
höjer en value till en parser

__applyParser__ `Parser<('a -> 'b)> -> Parser<'a> -> Parser<'b>`<br />
kör en (a->b)-parser på en value 