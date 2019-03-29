# AWS SAM command line

[AWS SAM CLI](https://github.com/awslabs/aws-sam-cli)

SAM CLI is an AWS tool to make Lambda development and deployment easier. Currently it is still in Beta.

## Testing

To generate sam samples, use sam cli:

```bash
sam init --runtime:[python2.7|dotnetcore2.1||go1.x|druby2.5|java8|...] --name [project name]
```

The samples are very simple with one http request to http://checkip.amazonaws.com/. If you are accessing the Internet via a proxy server, the test will fail.

### How sam runs Lambda test

A [lambdaci/lambda](https://github.com/lambci/lambci) Docker container is created to run local tests.

### Fixing proxy issues

There is no AWS offially documented way. sam cli uses Python [docker-py](https://github.com/docker/docker-py) to create and run a Docker container. 

To run a container that requires Internet access, you have to make sure:

- Reachable DNS servers
- Use http_proxy and https_proxy environment variables

Typically the docker run command line looks like this:

```bash
docker run -ti \
--dns $my_dns_server \
-e http_proxy=$http_proxy \
-e http_proxy=$https_proxy \
my-image $my_cmd

```

This would fix Python, Go, dotnet runtime and probably most other languages. Java is an exception, it does not automatically use environment variables, it wants System properties to be set:

- http.httpProxy, http.proxyPort
- https.httpProxy, https.proxyPort

```java
private String getPageContents(String address) throws IOException{
        // taking care of proxy: Java ignore http_proxy, https_proxy environ variables
        String httpHost = System.getProperty("http.proxyHost", "");
        String envHttpProxy = "";
        if (System.getenv("http_proxy") != null){envHttpProxy = System.getenv("http_proxy");}
        if (httpHost == "" && envHttpProxy != ""){ // setting Java System wide proxy settings
            URL proxyUrl = new URL(envHttpProxy);
            httpHost = proxyUrl.getHost();
            String httpPort = Integer.toString(proxyUrl.getPort());
            //
            System.setProperty("http.proxyHost", httpHost);
            System.setProperty("http.proxyPort", httpPort);
            //
            System.setProperty("https.proxyHost", httpHost);
            System.setProperty("https.proxyPort", httpPort);
        }
        // original sample starts here
        URL url = new URL(address);
        try(BufferedReader br = new BufferedReader(new InputStreamReader(url.openStream()))) {
            return br.lines().collect(Collectors.joining(System.lineSeparator()));
        }
    }
```

