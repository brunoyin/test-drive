docker run -d --name aerospike \
	-p 3000:3000 -p 3001:3001 -p 3002:3002 -p 3003:3003 \
	-v $PWD/conf:/opt/aerospike/etc \
	--ulimit nofile=500000:500000 \
	aerospike/aerospike-server:4.5.3.5 \
	asd --foreground --config-file /opt/aerospike/etc/aerospike.conf
