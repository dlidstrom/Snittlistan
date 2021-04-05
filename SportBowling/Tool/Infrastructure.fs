module Infrastructure

open System
open System.Diagnostics.Tracing
open System.IO
open System.Reflection

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
            raise (Exception($"Resource %s{name} not found. Available resources: %s{availableResources}", ex))

module BitsHttp =
    open System.Net
    open FSharp.Data
    open FSharp.Json

    let create (proxy : string option) noCheckCertificate =
        let customizeRequest (req : HttpWebRequest) =
            proxy |> function
            | Some s -> req.Proxy <- WebProxy(s, true)
            | _ -> ()
            noCheckCertificate |> function
            | Some b when b ->
                req.ServerCertificateValidationCallback <-
                    fun _sender _certificate _chain _policyErrors -> true
            | _ -> ()
            req

        fun (request : Contracts.RequestDefinition) ->
            match request.Method with
            | Contracts.Method.Get ->
                Http.AsyncRequestString(
                    url = request.Url,
                    httpMethod = HttpMethod.Get,
                    headers = request.Headers,
                    customizeHttpRequest = customizeRequest)
            | Contracts.Method.Post ->
                Http.AsyncRequestString(
                    url = request.Url,
                    httpMethod = HttpMethod.Post,
                    body = TextRequest (Json.serialize request.Body),
                    headers = request.Headers,
                    customizeHttpRequest = customizeRequest)
