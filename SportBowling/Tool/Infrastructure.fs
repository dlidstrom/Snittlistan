module Infrastructure

open System
open System.Diagnostics.Tracing
open System.IO
open System.Reflection
open Microsoft.EntityFrameworkCore.Design
open Microsoft.EntityFrameworkCore.Migrations

// https://developers.redhat.com/blog/2019/12/23/tracing-net-core-applications/
type LoggingEventListener() =
    inherit EventListener() with
        override this.OnEventSourceCreated(eventSource) =
            if eventSource.Name <> "System.Threading.Tasks.TplEventSource"
                && eventSource.Name <> "System.Buffers.ArrayPoolEventSource" then
                this.EnableEvents(eventSource, EventLevel.LogAlways, EventKeywords.All)
        override _.OnEventWritten(eventData) =
            printfn
                "%s - %s : %s"
                eventData.EventSource.Name
                eventData.EventName
                (eventData.Payload
                    |> Seq.zip eventData.PayloadNames
                    |> Seq.map string
                    |> String.concat ",")

let readFromResource name =
    let assembly = Assembly.GetExecutingAssembly()
    try
        use stream = assembly.GetManifestResourceStream(name)
        use reader = new StreamReader(stream)
        reader.ReadToEnd()
    with
        | ex ->
            let availableResources =
                assembly.GetManifestResourceNames()
                |> String.concat ", "
            raise (new Exception($"Resource %s{name} not found. Available resources: %s{availableResources}", ex))

module Entities =
    open Argu

    type Arguments =
        | [<Mandatory>] Host of host : string
        | [<Mandatory>] Database of database : string
        | [<Mandatory>] Username of username : string
        | [<Mandatory>] Password of password : string
        with
            interface IArgParserTemplate with
                member this.Usage =
                    match this with
                    | Host _ -> "Specifies the database host."
                    | Database _ -> "Specifies the database name."
                    | Username _ -> "Specifies the username"
                    | Password _ -> "Specifies the password."


    type ContextFactory() =

        interface IDesignTimeDbContextFactory<Entities.Context> with
            member _.CreateDbContext argv =
                let parser = ArgumentParser.Create<Arguments>(AppDomain.CurrentDomain.FriendlyName)
                let results = parser.Parse argv
                let host = results.GetResult Host
                let database = results.GetResult Database
                let username = results.GetResult Username
                let password = results.GetResult Password
                let connectionString =
                    $"Host=%s{host};Database=%s{database};Username=%s{username};Password=%s{password}"

                new Entities.Context(connectionString)

module Migrations =
    type Initial() =
        inherit Migration() with
            override _.Up builder =
                builder.Sql(readFromResource "0001.sql")
                |> ignore
