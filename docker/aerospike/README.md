## Running Aerospike server in Docker

*  [aerospike-tools on Docker Hub](https://hub.docker.com/r/aerospike/aerospike-tools)
*  [aerospike-server on Docker Hub](https://hub.docker.com/r/aerospike/aerospike-server)
*  [Aerospike AQL reference](https://www.aerospike.com/docs/tools/aql/)

### configuration 

conf/aerospike.conf

## Docker command
### Server: 
```bash
docker run -d --name aerospike \
	-p 3000:3000 -p 3001:3001 -p 3002:3002 -p 3003:3003 \
	-v $PWD/conf:/opt/aerospike/etc \
	--ulimit nofile=500000:500000 \
	aerospike/aerospike-server:4.5.3.5 \
	asd --foreground --config-file /opt/aerospike/etc/aerospike.conf

```

### Tools

```bash
docker run -ti --rm aerospike/aerospike-tools:3.21.1 \
        aql -h 192.168.1.250 --no-config-file
```

### aql 

Create index first before you run a query:

create index state_idx on YIN.college(State) string

select Name, City from YIN.college where State = 'NC'
 
 INFO
      SHOW NAMESPACES | SETS | BINS | INDEXES
      SHOW SCANS | QUERIES
      STAT NAMESPACE <ns> | INDEX <ns> <indexname>
      STAT SYSTEM
      ASINFO <ASInfoCommand>
      
## Using F# to run some simple tests

* [F# Visual Studio Project](./ftest)
* [F# Interactive](./fsharp)
