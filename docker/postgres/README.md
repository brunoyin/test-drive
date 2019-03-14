## Build an image that creates a new user and a database

1. make sure the bash script is executable
2. Make sure no DOS/Windows new line characters

## Load some data from US Government college scorecard 

Real World data to make it a little bit more interesting: college name, location, average SAT/ACT admitted, cost, earnings after 10 years.

## build docker image

```
docker build -t yin/postgres:11.2 .
```

### start postgres docker container in Bash

docker run -e POSTGRES_PASSWORD=bruno \
-p 5432:5432 \
--name yin-postgres -d \
yin/postgres:11.2