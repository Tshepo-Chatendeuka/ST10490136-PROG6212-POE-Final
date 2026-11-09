# RaceDay - Event Management System
Complete system containing API, MVC Web Application, and Docker setup.

## Running the API
```bash
cd RaceDay.Api
dotnet run --urls http://0.0.0.0:5000
```

## Running the MVC Application with Docker
```bash
docker build -f RaceDay.Mvc/Dockerfile -t raceday-mvc:part3 .
docker run -d --name raceday-mvc -p 8080:8080 -e ApiSettings_BaseUrl=http://host.docker.internal:5000/ raceday-mvc:part3
```
