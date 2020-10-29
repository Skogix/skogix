module ParserTests

open NUnit.Framework
open Basics

let parseA = parseChar 'a'
let parseB = parseChar 'b'
let parseC = parseChar 'c'
let parseAThenB = parseA .>>. parseB

[<SetUp>]
let Setup () =
    ()

[<Test>]
let parseChar () =
    Assert.AreEqual(run parseA "abc", Success('a', "bc"))
    // todo fulhaxx med tostring, är just nu object vs result-tuple
    Assert.AreEqual((run parseA "bbb").ToString(), Failure(sprintf "Ville ha a, fick b").ToString())
[<Test>]
let AndThen () =
    Assert.AreEqual(run parseAThenB "abc", Success(('a', 'b'), "c"))
    Assert.AreEqual((run parseAThenB "a").ToString(), Failure("Inge mer input").ToString())
    Assert.AreEqual((run parseAThenB "axx").ToString(), Failure(sprintf "Ville ha b, fick x").ToString())
    Assert.AreEqual((run parseAThenB "xxx").ToString(), Failure(sprintf "Ville ha a, fick x").ToString())
