#!/bin/sh

PROJECT="Server"
ENTITIES="Server.Entities"
DATABASE="sql_server"
SQL_PASS=$(uuidgen)
CONNECTION_STRING="Server=localhost;Database=$DATABASE;User Id=sa;Password=$SQL_PASS;Trusted_Connection=False;Encrypt=False"

echo "Configuring Connection String"
dotnet user-secrets init --project $PROJECT
dotnet user-secrets set --project $PROJECT "ConnectionStrings:$DATABASE" "$CONNECTION_STRING" > /dev/null

echo "Starting SQL Server"
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$SQL_PASS" -p 1433:1433 --name $DATABASE -d mcr.microsoft.com/mssql/server:2019-latest
dotnet ef migrations add InitialMigration -p $ENTITIES -s $PROJECT

echo "Starting ProjectBank (press Ctrl+C to stop)"
dotnet run --project $PROJECT

echo "Stopping SQL Server"
docker stop $DATABASE
docker rm $DATABASE
rm -rf $ENTITIES/Migrations