# RDI API

## Instructions

At the solution directory root

1. Up the databases configured at docker-compose file:
``docker-compose up -d``
   
2. Restore the packages:
``dotnet restore``
   
3. Run the application:
``dotnet run --project src/RDI.API``
   
In order to run the integration tests you need to run the docker-compose command as it has a separated database for it.