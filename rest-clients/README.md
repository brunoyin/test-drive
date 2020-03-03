# Testing and comparing 7 REST API clients

I had the impression that all HTTP clients are good for testing REST API. It might be true for functional tests. But for performing testing, you need to make sure your REST client is also high performing. In this test, I will try to use a few different clients to run again my [folaris server](https://github.com/brunoyin/folaris.git).

I am going to run tests against a local server running on localhost/loopback and remote server in Docker container

* Powershell/Invoke-Restmethod
* Powershell/Restsharp
* F# Data Http
* Python Requests
* Groovy/JVM
* Golanf
* Node js

## Set up folaris

```bash
# local server:
# 1. git clone https://github.com/brunoyin/folaris.git
# 2. cd [folaris repo]/folaris; dotnet run
#
# remote
# docker run, and warm it up 
docker pull brunoyin/folaris:0.0.5
# start folaris container
docker run -d --name folaris -e FOLARIS_PORT=9876 -p 9876:9876 \
-e FOLARIS_USER=folaris \
-e FOLARIS_PASSWD=folaris \
brunoyin/folaris:0.0.5
# test server is available
export FOLARIS_HOST='192.168.1.250:9876'
export FOLARIS_USER=folaris
export FOLARIS_PASSWD=folaris
curl --header "Content-Type: application/json" \
  --request POST \
  --data '{"cmd":"Get-Date"}' \
  -u $FOLARIS_USER:$FOLARIS_PASSWD \
  $FOLARIS_HOST/run

```

### Powershell REST Test

[source code](pwsh.rest.tests.ps1)

```powershell 
[string]$cmd
[string]$username='folaris'
[string]$password='folaris'
[string]$run_url = 'http://192.168.1.250:9876/run'

# encode password
$base64AuthInfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(("{0}:{1}" -f $username,$password)))
$authHeader = @{Authorization=("Basic {0}" -f $base64AuthInfo)}
# 
$payload = @{cmd = $cmd }|ConvertTo-Json
# call with Basic Authentication
Invoke-RestMethod -Headers $authHeader -Uri $run_url -Method Post -Body $payload -ContentType 'application/json' -UseBasicParsing
```

I used this script to test to [Run 1000 times](pwsh.rest.tests.ps1). The result

```powershell 
.\pwsh.rest.tests.ps1 -total 100
<#
# localhost
1,000 calls done in 1.650 seconds, average 606.236 calls per second

# remote
100 calls done in 5.260 seconds, average 19.011 calls per second
#>
```

## [RestSharp](http://restsharp.org/): a popular dotnet REST API client

[source code](restsharp.tests.ps1)

```powershell

Function f {
    [string]$username='folaris'
    [string]$password='folaris'
    $run_url = 'http://192.168.1.250:9876/run'
    #
    $client = [RestSharp.RestClient]::new($run_url)
    $client.Authenticator = [RestSharp.Authenticators.HttpBasicAuthenticator]::new($username,$password)
    $cmd = [PwshCommand]::new()
    $cmd.cmd = 'Get-Date'
    $req =   [RestSharp.RestRequest]::new("run",  [RestSharp.DataFormat]::Json);
    $req.AddJsonBody($cmd) |out-null
    $client.ExecuteAsPost($req, 'POST')
}

```

I used this script to [run 1000 times](restsharp.tests.ps1). Results

```powershell
.\pwsh.rest.tests.ps1 -total 100
<#
# localhost
1,000 calls done in 1.434 seconds, average 697.507 calls per second

# remote
100 calls done in 5.239 seconds, average 19.088 calls per second
#>
```

## Python requests

[source code](py.requests.tests.py)

```python
import requests
from requests.auth import HTTPBasicAuth

run_url = 'http://192.168.1.250:9876/run'
auth = HTTPBasicAuth("folaris", "folaris")
cmd = {"cmd": 'Get-Date'}
#
def run_folaris():
    ret = requests.post(run_url, json=cmd, auth=auth)

```

Results:

```powershell
python .\py.requests.tests.py 100
<#
# localhost
Total 5 calls done in 10.18 seconds, average 0.49 calls per second

# remote
Total 100 calls done in 1.44 seconds, average 69.04 calls per second
#>
```

## F# data

[source code](fsharp.http.tests.fsx)

```powershell
# set up
paket init
# paket add RestSharp --version 106.10.1
paket add FSharp.Data --version 3.3.3
paket install --generate-load-scripts --load-script-type fsx --load-script-framework netcoreapp2.2

# F# call 
let run_url = "http://192.168.1.250:9876/run"
let headers = [ContentType "application/json"; (BasicAuth "folaris" "folaris") ]
let payload = "{\"cmd\": \"Get-Date\"}"
Http.Request(run_url, httpMethod = HttpMethod.Post, headers=headers, body=TextRequest payload ) |> ignore

```

Results:

```powershell
dotnet fsi .\fsharp.http.tests.fsx 100
<#
# localhost
done -> 5 calls done in 10.297 seconds, average .486 calls per second

# remote
100 calls done in 1.704 seconds, average 58.679 calls per second
#>
```

## Groovy 

[source code](groovy.rest.tests.groovy)

```groovy
// ... 
def total = 1000
def timeStart = new Date()

def folarisSite = new HTTPBuilder( 'http://192.168.1.250:9876/run' )
folarisSite.auth.basic 'folaris', 'folaris'

for(int x=1; x<=total; x++){
    folarisSite.request( POST, JSON ){
        body = [cmd: "Get-Date"]
        response.success = { resp, json ->
            // println "HTTP response code: ${resp.status}."
        }
    }
}
// ....

```

Results

```powershell
# on Windows, run

<#
# localhost
total 1000 calls done in 16 seconds, avg: 62.5 per second

# remote
total 100 calls done in 3 seconds, avg: 33.33 per second
#>
```

## Go

[Source code](goHttpTest.go)

Compiled binaries:

* [Linux](go_linux_restclient)
* [Windows](go_win_restclient.exe)

Results:

```powershell
# on Windows, run
.\go_win_restclient.exe http://localhost:8080/run 10
.\go_win_restclient.exe http://192.168.1.4:8080/run 100

<#
# localhost: 
 command args 1:  10
total:  10
Seconds used:  3.0282483
total 10 calls in 3.028248 seconds, avg 3.302239 calls per seconds

# network interface: http://192.168.1.4:8080/run
command args 1:  100
total:  100
Seconds used:  0.1543947
total 100 calls in 0.154395 seconds, avg 647.690627 calls per seconds
#>
```



## Node js

[Source code](node-rest-client.js)

Results

```powershell
# install dependency before running it
# npm init
# npm install "node-rest-client"
node .\node-rest-client.js http://localhost:8080/run 50
node .\node-rest-client.js http://192.168.1.4:8080/run 50

<#
Total 50 calls done in 0.140113401 seconds, average 356.8538030134605 calls / second


Total 50 calls done in 0.1190855 seconds, average 419.8663985119935 calls / second
#>
```

## Conclusion

The numbers in the result are relative to the hardware. But the comparison is valid.

### When testing with localhost/loopback interface

 Restsharp with 697 calls per second is the clear winner, Powershell with 606 calls per second is in the second place. 

Groovy/Windows in third place is 10 times slower than RestSharp and Powershell/Invoke-RestMethod. 

With 1 call in 2 seconds, both Python and F# do not perform well at all, I will have to remember not use them to run a local performance test.

### When testing with remote Docker container

This is more typical and normal use case. Client performance from best to worst:

1. Golang: 127 calls per second
1. Python requests: 103 calls per second
1. F# Data Http: 103 calls per second
1. Node js: 102 calls per second
1. Groovy/JVM RestClient: 33 calls per second
1. Powershell Invoke-RestMethod: 19 calls per second
1. Powershell/RestSharp: 19 calls per second

While Python and F# struggles with loopback interface, they are the winners when running against remote servers.
