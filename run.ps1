$project = "./Server"
$entities = "./Server.Entities"
$database = "sql_server"
$sqlPass = New-Guid
$connectionString = "Server=localhost;Database=$database;User Id=sa;Password=$sqlPass;Trusted_Connection=False;Encrypt=False"

Write-Host "Configuring Connection String"
dotnet user-secrets init --project $project
dotnet user-secrets set --project $project "ConnectionStrings:$database" "$connectionString" > $null

Write-Host "Starting SQL Server"
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$sqlPass" -p 1433:1433 --name $database -d mcr.microsoft.com/mssql/server:2019-latest
dotnet ef migrations add InitialMigration -p $entities -s $project

Write-Host "Starting ProjectBank"
dotnet run --project $project

Write-Host "Stopping SQL Server"
docker stop $DATABASE
docker rm $DATABASE
Remove-Item "$entities/Migrations" -Recurse