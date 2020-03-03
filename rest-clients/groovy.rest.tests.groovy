@Grab('org.codehaus.groovy.modules.http-builder:http-builder:0.7')
// @Grab('oauth.signpost:signpost-core:1.2.1.2')
// @Grab('oauth.signpost:signpost-commonshttp4:1.2.1.2')
 
// import java.text.*

import groovyx.net.http.RESTClient
import groovyx.net.http.HTTPBuilder

import static groovyx.net.http.ContentType.*
import static groovyx.net.http.Method.POST
// import static groovyx.net.http.ContentType.JSON

import groovy.time.*

def total = 1

def cli = new CliBuilder(usage: 'groovy.rest.tests.groovy -[t]')
cli.with {
    h longOpt: 'help', 'Show usage information'
    t longOpt: 'total', args: 1, argName: 'total', 'Total REST calls'
    u longOpt: 'url', args: 1, argName: 'url', 'folaris'
}
def options = cli.parse(args)
if (options.h) { cli.usage() }
if (options.total){
    total = options.t as Integer
}
def folarisUrl = 'http://localhost:8080/run'
if (options.url){
    folarisUrl = options.u
}
println "folarisUrl = ${options.url} toal = ${options.t}"

def timeStart = new Date()

// def folarisSite = new HTTPBuilder( 'http://localhost:8080/run' )
def folarisSite = new HTTPBuilder(folarisUrl)
folarisSite.auth.basic 'folaris', 'folaris'

for(int x=1; x<=total; x++){
    folarisSite.request( POST, JSON ){
        body = [cmd: "Get-Date"]
        response.success = { resp, json ->
            // println "HTTP response code: ${resp.status}."
        }
    }
}


def timeStop = new Date()
TimeDuration duration = TimeCategory.minus(timeStop, timeStart)
// println duration.seconds
def totalSeconds = duration.toMilliseconds() / 1000.0
def avg = total / totalSeconds
println "folarisUrl = ${folarisUrl} toal = ${total}"
println "total ${total} calls done in ${totalSeconds} seconds, avg: ${avg} per second"