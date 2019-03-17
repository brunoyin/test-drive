# this is a Powershell script that works on Windows and Linux
# 

# unzipped OpenJDK path 
$env:JAVA_HOME = 'D:\tools\jdk11\jdk-11.0.2'
# download Java jar files for PostgresSql and OpenCSV and dependancies
$env:jdk11_libs = 'D:\tools\jdk11\libs\*'

$env:Path = "${env:JAVA_HOME}\bin;${env:Path}"

# Postgres db server
$env:PGHOST = '192.168.0.48'
$env:PGDATABASE = 'bruno'
$env:PGUSER = 'bruno'
$env:PGPASSWORD = 'bruno'

# On Windows, you need to download Postgres for Windows, server and client can be found in a zip file
# https://www.enterprisedb.com/download-postgresql-binaries
try { Get-Command psql -ErrorAction Stop}catch {  $env:Path = "D:\tools\pgsql\bin;${env:Path}" }

# sending temp files to designated directory
$env:TEST_TMP_DIR = 'D:\bruno\temp'
# for dotnet, if you download zip instead of installer, you need to do:
# 1.  set up DOTNET_ROOT variable
# $env:DOTNET_ROOT = 'C:\mydotnet'
# 2. set up path
# $env:Path = "${env:DOTNET_ROOT};${env:Path}"

# I installed dotnet SDK using installer, I do not need to set anything up for dotnet
# check dotnet version:
# dotnet --version
# check Java version:
# java -version
