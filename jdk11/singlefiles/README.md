## JDK 11 single source feature is perfect for learning and testing a library

## first, download

1. [OpenCSV](https://mvnrepository.com/artifact/com.opencsv/opencsv/4.5) and dependencies
2. [Postgre JDBC driver](https://mvnrepository.com/artifact/org.postgresql/postgresql/42.2.5)

### Set up
```
# in Powershell

$env:JAVA_HOME = 'D:\tools\jdk11\jdk-11.0.2'
$env:jdk11_libs = 'D:\tools\jdk11\libs\*'

$env:Path = "${env:JAVA_HOME}\bin;${env:Path}"

# $test_drive_root is the the top level of this repo on your computer
# this sets up Postgres db related variables 
. $test_drive_root\docker\postgres\postgres-env-setup.ps1

java --version

openjdk 11.0.2 2019-01-15
OpenJDK Runtime Environment 18.9 (build 11.0.2+9)
OpenJDK 64-Bit Server VM 18.9 (build 11.0.2+9, mixed mode)

```

### running Java single source file

```
java -cp "${env:jdk11_libs}" TheSourceFile.java

```