# set up variables to run psql
$env:PGHOST = '192.168.0.48'
$env:PGDATABASE = 'bruno'
$env:PGUSER = 'bruno'
$env:PGPASSWORD = 'bruno'

# On Windows, you need to download Postgres for Windows, server and client can be found in a zip file
$env:Path = "D:\tools\pgsql\bin;${env:Path}"

# On Linux: psql can be installed via package management. No manual changes needed to the PATH environment variable
# $env:PATH 

# USE COPY TO LOAD CSV
# cat .\docker\postgres\bruno.csv | psql -c 'COPY yin.college FROM STDIN CSV HEADER'