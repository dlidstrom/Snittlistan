namespace Workflows

open DbUp

type MigrateDatabase() =
    member _.Run host database username password =
        let upgrader =
            DeployChanges.To
                .PostgresqlDatabase($"Host=%s{host}")
                .Build()
        0
