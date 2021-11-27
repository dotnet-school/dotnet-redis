# Run Redis using docker

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

