namespace Workflows

open System.Reflection
open DbUp
open DbUp.Engine
open DbUp.Engine.Output
open NLog.FSharp

type ConfirmAction = ResizeArray<SqlScript> -> bool

type MigrateDatabase(
                     connectionString : Database.DatabaseConnection,
                     confirmAction : ConfirmAction,
                     log : ILogger) =
    member _.Run () =
        let log = { new IUpgradeLog with
            member this.WriteInformation(fmt, args) =
                log.Info "%s" (System.String.Format(fmt, args))
            member this.WriteWarning(fmt, args) =
                log.Info "%s" (System.String.Format(fmt, args))
            member this.WriteError(fmt, args) =
                log.Info "%s" (System.String.Format(fmt, args)) }
        let upgrader =
            DeployChanges.To
                .PostgresqlDatabase(connectionString.Format())
                .LogTo(log)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .Build()
        let scriptsToExecute = upgrader.GetScriptsToExecute()
        if not (Seq.isEmpty scriptsToExecute) && (confirmAction scriptsToExecute)
        then
            let result = upgrader.PerformUpgrade()
            if not result.Successful then raise result.Error
