#r "nuget: dbup-postgresql"
#r "nuget: Npgsql"

open DbUp
open Npgsql

let connectionString =
    "Host=localhost;Port=5432;Database=snittlistan;Username=postgres;Password=postgres"

let conn = new NpgsqlConnection(connectionString)
conn.Open()

for schema in [ "snittlistan" ] do
    let cmdText =
        $"CREATE SCHEMA IF NOT EXISTS %s{schema}"

    printfn $"%s{cmdText}"
    let cmd = new NpgsqlCommand(cmdText, conn)

    cmd.ExecuteNonQuery() |> ignore<int>

let upgradeEngine =
    DeployChanges
        .To
        .PostgresqlDatabase(connectionString)
        .JournalToPostgresqlTable("snittlistan", "migrations")
        .WithScriptsFromFileSystem("Sql/")
        .LogToConsole()
        .Build()

let result = upgradeEngine.PerformUpgrade()

if not result.Successful then
    printfn $"%A{result.Error.Message}"
