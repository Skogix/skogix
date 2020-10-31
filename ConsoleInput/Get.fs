module ConsoleInput.Get

open System
open Keybinds
let command = getCommand (Console.ReadKey(true).KeyChar)
  
