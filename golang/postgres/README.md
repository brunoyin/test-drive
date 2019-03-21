## Testing Go 

- Use Go to access Postgres db
- Use Go to run a query and save the results to a csv file

### Preparation

- make sure Postgres Docker container is up and running
- Go is properly set up: go executable is on the search PATH and GOPATH is set and pointing to 
    a valid OS directory path

Check [the sample dev ennv setup script](../../dev-env-startup-sample.ps1)

### DB access

- database/sql
- driver: github.com/lib/pq

Everything seems to be straightforward. Nullable columns do need extra handling. 

For people who worked on dotnet. It's similar to dotnet, one can test: "value is  System.DBNull", or SqlDataReader.IsDBNull(Int32) == true. 

In Go, sqlNullXxxx has "valid" when the value is not NULL. Use a call "Value()" to get the actual value:

```Golang
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

```

### Save results to a csv file

To run:

```Bash
go run ./main.go
```

### Running tests

[main_test.go](./main_test.go)

```bash
go test

```

