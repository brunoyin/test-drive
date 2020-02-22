param([int]$total = 1000)

Function f {
<#
.DESCRIPTION
	Folaris Powershell Command Line
.PARAMETER cmd
	this can be any Powershell cmdlet, Command Line utilities and Powershell scripts
.PARAMETER run_url
	default to 'http://localhost:8080/run'
.EXAMPLE
	# list all environment variables
	Invoke-RemotePwsh -cmd 'gci env:'
	# using pipeline
	'gci env:' |Invoke-RemotePwsh
	# test error handling with an invalid Command
	'Get-Command dude' |Invoke-RemotePwsh
#>
	[cmdletbinding()]
	param( 
		[parameter(Mandatory = $true, ValueFromPipeline = $true)]
		[string]$cmd,
		[string]$username='folaris',
		[string]$password='folaris',
		[string]$run_url = 'http://192.168.1.250:9876/run'
	)
	# encode password
	$base64AuthInfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(("{0}:{1}" -f $username,$password)))
	$authHeader = @{Authorization=("Basic {0}" -f $base64AuthInfo)}
	# 
	$payload = @{cmd = $cmd }|ConvertTo-Json
	# call with Basic Authentication
	Invoke-RestMethod -Headers $authHeader -Uri $run_url -Method Post -Body $payload -UseBasicParsing
}



# 1000 in Parallel run, this is not 1000 concurrent
$w = [System.Diagnostics.Stopwatch]::StartNew()
$cmd = 'Get-date'
$numbers = 1 .. $total
ForEach ($i in $numbers)
{
	$ret = f -cmd $cmd # -run_url 'http://192.168.1.250:9876/run'
}
$w.Stop()
$w
"`n" + ('#*^' * 40) + "`n"
"`n`n{2:#,###} calls done in {0:#,###.##0} seconds, average {1:#.##0} calls per second`n" -f $w.Elapsed.TotalSeconds, $($total / $w.Elapsed.TotalSeconds),$total
"=" * 90

