namespace Workflows

open System.Reflection
open DbUp
open DbUp.Engine

type ConfirmAction = ResizeArray<SqlScript> -> bool

type MigrateDatabase(confirmAction : ConfirmAction) =
    member _.Run connection =
        let upgrader =
            DeployChanges.To
                .PostgresqlDatabase(connection.ToString())
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .Build()
        let scriptsToExecute = upgrader.GetScriptsToExecute()
        if Seq.length scriptsToExecute > 0 && (confirmAction scriptsToExecute)
        then
            let result = upgrader.PerformUpgrade()
            if result.Successful then
                None
            else
                Some result.Error
        else
            None
