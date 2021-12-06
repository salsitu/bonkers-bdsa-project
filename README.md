# bonkers-bdsa-project
BDSA2021 Final Project

## How to setup SQL database connection (using powershell)

Navigate to `ProjectBank/Server` in a terminal.

Install Entity Framework tool for all users:

`dotnet tool install --global dotnet-ef`

Set password variable:

`$password = New-Guid`

Set database name variable:

`$database = sql_server`

Run Microsoft Sql Server in docker: 

`docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$password" -p 1433:1433 --name $database -d mcr.microsoft.com/mssql/server:2019-latest`

Set connnectionString variable: 

`$connectionString = "Server=localhost;Database=$database;User Id=sa;Password=$password;Trusted_Connection=False;Encrypt=True;TrustServerCertificate=true"`

Update the connectionString in Server.csproj:

`dotnet user-secrets set "ConnectionStrings:$database" "$connectionString"`

Apply migration to database: 

`dotnet ef database update`

## Start up server 
Navigate to `ProjectBank/Server` in a terminal.

Run the following command:
`dotnet run`

## Start client session
Open browser (Firefox/Chrome/Safari/Edge)

Input the following in the address bar: `localhost:5001`

