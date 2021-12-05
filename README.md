# bonkers-bdsa-project
BDSA2021 Final Project

## How to setup SQL database and start up server (using powershell)

Navigate to `ProjectBank/Server` in a terminal.

Install Entity Framework tool for all users:

`dotnet tool install --global dotnet-ef`

Set password variable:

`$password = New-Guid`

Run Microsoft Sql Server in docker: 

`docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$password" -p 1433:1433 --name serene_kepler -d mcr.microsoft.com/mssql/server:2019-latest`

Set database name variable:

`$database = serene_kepler`

Set connnectionString variable: 

`"Server=localhost;Database=$database;User Id=sa;Password=$password;Trusted_Connection=False;Encrypt=True;TrustServerCertificate=true"`

Update the connectionString in Server.csproj:

`dotnet user-secrets set "ConnectionStrings:serene_kepler" "$connectionString"` 

Apply migration to database: 

`dotnet ef database update`

Run server:

`dotnet run`
