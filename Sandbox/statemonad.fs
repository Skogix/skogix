module Sandbox.statemonad
//
//type State1 = {
//  KV: Map<string, int>
//  counter: int
//}
//let get1 state key =
//  let value = Map.find key state.KV
//  value
//let set1 state key value =
//  let newState = {state with KV = Map.add key value state.KV}
//  newState
//let increment1 state =
//  let newCounter = state.counter + 1
//  let newState = { state with counter = newCounter }
//  (newCounter, newState)
//let test1 state =
//  let a = get1 state "a"
//  let b = get1 state "b"
//  let state1 = set1 state "c" (a+b)
//  let (counter, state2) =
//    increment1 state1
//  (counter, state2)
//let testState1 = {
//  KV = Map.ofList ["a",1; "b",2]
//  counter = 0 }
//let huhu = test1 testState1
//test1 huhu