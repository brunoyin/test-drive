# Using cflinuxfs3 to ready app for PCF binary buildpack

## Preparing Aerospike sever to run in a PCF container

* Download Aerospike server for Ubuntu 18: wget https://www.aerospike.com/download/server/4.6.0.4/artifact/ubuntu18

* Unpack the *.deb files

```bash
mkdir ./aerospike-root
cd aerospike-root/
dpkg-deb -xv ../aerospike-server-community-4.6.0.4.ubuntu18.04.x86_64.deb ./
dpkg-deb -xv ../aerospike-tools-3.21.1.ubuntu18.04.x86_64.deb ./
# => etc  opt  usr
mkdir conf
vi conf/aerospike.conf
# copy the content belore
mkdir run
# to store pid file
```

## Start a cflinuxfs3 container with unpacked binary mounted to /aerospike

Start a container:

```bash
docker run --rm -ti \
        -v $PWD/aerospike-root:/aerospike \
        cloudfoundry/cflinuxfs3:0.151.0 bash
```

## Start Aerospike server to test

```bash
# aerospike server configuration: proto-fd-max 1500
ulimit -n 4080

# because I mounted to /aerospike
# mod-lua -> user-path /aerospike/opt/aerospike/usr/udf/lua
asd --foreground --config-file /aerospike/conf/aerospike.conf

```

aerospike configuration file:

```c
/*
service {
        user root
        group root
        paxos-single-replica-limit 1 # Number of nodes where the replica count is automatically reduced to 1.
        # !important override the default
        pidfile /aerospike/run/asd.pid
        # !important to override default
        work-directory /aerospike/opt/aerospike
        service-threads 2
        transaction-queues 2
        transaction-threads-per-queue 4
        # !important override the default 15000 => ulimit -n 4080 => which greater the configured number here
        proto-fd-max 1500
}

logging {

        # Log file must be an absolute path.
        file /dev/null {
                context any info
        }

        # Send log messages to stdout
        console {
                context any info
        }
}

network {
        service {
                address any
                port 3000

                # Uncomment the following to set the `access-address` parameter to the
                # IP address of the Docker host. This will the allow the server to correctly
                # publish the address which applications and other nodes in the cluster to
                # use when addressing this node.
                # access-address <IPADDR>
        }

        heartbeat {

        address any
                # mesh is used for environments that do not support multicast
                mode mesh
                port 3002

                # use asinfo -v 'tip:host=<ADDR>;port=3002' to inform cluster of
                # other mesh nodes

                interval 150
                timeout 10
        }

        fabric {
            address any
                port 3001
        }

        info {
            address any
                port 3003
        }
}

mod-lua {
        user-path /aerospike/opt/aerospike/usr/udf/lua
}

namespace YIN {
        replication-factor 2
        memory-size 512M
        default-ttl 0 #30d # 5 days, use 0 to never expire/evict.

        storage-engine memory

        # To use file storage backing, comment out the line above and use the
        # following lines instead.

}
*/
```