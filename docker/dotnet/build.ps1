#!/usr/bin/pwsh

"building yin/dotnet:2.2 Docker image"
$w = New-Object System.Diagnostics.Stopwatch
$w.Start()

docker build `
--build-arg UID=$(id -u) `
--build-arg GID=$(id -g) `
-t yin/dotnet:2.2 .

if ($?){
    $w.Stop()
    "yin/dotnet:2.2 has been built successfully in {0:#,###.##0} seconds" -f $w.Elapsed.TotalSeconds
}else {
    $w.Stop()
    "Failed to build yin/dotnet:2.2, time used: {0:#,###.##0} seconds" -f $w.Elapsed.TotalSeconds
}

# to test, 
# 1. Make sure postgres docker server is running, set up properly
# $env:PGHOST = '192.168.0.48'
# $env:PGDATABASE = 'bruno'
# $env:PGUSER = 'bruno'
# $env:PGPASSWORD = 'bruno'
# 2. cd $TEST_DRIVE_ROOT/dotnet/CmdlineApp
# 
# docker run -it --rm `
# -e PGHOST=$env:PGHOST  `
# -e PGDATABASE=$env:PGDATABASE `
# -e PGUSER=$env:PGUSER `
# -e PGPASSWORD=$env:PGPASSWORD `
# -v $PWD:/app `
# --workdir /app `
# yin/dotnet:2.2 dotnet run

<#
When behind corporate firewall

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