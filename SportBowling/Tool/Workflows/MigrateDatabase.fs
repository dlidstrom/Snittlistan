namespace Workflows

open System.Reflection
open DbUp

type MigrateDatabase() =
    member _.Run host database username password =
        let upgrader =
            DeployChanges.To
                .PostgresqlDatabase($"Host=%s{host};Database=%s{database};Username=%s{username};Password=%s{password}")
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .Build()
        let result = upgrader.PerformUpgrade()
        if result.Successful
        then
            printfn "Upgrade successful"
            0
        else
            eprintfn "%A" result.Error
            1
