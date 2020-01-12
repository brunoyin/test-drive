open System

printfn "%A" fsi.CommandLineArgs
// let scriptname::scriptArgs = fsi.CommandLineArgs
// printfn "scriptname: %A" scriptname
// printfn "scriptArgs: %A" scriptArgs

match fsi.CommandLineArgs with
| [| scriptname;|] -> printfn "scriptname: %A" scriptname
| _ -> printfn "scriptArgs: %A"  fsi.CommandLineArgs.[1 .. ]
// | x::xs  -> printfn "scriptArgs: %A" xs
// | _ -> failwith "Invalid command line"