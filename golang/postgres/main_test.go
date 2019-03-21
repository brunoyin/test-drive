package main

import "testing"
import "os"

func TestEnvVariable(t *testing.T) {
	cases := []struct {
		keyName, keyValue string
	}{
		{"PGHOST", "192.168.0.48"},
		{"GOPATH", `D:\bruno\projects\go`},
		{"PGDATABASE", "bruno"},
	}
	for _, c := range cases {
		got := os.Getenv(c.keyName)
		if got != c.keyValue {
			t.Errorf("Variable(%q) == %q, want %q", c.keyName, got, c.keyValue)
		}
	}
}
