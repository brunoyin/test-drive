package helloworld;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.io.IOException;
import java.net.URL;
import java.util.HashMap;
import java.util.Map;
import java.util.stream.Collectors;

import com.amazonaws.services.lambda.runtime.Context;
import com.amazonaws.services.lambda.runtime.RequestHandler;

/**
 * Handler for requests to Lambda function.
 */
public class App implements RequestHandler<Object, Object> {

    public Object handleRequest(final Object input, final Context context) {
        Map<String, String> headers = new HashMap<>();
        headers.put("Content-Type", "application/json");
        headers.put("X-Custom-Header", "application/json");
        try {
            final String pageContents = this.getPageContents("https://checkip.amazonaws.com");
            String output = String.format("{ \"message\": \"hello world\", \"location\": \"%s\" }", pageContents);
            return new GatewayResponse(output, headers, 200);
        } catch (IOException e) {
            return new GatewayResponse("{}", headers, 500);
        }
    }

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
        URL url = new URL(address);
        try(BufferedReader br = new BufferedReader(new InputStreamReader(url.openStream()))) {
            return br.lines().collect(Collectors.joining(System.lineSeparator()));
        }
    }
}
