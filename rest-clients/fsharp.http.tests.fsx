#load @".paket\load\netcoreapp2.2\FSharp.Data.fsx"

open System
open System.Diagnostics

open FSharp.Data
open FSharp.Data.HttpRequestHeaders

printfn "%A" (Environment.GetCommandLineArgs() )
let run_url, total = 
    match Environment.GetCommandLineArgs() with
    | [|_; _; url; t; |] -> url, Convert.ToInt32(t)
    | _ -> "http://localhost:8080/run", 10

printfn "run_url = %s and total calls = %i" run_url total

let w = Stopwatch.StartNew()
// type PwshCommand = {cmd: String}
// let run_url = "http://192.168.1.250:9876/run"
let headers = [ContentType "application/json"; (BasicAuth "folaris" "folaris") ]
// let cmd = {cmd = "Get-Date"}
let payload = "{\"cmd\": \"Get-Date\"}"

let simplerun() = 
    Http.Request(run_url, httpMethod = HttpMethod.Post, headers=headers, body=TextRequest payload ) |> ignore

// let total = 100
seq {1 .. 1 .. total}
|> Seq.iter (fun _ -> simplerun())
// 
w.Stop()
let ts = w.Elapsed.TotalSeconds
let avg = (Convert.ToDouble(total))/ts
let message = String.Format("{0:#,###} calls done in {1:#,###.##0} seconds, average {2:#.##0} calls per second", total, ts, avg)
printfn "done -> %s" message
