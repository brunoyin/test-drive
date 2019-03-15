<#
Function runTest{
    Param(
        [scriptblock]$Script
    )
    $w = New-Object System.Diagnostics.Stopwatch
    $w.Start()
    & $Script
    $w.Stop()
    "`n{0:#,###.#0} milli seconds used`n" -f $w.Elapsed.TotalMilliseconds
}

$efqueryTest = {
    "`n-----`nRunning EFQuery"
    dotnet run -n cali -qt ef
}

$simplequeryTest = {
    "`n-----`nRunning SimpleQuery"
    dotnet run -n cali
}


runTest -Script $efqueryTest
runTest -Script $simplequeryTest

runTest -Script $simplequeryTest
runTest -Script $efqueryTest
#>

$testEF = {    
    dotnet run -n cali -qt ef
}

$testSimple = {
    dotnet run -n cali
}

Function runTest{
    Param(
        [scriptblock]$Script
    )
    $w = New-Object System.Diagnostics.Stopwatch
    $w.Start()
    & $Script | Out-Null
    $w.Stop()
    # "`n{0:#,###.#0} milli seconds used`n" -f $w.Elapsed.TotalMilliseconds
    $w.Elapsed.TotalMilliseconds
}
 
$results = @()

1 .. 25 | % {
    $a = runTest -Script $testEF
    $b = runTest -Script $testSimple

    $results += New-Object -TypeName psobject -Property @{EF=$a; Simple=$b; Difference=$b-$a }
}

$results

<#
ADO.Net query is a little bit faster

Difference    Simple        EF
----------    ------        --
 -890.4726 2003.5534  2894.026
 -790.5495 2026.9623 2817.5118
 -803.0365 2023.1614 2826.1979
 -683.2189 2137.6329 2820.8518
 -787.4604 2067.4506  2854.911
 -873.2238 2066.3301 2939.5539
 -828.5192 2011.5325 2840.0517
 -788.0466 2070.0426 2858.0892
 -757.0905 2055.0067 2812.0972
 -745.6193   2085.95 2831.5693
 -812.5492 2031.2828  2843.832
 -778.6706  2036.892 2815.5626
 -779.2398 2032.2943 2811.5341
 -763.1625 2045.6339 2808.7964
 -911.0323 2029.7009 2940.7332
 -821.3651 1992.6945 2814.0596
 -802.1631 2011.2111 2813.3742
 -781.8481   2038.95 2820.7981
 -787.0569 2059.4261  2846.483
 -773.6388 2036.2037 2809.8425
 -794.3785 2027.4133 2821.7918
 -811.3318 2031.5567 2842.8885
 -811.1682 2000.7685 2811.9367
 -809.3262 2014.9895 2824.3157
 -763.4612  2029.836 2793.2972
#>
