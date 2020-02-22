param([int]$total = 1000)

try {
    [void][RestSharp.Authenticators.HttpBasicAuthenticator]
}catch {
$rsharp = 'C:\Users\bruno\.nuget\packages\restsharp\106.10.1\lib\netstandard2.0\RestSharp.dll'
Add-Type -path $rsharp

 Add-Type -TypeDefinition '
 public class PwshCommand {
        public string cmd {get; set;}
 }
 '
 }
 
Function f {
    [string]$username='folaris'
    [string]$password='folaris'
    $client = [RestSharp.RestClient]::new( 'http://192.168.1.250:9876')
    $client.Authenticator = [RestSharp.Authenticators.HttpBasicAuthenticator]::new($username,$password)	 
    $cmd = [PwshCommand]::new()
    $cmd.cmd = 'Get-Date'
    $req =   [RestSharp.RestRequest]::new("run",  [RestSharp.DataFormat]::Json);
    $req.AddJsonBody($cmd) |out-null
    $client.ExecuteAsPost($req, 'POST')
}

# $total = 1000
$w = [System.Diagnostics.Stopwatch]::StartNew()
$cmd = 'Get-date'
1 .. $total | % {
    $ret = f
}
$w.Stop()
$w
"`n" + ('#*^' * 40) + "`n"
"`n`n{2:#,###} calls done in {0:#,###.##0} seconds, average {1:#.##0} calls per second`n" -f $w.Elapsed.TotalSeconds, $($total / $w.Elapsed.TotalSeconds),$total
"=" * 90

