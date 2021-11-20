

### Todo: 

- [ ] Setup docker-compose with redis

- [ ] Create a client 

- [ ] Use `SETENX`

- [ ] Save a POCO by its id in redis

- [ ] Retrieve a POCO by its id in redis

- [ ] Implement connection reconnect

  

# Redis with C#



### Chossing a Redis client library : 

Following two seem to be most used and best supported : 

- [StackExchange.Redis](https://github.com/StackExchange/StackExchange.Redis)

  > **Recommended** : We will use this one.

- [ServiceStack.Redis](https://github.com/ServiceStack/ServiceStack.Redis)

  > Not recommended as this is commercial and will create problem when project scales, thorowing up exceptions for use exceeded: https://servicestack.net/download#free-quotas.



### Pre-requisite

- Read basics about redis: 
- Ensure .NET6 is installed
- Ensure docker is installed ( v3.3.1 or above)



### Run redis using docker

- Run redis as a docker container 

  ```bash
  # Persist Redis data here
  mkdir redis-data
  
  docker run \
  		-p 6379:6379\
      -e ALLOW_EMPTY_PASSWORD=yes \
      -v $(pwd)/redis-data:/bitnami/redis/data \
      --name redis \
      -d   \
      bitnami/redis:latest
  ```

- To connect using redis cli : 

  ```bash
  docker exec -it redis redis-cli   
  ```

- If you want to stop your Redis container later on: 

  ```bash
  docker stop redis
  ```

- To re-start when you need to : 

  ```bash
  docker start redis
  ```

- If you want to delete the redis container 

  ```bash
  docker rm redis
  ```

  

### References: 

- https://docs.redis.com/latest/rs/references/client_references/client_csharp/

- https://github.com/StackExchange/StackExchange.Redis

- https://gist.github.com/JonCole/925630df72be1351b21440625ff2671f#stackexchangeredis

  