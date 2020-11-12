// dotnet watch run --no-restore -- --api-key 123 --season-id 2006
open System
open Argu

type Arguments =
    | [<Mandatory>] Api_Key of apiKey : string
    | [<Mandatory>] Season_Id of seasonId : int
    | No_Check_Certificate
    | Debug_Http
    | Proxy of proxy : string
    with
        interface IArgParserTemplate with
            member this.Usage =
                match this with
                | Api_Key _ -> "Specifies the ApiKey"
                | Season_Id _ -> "specifies the season id"
                | No_Check_Certificate -> "Don't check the server certificate against the available certificate authorities."
                | Debug_Http -> "Debug HTTP"
                | Proxy _ -> "Specifies the proxy to use."

let run argv =
    let parser = ArgumentParser.Create<Arguments>(programName = AppDomain.CurrentDomain.FriendlyName)
    let results = parser.Parse argv
    let apiKey = results.GetResult(Api_Key)
    let seasonId = results.GetResult(Season_Id)
    let noCheckCertificate = if results.Contains(No_Check_Certificate) then Some true else None
    let debugHttp = results.Contains(Debug_Http)
    let proxy = results.TryGetResult(Proxy)
    let _l = if debugHttp then Some (new Infrastructure.LoggingEventListener()) else None
    let bitsClient = Api.Bits.Client(apiKey, noCheckCertificate, proxy)
    let workflow = Workflows.FetchMatches()
    workflow.Run bitsClient seasonId

[<EntryPoint>]
let main argv =
    try
        run argv
    with
        | :? ArguException as ex ->
            eprintfn "%s" ex.Message
            1
        | ex ->
            eprintfn "%A" ex
            1
