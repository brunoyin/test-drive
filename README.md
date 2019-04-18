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

## [Apache Airflow](./python/airflow): workflow automation and scheduling

Each Workflow, or a DAG, is written in Python code. Each task is still executed by a worker in a separate process, that can be any OS command. It does not run on Windows natively. It can run in a Linux subsystem on Windows.

- [A test DAG](python/airflow/dags/fetch_load.py)
- [A test plugin](python/airflow/plugins/flickrplugin.py), or the code in a separate module so that a DAG is more readable

## [sqlalchemy](./python/sqlalchemy): copy identical tables with fewer than 500K records using sqlachemy

A test project that does:

- use sqlalchemy to generate select and insert queries
- use batches to improve performance

Check out [my blog post](https://brunoyin.gitlab.io/post/20190416-sqlalchemy-table-copy/) about how to deal with big data sets.

## [Poetry](./python/development/db2csv) managed package project:

Using [poetry](https://github.com/sdispater/poetry) to manage dependencies and virtualenv
