module CachedApi

open System

let cachedHttp
    (http: Contracts.RequestStringAsync)
    (database: Database.Gateway)
    (request: Contracts.RequestDefinition)
    (acceptableAge: TimeSpan)
    =
    // database.GetRequest where hash equals and timestamp is ok
    // if cached, return cache
    // else:
    http request
