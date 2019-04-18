## sqlalchemy

sqlalchemy is widely used. For example, Pandas read_sql uses sqlalchemy to connect to a supported database. 

In this test, I want to transfer all records between two identitical tables. It is useful when you need to move data between 2 non-production environments. This is not the most efficient way to move data, but I will show here that it's very convenient to move small data sets.

### Here is how it works:

- sqlalchemy autoload can retrieve table ddl
- sqlalchemy can generate select query and insert query
- use fetchmany/select and executemany/insert to copy rows from one to another

With a short script, you can copy all the tables you need to mirror.

This is not a good solution to deal with large data set because inserting is really slow even you do all you can with batches. This is just how an RDBMS works. You need use a bulk load tools like SQL*Loader for Oracle, MultiLoad for Teradata, COPY for PostGresql.