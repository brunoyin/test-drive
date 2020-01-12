// Learn more about F# at http://fsharp.org

open System
open System.Diagnostics
open FSharp.Data
open Aerospike.Client

type Choices = LOAD | QUERY | DELETE
//
type CsvCollege = CsvProvider<"D:/bruno/projects/powershell/aerospike/scorecard-recent.csv", HasHeaders=true>

let dataFilename = "D:/bruno/projects/powershell/aerospike/scorecard-recent.csv"
//
let asServerName = "192.168.1.250"
let asServerPort = 3000
//
let ns = "YIN"
let setName = "college"

let writePolicy = WritePolicy()
writePolicy.SetTimeout 300
writePolicy.maxRetries <- 5
writePolicy.totalTimeout <- 5000
writePolicy.recordExistsAction <- RecordExistsAction.REPLACE
writePolicy.sendKey <- true

let checkCSVFile =
    let csv = CsvCollege.Load(dataFilename)
    let headers = 
      match csv.Headers with
      | Some hdr -> hdr
      | None -> [|"";|]
    // printfn "%s" headers.[0]
    headers |> String.concat "," |> printfn "%A"
    let row = csv.Rows |> Seq.head
    printfn "First row: %s / %s" row.CITY row.STABBR
    ()

let loadCollege (client: AerospikeClient) : int =
    let csv = CsvCollege.Load(dataFilename)
    let headers = 
      match csv.Headers with
      | Some hdr -> hdr
      | None -> [|"";|]
    printfn "%s" headers.[0]
    //
    csv.Rows |> Seq.iter ( fun row -> 
        let key = Key(ns, setName, row.UNITID)
        let bin1, bin2, bin3, bin4, bin5 = Bin("Name", row.INSTNM), Bin("City", row.CITY), Bin("Latitide", row.LATITUDE), Bin("Longitude", row.LONGITUDE),Bin("State", row.STABBR)
        let bin6, bin7, bin8, bin9, bin10 = Bin("Region", row.REGION), Bin("ACT", row.ACTCMMID), Bin("SAT", row.SAT_AVG), Bin("AdminRate", row.ADM_RATE),Bin("Cost", row.COSTT4_A)
        let bin11, bin12, bin13 = Bin("Earning", row.MD_EARN_WNE_P10), Bin("StudentTotal", row.UGDS),Bin("ZipCode", row.ZIP)
        client.Put(writePolicy, key, bin1,bin2,bin3,bin4,bin5,bin6,bin7,bin8,bin9,bin10,bin11,bin12,bin13)
        //row.INSTNM
        //let item = loadCollege client writePolicy r
        printfn "done: %s" row.INSTNM
    )
    0
    
let runQuery (client: AerospikeClient) (name : string) : int =
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
    0

let deleteAll (client: AerospikeClient) : int =
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
    0

[<EntryPoint>]
let main argv =
    checkCSVFile
    let w = Stopwatch.StartNew()
    let clientPolicy = ClientPolicy()
    clientPolicy.timeout <- 150000
    let client = new AerospikeClient(clientPolicy, asServerName, asServerPort)
    printfn "Connected to server %s in %f seconds " asServerName w.Elapsed.TotalSeconds
    //
    // let mutable ret : int = 0
    let ret: int =
        match argv with
        | [|actionType;|] -> 
            match actionType with
            | "load" -> (loadCollege client )
            | "delete" -> (loadCollege client )
            | "query" -> (runQuery client "carol")
            | _ -> -2
            //if actionType = "load"  then (loadCollege client )
            //elif actionType = "delete" then (deleteAll client )
            //elif actionType = "query" then (runQuery client "carol")
            //else  -2
        | _ -> -1
    //printfn "Hello World from F#!"
    
    // loadCollege client 
    //
    // deleteAll client
    // 0 // return an integer exit code
    //let ret = runQuery client "carol"
    if ret < 0 then
        printfn "Invalid command line options. Please use one of these values: load, query, delete"
    w.Stop()
    printfn "All done in %f seconds " w.Elapsed.TotalSeconds
    ret
