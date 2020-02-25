package main

/*
$env:GOOS='windows'
go build

// or
$env;GOOS='linux'
go build

*/

import (
        "os"
        "strconv"
	"fmt"
        "encoding/json"
        "bytes"
	 //"io/ioutil"
	"net/http"
	// "net/url"
         "log"
         "time"
)


var client = &http.Client{}
// var url = "http://localhost:8080/run"

func Run(url string) error {
    h := json.RawMessage(`{"cmd": "Get-Date"}`)
    req, err := http.NewRequest("POST", url, bytes.NewBuffer(h))
    if err != nil {
            fmt.Println("error in creating http req: ", err.Error())
            return err
    }
    req.Header.Set("Content-Type", "application/json")
    req.SetBasicAuth("folaris", "folaris")
    _, err = client.Do(req)
    if err != nil {
            fmt.Println("error in send req: ", err.Error())
    }
    return err
}

// toHttpTest "http://192.168.1.250:9876/run" 100
// toHttpTest "http://localhost:8080/run" 100
// toHttpTest "http://192.168.1.4:8080/run" 100
func main() {
        t1 := time.Now()
        url := os.Args[1]
        fmt.Println("command args 1: ", os.Args[2])
        total, err := strconv.Atoi(os.Args[2])
        fmt.Println("total: ", total)
        if err != nil {
		log.Fatal(err)
	}
        //
        for i := 0; i < total; i++ {
                // fmt.Println("i: ", i)
                Run(url) 
	}
        ts := time.Now().Sub(t1).Seconds()
        fmt.Println("Seconds used: ", ts)
        
	fmt.Printf("total %d calls in %f seconds, avg %f calls per seconds\n", total, ts, float64(total)/ts)
	
}