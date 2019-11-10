#Requires -Version 6.2.3 
$nugetTestPath = $PWD
cp $env:APPDATA\nuget\nuget.config $nugetTestPath
$testNugetConfig = Join-Path $nugetTestPath nuget.config
nuget config -set repositoryPath=$nugetTestPath\packages -configfile $testNugetConfig
# install Aerospike.Client => Retrieving package 'Aerospike.Client 3.9.1' from 'nuget.org'
nuget install Aerospike.Client -configfile $testNugetConfig
# 
$aerospike_client_dll = "$nugetTestPath\packages\Aerospike.Client.3.9.1\lib\netstandard\AerospikeClient.dll"
$dll = Add-type -Path $aerospike_client_dll  -PassThru

<#
The field or property: "readPolicyDefault" for type: "Aerospike.Client.AerospikeClient" differs only in letter casing from the field or property: "ReadPolicyDefault". The type must be Common Language Specification (CLS) compliant.
At line:9 char:5
+     $client.Put($policy, $key, $bins[0],$bins[1],$bins[2],$bins[3],$b ...
+     ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
+ CategoryInfo          : NotSpecified: (:) [], ExtendedTypeSystemException
+ FullyQualifiedErrorId : NotACLSComplaintField
#>

$clientPolicy = [Aerospike.Client.ClientPolicy]::new()
$clientPolicy.timeout = 15000

$client = [Aerospike.Client.AerospikeClient]::new($clientPolicy, '192.168.1.250', 3000)
$namespace = [string]'YIN'
$setName = [string]'college'

$ErrorActionPreference = 'Stop'
$policy = [Aerospike.Client.WritePolicy]::new()
$policy.SetTimeout(300)
$policy.maxRetries = 5
$policy.totalTimeout = 5000
# $policy.recordExistsAction = [Aerospike.Client.RecordExistsAction]::UPDATE
$policy.recordExistsAction = [Aerospike.Client.RecordExistsAction]::REPLACE
$policy.sendKey = $true

$recs = Import-Csv .\scorecard-recent.csv
$props = $recs[0]|Get-Member -MemberType NoteProperty |Select-Object -ExpandProperty name
$recs | ForEach-Object  {
    $rec = $_
    # Write-Host $($rec  )
    $key = [Aerospike.Client.Key]::new($namespace, $setName, [string]$rec.UNITID)
    $bins = $props | ForEach-Object {
        $propName = $_
        [Aerospike.Client.Bin]::new($propName, $rec.$propName) 
    }
    $client.Put($policy, $key, $bins[0],$bins[1],$bins[2],$bins[3],$bins[4],$bins[5],$bins[6],$bins[7],$bins[8],$bins[9],$bins[10],$bins[11],$bins[12],$bins[13]) 
    Write-Host($rec.INSTNM)
}
$client.Close()



