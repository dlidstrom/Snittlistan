module HopacTests

open System
open Argu

module HopacIntro =
    open Hopac
    open Hopac.Infixes

    type Timing(s : string, ?formatEnd : float -> string) =
        do printfn $"%s{s}"
        let start = DateTime.Now
        let defaultEndFormatter t = $"end: %.3f{t}s"
        let endFormatter =
            Option.defaultValue
                defaultEndFormatter
                formatEnd
        interface IDisposable with
            member _.Dispose() =
                let totalSeconds = (DateTime.Now - start).TotalSeconds
                let s = endFormatter totalSeconds
                printfn $"%s{s}"

    type JobStatus =
        | Started of jobId : int
        | Completed of jobId : int

    type Product = {
        Id : int
        Name : string
    }

    type Review = {
        ProductId : int
        Author : string
        Comment : string
    }

    type ProductWithReviews = {
        Id : int
        Name : string
        Reviews : (string * string) list
    }

    let getProductReviews id = job {

        // Delay in the place of an external HTTP API call
        do! timeOutMillis 3000

        return [
            {ProductId = id; Author = "John"; Comment = "It's awesome!"}
            {ProductId = id; Author = "Sam"; Comment = "Great product"}
        ]
    }

    let getProduct id = job {
        do! timeOutMillis 2000
        return {
            Id = id
            Name = "My Awesome Product"
        }
    }

    let getProductWithReviews id = job {
        let! product = getProduct id // 1
        let! reviews = getProductReviews id // 2
        return {  // 3
            Id = id
            Name = product.Name
            Reviews = reviews |> List.map (fun r -> r.Author, r.Comment)
        }
    }

    let getProductWithReviews2 id = job {
        let! product, reviews =
            getProduct id <*> getProductReviews id // 2
        return {  // 3
            Id = id
            Name = product.Name
            Reviews = reviews |> List.map (fun r -> r.Author, r.Comment)
        }
    }

    let createJob channel jobId = job {
        do! Ch.give channel (Started jobId)
        do! timeOutMillis (jobId * 1000)
        do! Ch.give channel (Completed jobId)
    }

    let printerJob channel = job {
        let! status = Ch.take channel
        match status with
        | Started jobId ->
            printfn $"starting job: %d{jobId}"
        | Completed jobId ->
            printfn $"completed job: %d{jobId}"
    }

    let Run _ =

        use _t = new Timing("start")
        // let longerHelloWorldJob = job {
        //   do! timeOutMillis 2000
        //   printfn "Hello, World!"
        // }
        // run longerHelloWorldJob

        // let jobs = [
        //     createJob 1 4000
        //     createJob 2 3000
        //     createJob 3 2000
        // ]

        // run (Job.conIgnore jobs)

        // run (getProductWithReviews2 1) |> ignore

        let result = run <| job {
            let channel = Ch<JobStatus>()
            let statusPrinter = printerJob channel
            do! Job.foreverServer statusPrinter
            let myJobs = List.init 5 (createJob channel)
            return! Job.conIgnore myJobs
        }

        0

module ApiGateway =
    open Hopac
    open FSharp.Data
    open HttpFs.Client

    type UserTypeProvider = JsonProvider<"https://api.github.com/users/dlidstrom", RootName = "UserProfile">
    type User = UserTypeProvider.UserProfile

    let httpGet : string -> Job<string> =
        fun url ->
            Request.createUrl Get url
            |> Request.setHeader (UserAgent "FsHopac")
            |> getResponse
            |> Job.bind Response.readBodyAsString

    let basePath = "https://api.github.com"

    let getUser : string -> Job<User> =
        fun username ->
            $"%s{basePath}/users/%s{username}"
            |> httpGet
            |> Job.map UserTypeProvider.Parse

    let getUser2 username : Job<User> =
        $"%s{basePath}/users/%s{username}"
        |> httpGet
        |> Job.map UserTypeProvider.Parse

    let Run _ =
        let userProfile = getUser "dlidstrom" |> run
        printfn "%A" userProfile
        0
