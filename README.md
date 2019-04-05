# test-drive
**Trying out new technologies**

## Lab setup
[Details](./docker/postgres/)
1. Build a Postgres Docker image that creates a user and a database at startup
2. Populate a table using College Scorecard data from US Government 

## Development environment setup
1. Download dotnet core and JDK 11
2. Download PostgresSQL client 
3. Use [the sample Powershell script](./dev-env-startup-sample.ps1) to set up dotnet core, JDK and Postgres db server access

## [Testing dotnet core 2.2](./dotnet)
- A simple console app
- An xUnit test project
- Cake or C# Make: writing code inside cake scripts

## [Testing JDK](./jdk11)

- Single-source feature test
- A Maven test project targeting JDK 11

## [Golang](./golang)

- A simple Postgressql db access Go project

## [Kafka](./kafka)

- A [faust: Python Kafka Stream Processing](https://github.com/robinhood/faust) test project

## [aws-sam-cli: AWS Lamba Serverless Application Model Command Line tool](./aws/sam)

- A Golang sample project
- A dotnet core 2.1 sample project
- A Java project

If you are behind a firewall, these sample projects will fail. I documented my workaround to run tests successfully in local dev environment behind a proxy server.