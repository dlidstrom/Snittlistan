module CachedApi

open System

let cachedHttp
    (http: Contracts.RequestStringAsync)
    (database: Database.Gateway)
    (acceptableAge: TimeSpan)
    (request: Contracts.RequestDefinition)
    =
    // database.GetRequest where hash equals and timestamp is ok
    // if cached, return cache
    // else:
    http request
