module GameEngine.Common

let slowConsoleWrite msg =
  msg
  |> String.iter (fun ch ->
    System.Threading.Thread.Sleep(5)
    printf "%c" ch
    if(ch = ';') then printfn "")
