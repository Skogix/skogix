module ConsoleInput.Keybinds
let getCommand inputChar =
  match inputChar with
  | ',' -> "Move Up"
  | 'o' -> "Move Down"
  | 'a' -> "Move Left"
  | 'e' -> "Move Right"
  | _ -> ""
  


