#load ".paket/load/netcoreapp3.1/main.group.fsx"

open System
open System.Threading
open System.Diagnostics
open FSharp.Data
open Aerospike.Client

//
type CsvCollege = CsvProvider<"../scorecard-recent.csv", HasHeaders=true>

let dataFilename = "../scorecard-recent.csv"
//
// let asServerName = "13.92.208.142"
let asServerName = "192.168.1.250"
let asServerPort = 3000
//
let ns = "YIN"
let setName = "college"

let clientPolicy = ClientPolicy()
clientPolicy.timeout <- 15000

let writePolicy = WritePolicy()
writePolicy.SetTimeout 300
writePolicy.maxRetries <- 5
writePolicy.totalTimeout <- 5000
writePolicy.recordExistsAction <- RecordExistsAction.REPLACE
writePolicy.sendKey <- true

type WriteHandler(client: AsyncClient, policy: WritePolicy , key: Key , bin: Bin)=
    interface WriteListener with
        member this.OnSuccess(key: Key) = 
            printfn "Loaded OK: %s %s %A" key.ns key.setName key.userKey
            ()
        member this.OnFailure(e: AerospikeException) = 
            printfn "Failed to: %s %s %A" key.ns key.setName key.userKey
            printfn "%A" e
            ()

let checkCSVFile =
    //let csv = CsvCollege.Load(dataFilename)
    //let headers = 
    //  match csv.Headers with
    //  | Some hdr -> hdr
    //  | None -> [|"";|]
    //// printfn "%s" headers.[0]
    //headers |> String.concat "," |> printfn "%A"
    //let row = csv.Rows |> Seq.head
    //printfn "First row: %s / %s" row.CITY row.STABBR
    ()

let connectSync (servernName: string) (serverPort : int) : AerospikeClient = 
    let w = Stopwatch.StartNew()
    let c = new AerospikeClient(clientPolicy, servernName, serverPort)
    w.Stop()
    printfn "Connected to server %s in %f seconds " servernName w.Elapsed.TotalSeconds
    c

let connectASync (servernName: string) (serverPort : int) : AsyncClient = 
    let w = Stopwatch.StartNew()
    let asyncPolicy = new AsyncClientPolicy()
    asyncPolicy.timeout <- 15000
    let c = new AsyncClient(asyncPolicy, servernName, serverPort)
    w.Stop()
    printfn "Connected to server %s in %f seconds " servernName w.Elapsed.TotalSeconds
    c

// using CancellationTokenSource -> Token: fast without feedback
let loadCollegeToken() : int =
    let client: AsyncClient = connectASync asServerName asServerPort
    let csv = CsvCollege.Load(dataFilename)
    let headers = 
      match csv.Headers with
      | Some hdr -> hdr
      | None -> [|"";|]
    printfn "%s" headers.[1]
    //
    let source = new CancellationTokenSource()
    let insertAsync (row:CsvCollege.Row) = 
        async {
            let key = Key(ns, setName, row.UNITID)
            let bin1, bin2, bin3, bin4, bin5 = Bin("Name", row.INSTNM), Bin("City", row.CITY), Bin("Latitide", row.LATITUDE), Bin("Longitude", row.LONGITUDE),Bin("State", row.STABBR)
            let bin6, bin7, bin8, bin9, bin10 = Bin("Region", row.REGION), Bin("ACT", row.ACTCMMID), Bin("SAT", row.SAT_AVG), Bin("AdminRate", row.ADM_RATE),Bin("Cost", row.COSTT4_A)
            let bin11, bin12, bin13 = Bin("Earning", row.MD_EARN_WNE_P10), Bin("StudentTotal", row.UGDS),Bin("ZipCode", row.ZIP)
            //let writeListener: WriteListener = WriteHandler(client,writePolicy,key, bin1 ) :> WriteListener
            let! ok = client.Put(writePolicy, source.Token, key, bin1,bin2,bin3,bin4,bin5,bin6,bin7,bin8,bin9,bin10,bin11,bin12,bin13) |> Async.AwaitTask
            //row.INSTNM
            //let item = loadCollege client writePolicy r
            // printfn "done: %s" row.INSTNM
            ()
        }
    csv.Rows
    |> Seq.map insertAsync
    |> Async.Parallel
    |> Async.Ignore
    |> Async.RunSynchronously
    //
    client.Close()
    //
    0

let loadCollegeAsync() : int =
    let client: AsyncClient = connectASync asServerName asServerPort
    let csv = CsvCollege.Load(dataFilename)
    let headers = 
      match csv.Headers with
      | Some hdr -> hdr
      | None -> [|"";|]
    printfn "%s" headers.[1]
    //
    let insertAsync (row:CsvCollege.Row) = 
        async {
            let key = Key(ns, setName, row.UNITID)
            let bin1, bin2, bin3, bin4, bin5 = Bin("Name", row.INSTNM), Bin("City", row.CITY), Bin("Latitide", row.LATITUDE), Bin("Longitude", row.LONGITUDE),Bin("State", row.STABBR)
            let bin6, bin7, bin8, bin9, bin10 = Bin("Region", row.REGION), Bin("ACT", row.ACTCMMID), Bin("SAT", row.SAT_AVG), Bin("AdminRate", row.ADM_RATE),Bin("Cost", row.COSTT4_A)
            let bin11, bin12, bin13 = Bin("Earning", row.MD_EARN_WNE_P10), Bin("StudentTotal", row.UGDS),Bin("ZipCode", row.ZIP)
            let writeListener: WriteListener = WriteHandler(client,writePolicy,key, bin1 ) :> WriteListener
            // let source = new CancellationTokenSource()
            client.Put(writePolicy, writeListener, key, bin1,bin2,bin3,bin4,bin5,bin6,bin7,bin8,bin9,bin10,bin11,bin12,bin13) // |> Async.AwaitTask
            //row.INSTNM
            //let item = loadCollege client writePolicy r
            // printfn "done: %s" row.INSTNM
        }
    csv.Rows
    |> Seq.map insertAsync
    |> Async.Parallel
    |> Async.Ignore
    |> Async.RunSynchronously
    //
    client.Close()
    //
    0

let loadCollege (): int =
    let client: AerospikeClient = connectSync asServerName asServerPort
    let csv = CsvCollege.Load(dataFilename)
    let headers = 
      match csv.Headers with
      | Some hdr -> hdr
      | None -> [|"";|]
    printfn "%s" headers.[0]
    // csv.Rows |> Seq.iter ( fun row -> 
    //    printfn "done: %s" row.INSTNM
    // )
    //
    for row in csv.Rows do
        printfn "starting: %s" row.INSTNM
        let key = Key(ns, setName, row.UNITID)
        let bin1, bin2, bin3, bin4, bin5 = Bin("Name", row.INSTNM), Bin("City", row.CITY), Bin("Latitide", row.LATITUDE), Bin("Longitude", row.LONGITUDE),Bin("State", row.STABBR)
        let bin6, bin7, bin8, bin9, bin10 = Bin("Region", row.REGION), Bin("ACT", row.ACTCMMID), Bin("SAT", row.SAT_AVG), Bin("AdminRate", row.ADM_RATE),Bin("Cost", row.COSTT4_A)
        let bin11, bin12, bin13 = Bin("Earning", row.MD_EARN_WNE_P10), Bin("StudentTotal", row.UGDS),Bin("ZipCode", row.ZIP)
        client.Put(writePolicy, key, bin1,bin2,bin3,bin4,bin5,bin6,bin7,bin8,bin9,bin10,bin11,bin12,bin13) |> ignore
        //row.INSTNM
        //let item = loadCollege client writePolicy r
        printfn "done: %s" row.INSTNM
    client.Close() |> ignore
    0
    
let runQuery(name : string) : int =
    // let client: AerospikeClient = connectSync asServerName asServerPort
    let w = Stopwatch.StartNew()
    let client = new AerospikeClient(clientPolicy, asServerName, asServerPort)
    w.Stop()
    printfn "Connected to server %s in %f seconds " asServerName w.Elapsed.TotalSeconds
    printfn "Connected. Ready for querying: %s" name
    let w = Stopwatch.StartNew()
    let stmt = Statement()
    stmt.SetNamespace ns
    stmt.SetSetName setName
    // stmt.SetIndexName "collegename_index"
    stmt.SetBinNames [|"Name"; "City";|]
    stmt.SetPredExp(
      (PredExp.StringBin "Name"),
      (PredExp.StringValue name), 
      (PredExp.StringRegex RegexFlag.ICASE)
    )
    // 
    printfn "Running query ..."
    let rs = client.Query(null, stmt) 
    w.Stop()
    printfn "Time user %f seconds used to run query" w.Elapsed.TotalSeconds
    w.Reset()
    w.Start()
    while rs.Next() do
        printfn "Name: %s, City: %s" (rs.Record.GetString "Name") (rs.Record.GetString "City")
    rs.Close()
    w.Stop()
    printfn "Done looping result set in %f seconds " w.Elapsed.TotalSeconds
    client.Close()
    0

let deleteAll (): int =
    let client: AerospikeClient = connectSync asServerName asServerPort
    let scanPol = ScanPolicy()
    scanPol.concurrentNodes <- true
    scanPol.priority <- Priority.LOW
    let cb =  ScanCallback(fun key record -> 
        let name = record.GetString("Name")
        let message =
            if client.Delete(writePolicy, key) then "Deleted " + name
            else "Failed to delete " + name
        printfn "%s" message
        () // unit or void
    )
    client.ScanNode(scanPol, client.Nodes.[0], ns, setName, cb, "Name")
    client.Close() |> ignore
    0

// [<EntryPoint>]
let main argv =
    // checkCSVFile
    let w = Stopwatch.StartNew()
    // let ret = 0
    // let client = new AerospikeClient(clientPolicy, asServerName, asServerPort)
    //
    // let mutable ret : int = 0
    let ret: int =
        match argv with
        | [|actionType;|] -> 
            match actionType with
            | "load" -> (loadCollege () )
            | "async" -> (loadCollegeAsync () )
            | "token" -> (loadCollegeToken () )
            | "delete" -> ( deleteAll () )
            | "query" -> (runQuery "carol")
            | _ -> -2
            //if actionType = "load"  then (loadCollege client )
            //elif actionType = "delete" then (deleteAll client )
            //elif actionType = "query" then (runQuery client "carol")
            //else  -2
        | _ -> -1
        
    if ret < 0 then
        printfn "Invalid command line options. Please use one of these values: load, query, delete"
    w.Stop()
    printfn "All done in %f seconds " w.Elapsed.TotalSeconds
    ret

printfn "%A" fsi.CommandLineArgs
match fsi.CommandLineArgs with
| [| scriptName; |] -> printfn "Syntax: dotnet fsi %s [|async | load | query | delete]" scriptName
| _ -> main fsi.CommandLineArgs.[1 .. ] |> ignore
