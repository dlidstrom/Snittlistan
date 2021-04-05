namespace Workflows

open System.Reflection
open DbUp
open DbUp.Engine

type ConfirmAction = ResizeArray<SqlScript> -> bool

type MigrateDatabase(
                     connectionString : Database.DatabaseConnection,
                     confirmAction : ConfirmAction) =
    member _.Run () =
        let upgrader =
            DeployChanges.To
                .PostgresqlDatabase(connectionString.Format())
                .LogToConsole()
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .Build()
        let scriptsToExecute = upgrader.GetScriptsToExecute()
        if not (Seq.isEmpty scriptsToExecute) && (confirmAction scriptsToExecute)
        then
            let result = upgrader.PerformUpgrade()
            if not result.Successful then raise result.Error
