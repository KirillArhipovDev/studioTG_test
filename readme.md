# Команды для контейнера
docker build -t webapi:latest .

docker run -d -p 8080:8080 -p 8081:8081 --name webapi_container webapi:latest

# URL API
http://localhost:8080/api
