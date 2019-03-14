## Testing dotnet data access

1. use dotnet EF to generate context and table entity code
2. create a Console app project with 2 db access components using EF raw sql query and plain old Npgsql
3. create an xUnit test project to test the db access components

### generate code in pwsh
``
$cnstr = "Host={0};Database={1};Username={2};Password={3}" -f $env:PGHOST,$env:PGDATABASE,$env:PGUSER,$env:PGPASSWORD
dotnet ef dbcontext scaffold $cnstr Npgsql.EntityFrameworkCore.PostgreSQL
```