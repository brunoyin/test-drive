## using Paket to generate fsi dependency load script

```bash
# making sure you have paket installed
dotnet tool install -g paket
# 
paket init
paket add Aerospike.Client --version 3.9.2 
paket add FSharp.Data --version 3.3.3
# generate load scripts
paket install --load-script-type fsx --load-script-framework netcoreapp3.1 --generate-load-scripts
```
### run dotnet fsi

```bash
dotnet fsi query-scorecards.fsx

```
