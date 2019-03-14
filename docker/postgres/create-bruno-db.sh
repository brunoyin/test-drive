#!/bin/sh
set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    CREATE USER bruno PASSWORD 'bruno';
    CREATE DATABASE bruno;
    GRANT ALL PRIVILEGES ON DATABASE bruno TO bruno;
EOSQL
