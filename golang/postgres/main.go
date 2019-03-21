package main

import (
	"database/sql"
	"encoding/csv"
	"fmt"
	"log"
	"os"

	_ "github.com/lib/pq"
)

func main() {
	db, err := sql.Open("postgres", "host=192.168.0.48 user=bruno password=bruno dbname=bruno sslmode=disable")
	checkError("Failed to connect db\n", err)
	defer db.Close()

	rows, err := db.Query("select * from yin.college limit 1")
	checkError("Query failed\n", err)

	// show column names
	cs, err := rows.Columns()
	checkError("Failed to get columns\n", err)
	fmt.Printf("\n%v\n", cs)

	// single row
	var name string
	err1 := db.QueryRow("select name from yin.college limit 1").Scan(&name)
	switch {
	case err1 == sql.ErrNoRows:
		log.Printf("nothing returned")
	case err1 != nil:
		checkError("db.QueryRow did not return anything\n", err1)
	default:
		fmt.Printf("college name = %s\n", name)
	}

	// Query
	rows1, err := db.Query("select * from yin.college limit 20")
	checkError("query error", err)

	file, err := os.Create("results.csv")
	checkError("could not create results.csv\n", err)
	defer file.Close()

	writer := csv.NewWriter(file)
	defer writer.Flush()

	for rows1.Next() {
		var id, name, city, state, zip string
		var region int
		var latitude, longitude, adm_rate, sat_avg, act_avg, earnings, cost, enrollments sql.NullFloat64
		if e := rows1.Scan(&id, &name, &city, &state, &zip, &region, &latitude, &longitude, &adm_rate, &sat_avg, &act_avg, &earnings, &cost, &enrollments); e != nil {
			checkError("row scan error", e)
		}
		value := []string{id, name, city, state, zip, fmt.Sprintf("%d", region),
			convert2str(latitude), convert2str(longitude), convert2str(adm_rate), convert2str(sat_avg), convert2str(act_avg),
			convert2str(earnings), convert2str(cost), convert2str(enrollments)}
		err := writer.Write(value)
		checkError("Error write row", err)
	}
	fmt.Printf("\ndone\n")
}

func convert2str(v sql.NullFloat64) string {
	var strVal = ""
	if v.Valid {
		var tmpVal, err = v.Value()
		if err == nil {
			strVal = fmt.Sprintf("%f", tmpVal)
		}
	}
	return strVal
}

func checkError(message string, err error) {
	if err != nil {
		log.Fatal(message, err)
	}
}
