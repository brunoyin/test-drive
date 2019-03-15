#!/usr/bin/pwsh

"building yin/dotnet:2.2 Docker image"
$w = New-Object System.Diagnostics.Stopwatch
$w.Start()

docker build `
--build-arg UID=$(id -u) `
--build-arg GID=$(id -g) `
-t yin/dotnet:2.2 .

$w.Stop()
if ($?){
    "yin/dotnet:2.2 has been built successfully in {0:#,###.##0} seconds" -f $w.Elapsed.TotalSeconds
}else {
    "Failed to build yin/dotnet:2.2, time used: {0:#,###.##0} seconds" -f $w.Elapsed.TotalSeconds
}

<#
Wehn behind corporate firewall

docker build `
--build-arg UID=$(id -u) `
--build-arg GID=$(id -g) `
--build-arg USERNAME=notbruno `
--build-arg GROUPNAME=notbruno `
-e HTTP_PROXY=http://your-company-proxy-server:proxy-port `
-e HTTPS_PROXY=http://your-company-proxy-server:proxy-port `
-e NO_PROXY=169.254.169.254,your-company.com `
-t yin/dotnet:2.2 .
#>