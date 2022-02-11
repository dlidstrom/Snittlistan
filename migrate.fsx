#r "nuget: dbup-postgresql"
#r "nuget: Npgsql"

open System
open DbUp
open Npgsql

let connectionString =
  if fsi.CommandLineArgs.Length = 2 then
    fsi.CommandLineArgs.[1]
  else
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

let scriptsToExecute = upgradeEngine.GetScriptsToExecute()
if scriptsToExecute.Count > 0 then
    printfn "Found these scripts to execute:"
    scriptsToExecute |> Seq.iter (fun s -> printfn $"%s{s.Name}")

    printfn "Run these scripts? Enter 'yes' to proceed."
    if Console.ReadLine() = "yes" then
        let result = upgradeEngine.PerformUpgrade()

        if not result.Successful then
            printfn $"%A{result.Error.Message}"
else
    printfn "No scripts found to execute, migration already complete"
