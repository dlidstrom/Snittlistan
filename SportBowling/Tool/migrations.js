var migrations = require('postgres-migrations')

async function Main() {
    const dbConfig = {
        database: "prisma",
        user: "prisma",
        password: "prisma",
        host: "localhost",
        port: 5432,
    }

    await migrations.createDb('prisma', {
        ...dbConfig,
        defaultDatabase: "prisma"
    })
    await migrations.migrate(dbConfig, "Sql")
}

Main()
