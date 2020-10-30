module GameEngine.Common

let slowConsoleWrite msg =
  msg
  |> String.iter (fun ch ->
    System.Threading.Thread.Sleep(100)
    System.Console.Write ch)
