param([int]$total = 1000, [string]$run_url='http://localhost:8080/run')

$w = [System.Diagnostics.Stopwatch]::StartNew()
$cmd = 'Get-date'
$numbers = 1 .. $total
ForEach ($i in $numbers)
{
	# using silent mode -s
        curl.exe -u folaris:folaris -s -H "Content-Type: application/json" -X POST -d '{"cmd": "get-date"}' $run_url |Out-Null
}
$w.Stop()
$w
"`n" + ('#*^' * 40) + "`n"
"`n`n{2:#,###} calls done in {0:#,###.##0} seconds, average {1:#.##0} calls per second`n" -f $w.Elapsed.TotalSeconds, $($total / $w.Elapsed.TotalSeconds),$total
"=" * 90

<#

# obviously this is slower because we have to create a process every time. When using programming language, we create a process once only

.\curl-test.ps1 -run_url http://192.168.1.250:9876/run -t 100

IsRunning Elapsed          ElapsedMilliseconds ElapsedTicks
--------- -------          ------------------- ------------
    False 00:00:03.9927192                3992     39927192

#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^#*^



100 calls done in 3.993 seconds, average 25.046 calls per second

==========================================================================================
#>