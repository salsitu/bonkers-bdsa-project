#!/usr/bin/env bash

PROJECT="Server"
ENTITIES="Server.Entities"
DATABASE="sql_server"
SQL_PASS=$(uuidgen)
CONNECTION_STRING="Server=localhost;Database=$DATABASE;User Id=sa;Password=$SQL_PASS;Trusted_Connection=False;Encrypt=True;TrustServerCertificate=true"

echo "Configuring Connection String"
dotnet user-secrets init --project $PROJECT
dotnet user-secrets set "ConnectionStrings:$DATABASE" "$CONNECTION_STRING" --project $PROJECT > /dev/null

echo "Starting SQL Server"
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$SQL_PASS" -p 1433:1433 --name $DATABASE -d mcr.microsoft.com/mssql/server:2019-latest
sleep 5
dotnet ef database update --project $PROJECT

echo "Starting ProjectBank (press Ctrl+C to stop)"
dotnet run --project $PROJECT

echo "Stopping SQL Server"
docker stop $DATABASE
docker rm $DATABASE