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
