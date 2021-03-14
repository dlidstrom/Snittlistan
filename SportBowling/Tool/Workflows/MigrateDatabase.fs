namespace Workflows

open System.Reflection
open DbUp
open DbUp.Engine

type ConfirmAction = ResizeArray<SqlScript> -> bool

type MigrateDatabase(confirmAction : ConfirmAction) =
    member _.Run (connectionString : Database.DatabaseConnection) =
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
            if result.Successful then
                None
            else
                Some result.Error
        else
            None
