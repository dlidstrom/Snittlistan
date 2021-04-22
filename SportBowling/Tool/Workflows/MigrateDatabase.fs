namespace Workflows

open System.Reflection
open DbUp
open DbUp.Engine
open DbUp.Engine.Output

type ConfirmAction = ResizeArray<SqlScript> -> bool

type MigrateDatabase
    (
        connectionString: Database.DatabaseConnection,
        confirmAction: ConfirmAction,
        logger: Contracts.Logger
    ) =
    member _.Run() =
        let log =
            { new IUpgradeLog with
                member _.WriteInformation(fmt, args) =
                    logger.Log "%s" (System.String.Format(fmt, args))

                member _.WriteWarning(fmt, args) =
                    logger.Log "%s" (System.String.Format(fmt, args))

                member _.WriteError(fmt, args) =
                    logger.Log "%s" (System.String.Format(fmt, args)) }

        let upgrader =
            DeployChanges
                .To
                .PostgresqlDatabase(connectionString.Format())
                .LogTo(log)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .Build()

        let scriptsToExecute = upgrader.GetScriptsToExecute()

        if not (Seq.isEmpty scriptsToExecute)
           && (confirmAction scriptsToExecute) then
            let result = upgrader.PerformUpgrade()

            if not result.Successful then
                raise result.Error
