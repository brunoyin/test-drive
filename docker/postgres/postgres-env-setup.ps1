# set up variables to run psql
$env:PGHOST = '192.168.0.48'
$env:PGDATABASE = 'bruno'
$env:PGUSER = 'bruno'
$env:PGPASSWORD = 'bruno'

# On Windows, you need to download Postgres for Windows, server and client can be found in a zip file
# https://www.enterprisedb.com/download-postgresql-binaries
$env:Path = "D:\tools\pgsql\bin;${env:Path}"

# On Linux: psql can be installed via package management. No manual changes needed to the PATH environment variable
# $env:PATH 

# USE COPY TO LOAD CSV
# cat .\bruno.csv | psql -c 'COPY yin.college FROM STDIN CSV HEADER'

<#

bruno=> \d yin.college
                          Table "yin.college"
   Column    |          Type          | Collation | Nullable | Default
-------------+------------------------+-----------+----------+---------
 id          | character varying(12)  |           | not null |
 name        | character varying(128) |           | not null |
 city        | character varying(36)  |           | not null |
 state       | character(2)           |           | not null |
 zip         | character varying(16)  |           | not null |
 region      | integer                |           |          |
 latitude    | numeric                |           |          |
 longitude   | numeric                |           |          |
 adm_rate    | numeric                |           |          |
 sat_avg     | numeric                |           |          |
 act_avg     | numeric                |           |          |
 earnings    | numeric                |           |          |
 cost        | numeric                |           |          |
 enrollments | numeric                |           |          |
Indexes:
    "college_pkey" PRIMARY KEY, btree (id)


bruno=> select * from yin.college limit 10;
   id   |                name                 |      city      | state |    zip     | region |      latitude      | longitude  |      adm_rate       | sat_avg | act_avg | earnings |  cost   | enrollments
--------+-------------------------------------+----------------+-------+------------+--------+--------------------+------------+---------------------+---------+---------+----------+---------+-------------
 100654 | Alabama A & M University            | Normal         | AL    | 35762      |      5 | 34.783367999999996 | -86.568502 |              0.8738 |   849.0 |    18.0 |  31000.0 | 22667.0 |      4616.0
 100663 | University of Alabama at Birmingham | Birmingham     | AL    | 35294-0110 |      5 |          33.505697 | -86.799345 |              0.5814 |  1125.0 |    25.0 |  41200.0 | 22684.0 |     12047.0
 100690 | Amridge University                  | Montgomery     | AL    | 36117-3553 |      5 |          32.362609 |  -86.17401 |                     |         |         |  39600.0 | 13380.0 |       293.0
 100706 | University of Alabama in Huntsville | Huntsville     | AL    | 35899      |      5 |          34.724557 | -86.640449 |              0.7628 |  1257.0 |    28.0 |  46700.0 | 22059.0 |      6346.0
 100724 | Alabama State University            | Montgomery     | AL    | 36104-0271 |      5 |          32.364317 | -86.295677 | 0.45899999999999996 |   825.0 |    17.0 |  27700.0 | 19242.0 |      4704.0
 100751 | The University of Alabama           | Tuscaloosa     | AL    | 35487-0166 |      5 |          33.211875 | -87.545978 |              0.5259 |  1202.0 |    27.0 |  44500.0 | 28422.0 |     31663.0
 100760 | Central Alabama Community College   | Alexander City | AL    | 35010      |      5 |           32.92478 | -85.945266 |                     |         |         |  27700.0 | 13868.0 |      1492.0
 100812 | Athens State University             | Athens         | AL    | 35611      |      5 |          34.806793 | -86.964698 |                     |         |         |  38700.0 |         |      2888.0
 100830 | Auburn University at Montgomery     | Montgomery     | AL    | 36117-3596 |      5 |           32.36736 | -86.177544 |              0.7659 |  1009.0 |    22.0 |  33300.0 | 19255.0 |      4171.0
 100858 | Auburn University                   | Auburn         | AL    | 36849      |      5 |          32.599378 | -85.488258 |              0.8054 |  1217.0 |    27.0 |  48800.0 | 29794.0 |     22095.0
(10 rows)
#>