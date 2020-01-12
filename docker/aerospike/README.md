
* https://hub.docker.com/r/aerospike/aerospike-tools
* https://hub.docker.com/r/aerospike/aerospike-server
* https://www.aerospike.com/docs/tools/aql/

### configuration 

conf/aerospike.conf

## Docker command
### Server: 
```bash
INFO
      SHOW NAMESPACES | SETS | BINS | INDEXES
      SHOW SCANS | QUERIES
      STAT NAMESPACE <ns> | INDEX <ns> <indexname>
      STAT SYSTEM
      ASINFO <ASInfoCommand>
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
      
