namespace Skogix

module Core =
  type Result<'yay, 'nay> =
    | Yay of 'yay 
    | Nay of 'nay
  let yay = Result.Yay
  let nay = Result.Yay
  let either yayFunc nayFunc twoWayInput =
    match twoWayInput with
    | Yay y -> yayFunc y
    | Nay n -> nayFunc n
  let bind f = either f Nay
  let ( >>= ) x f = bind f x
  let switch f = f >> yay
  let ( >=> ) switch1 switch2 = switch1 >>= switch2
  let map f = either (f >> yay) nay
  let tee f x = f x; x
  let tryCatch f x =
    try f x |> Yay
    with | ex -> Nay ex.Message
  let map2 yayFunc nayFunc = either (yayFunc >> yay) (nayFunc >> nay)
  let log twoWayInput =
    let yay y = printfn "DEBUG: %A" y;y
    let nay n = printfn "ERROR: %A" n;n
    map2 yay nay twoWayInput
  let plus addYay addNay switch1 switch2 x =
    match (switch1 x), (switch2 x) with
    | Yay y1, Yay y2 -> Yay (addYay y1 y2)
    | Nay n1, Yay _  -> Nay n1
    | Yay _,  Nay n2 -> Nay n2
    | Nay n1, Nay n2 -> Nay (addNay n1 n2)
// paralell check av samma input och concata en string med felmeddelanden
//  let (&&&) v1 v2 =
//    let addYay y1 _ = y1
//    let addNay n1 n2 = n1 + "; " + n2
//    plus addYay addNay v1 v2
  /// function injection

// Constructors      
// yay         1-2
// nay         1-2
// Adapters
// bind        switchar 1-1 -> 2-2
// >>=         bind infix
// switch      1-1 -> 1-2
// map         1-1 -> 2-2
// tee         1-0 -> 1-1
// tryCatch    1-1 -> 1-2 tar exceptions
// doubleMap   (1-1->1-1) -> 1-2 (bimap/map2)
// Combiners   
// >>          vanlig composition
// >=>         switch composition
// plus/<+>    (1-2->1-2) -> 1-2 paralell join
// &&&         plus med and

//  let injectable = if xxx.config then doStuff else id
  
//       ('a -> Result<'b,'c>) -> Result<'a,'c> -> Result<'b,'c>
//       1->2, yay till yay/nay, happy till happy/sad
//       högra funktionen kallas switch function, kan switcha tracks
//
//       bind är lättare att använda för att slotta in en 1->2 till 2->2 workflow
// >>=   bind / 1->2

//       Result<'a,'b> -> ('a -> Result<'c,'b>) -> 'd -> Result<'c,'b>
//       bygg en bind via composition, ta två funktioner och lägg ihop dem
//       if switch1 = yay skicka result till switch2
//       switch funkar bra för att composea fler 1->2
// >=>   switch / 1->2 + 1->2 = 1->2

//       ('a -> 'b) -> Result<'a,'c> -> Result<'b,'c>
//       tar en vanlig 1-1 och gör den till 2-2
// <!>   map / 1-1 -> 2-2
// tee   tar något som returnar unit och pipear den vidare

// |>>   infix map
// <*>   apply
// <|>   orElse
// <?>   label/description
// .>>.  combinator behåll båda
// .>>   behåll vänster
// >>.   behåll höger


