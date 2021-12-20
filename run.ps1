#!/usr/bin/env pwsh

$project = "./Server"
$entities = "./Server.Entities"
$database = "sql_server"
$sqlPass = New-Guid
$connectionString = "Server=localhost;Database=$database;User Id=sa;Password=$sqlPass;Trusted_Connection=False;Encrypt=True;TrustServerCertificate=true"

try {
    Write-Host "Configuring Connection String"
    dotnet user-secrets init --project $project
    dotnet user-secrets set "ConnectionStrings:$database" "$connectionString" --project $project > $null

    Write-Host "Starting SQL Server"
    docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$sqlPass" -p 1433:1433 --name $database -d mcr.microsoft.com/mssql/server:2019-latest
    Start-Sleep -Seconds 6
    dotnet ef database update --project $project

    Write-Host "Starting ProjectBank"
    dotnet run --project $project
} finally {
    Write-Host "Stopping SQL Server"
    docker stop $database
    docker rm $database
}
