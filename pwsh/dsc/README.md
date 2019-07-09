## Powershell Desired State Configuration

#### Invoke-DscResource

[Calling DSC resource methods directly](https://docs.microsoft.com/en-us/powershell/dsc/managing-nodes/directcallresource)

*Required*:

1. Run "winrm quickconfig" to make sure WinRM service is already running on your test computer
1. Example:
```powershell

$props = @{
	DestinationPath = "${env:USERPROFILE}\DirectAccess.txt";
	Contents = 'This file is create by Invoke-DscResource';
}
# Set-TargetResource
$result = Invoke-DscResource -Name File -Method Set -Property $props -ModuleName PSDesiredStateConfiguration -Verbose
# Get-TargetResource
$result = Invoke-DscResource -Name File -Method Get -Property $props -ModuleName PSDesiredStateConfiguration -Verbose
# Test-TargetResource
$result = Invoke-DscResource -Name File -Method Test -Property $props -ModuleName PSDesiredStateConfiguration -Verbose

```

### Interesting 

1. [Some UWP code](https://github.com/HumanEquivalentUnit/PowerShell-Misc)
1. 