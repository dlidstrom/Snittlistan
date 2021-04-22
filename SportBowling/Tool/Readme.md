# Usage

## Publish

```bash
dotnet publish --runtime osx.11.0-x64 -p:PublishSingleFile=true --self-contained true --configuration Release
```

## Todo

- Fix storing of requests, and the hash column which is md5(url+method+body)
- Fix request caching by reading from database before doing http (check timestamp)
