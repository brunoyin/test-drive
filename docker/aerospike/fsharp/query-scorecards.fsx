#load ".paket/load/netcoreapp3.1/main.group.fsx"

open Aerospike.Client

let clientPolicy = ClientPolicy()
clientPolicy.timeout <- 150000
// 
printfn "Connecting to server ..."
let client = new AerospikeClient(clientPolicy, "192.168.1.250", 3000)
let ns = "YIN"
let setName = "college"
// 
let stmt = Statement()
stmt.SetNamespace ns
stmt.SetSetName setName
// stmt.SetIndexName "collegename_index"
stmt.SetBinNames [|"INSTNM"; "CITY";|]
stmt.SetPredExp(
  (PredExp.StringBin "INSTNM"),
  (PredExp.StringValue "carol"), 
  (PredExp.StringRegex RegexFlag.ICASE)
)
// 
printfn "Running query ..."
let rs = client.Query(null, stmt) 
while rs.Next() do
    printfn "Name: %s, City: %s" (rs.Record.GetString "INSTNM") (rs.Record.GetString "CITY")
rs.Close()
printfn "Done"