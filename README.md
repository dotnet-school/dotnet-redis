

### Todo: 

- [ ] Topics

  - [x] Strings
  - [x] Object as json strings
  - [ ] HashMap
    - [ ] sample use case (update field concurrently)

  - [ ] List
  - [ ] listening to list
  - [ ] Set
  - [ ] Understand `RedisValue`
  - [ ] Connections and configurations
    - [ ] Explain multiplexer vs getDatabase(), cheap pass thorugh and reusable

- [ ] Cookbook

  - [ ] Consensus building
  - [ ] Work distribution

- [ ] Best practices

  - [ ] Wrap IDatabase (for testing/mocking)
  - [ ] Create only one multiplexer

- [ ] Advanced

  - [ ] SSL with redis
  - [ ] mastter slave
  - [ ] kyspace notifications

- [x] Create a client 

- [x] Use `SETENX`

- [x] Save a POCO by its id in redis

- [x] Retrieve a POCO by its id in redis

- [x] Implement connection recovery

  

# Redis with C#



### Contents

- ***[Setup Redis using Docker](00-SetupRedis)***

  > Use Docker to run redis and redis client on local machine

- ***[StackExchange.Redis](01-StackExchangeRedis)***

  > Choose a client library and create a .NET console app

- ***[Strings](./Strings)***

  > Save strings. learn TTL for key expiry, and using NX for writting only when value is null

- ***[Storing Objects As Json Strings](04-ObjectsAsJsonStrings)***

  > Create extensions to build custom logic like serializing/deserializing objects to store them as JSON in Redis.

- ***[Storing Objects as HashMaps](06-ObjectsAsHashMaps)***



### Pre-requisite

- Read basics about redis: 

  > https://redis.io/topics/introduction

- Ensure .NET6 is installed

  > Download : https://dotnet.microsoft.com/download/dotnet/6.0

- Ensure docker is installed ( v3.3.1 or above)

  > Download and install from https://www.docker.com/products/docker-desktop.

- CLI

  > Commands in this arcticle are created for bash. You can use most command in a poweshell. Its recommended to use something like `wsl` or `git bash` if you are running on windows.
  >
  > Download and install gitbash (if required) from : https://git-scm.com/downloads





### References: 

- Redis c# clients: https://docs.redis.com/latest/rs/references/client_references/client_csharp/

- StackExchangeRedis: https://github.com/StackExchange/StackExchange.Redis

- StackExchangeRedis Docs: https://stackexchange.github.io/StackExchange.Redis/

- StackExchangeRedis Best Practices : https://gist.github.com/JonCole/925630df72be1351b21440625ff2671f#stackexchangeredis

  

