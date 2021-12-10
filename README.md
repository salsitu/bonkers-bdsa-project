# ProjectBank

## How to setup ProjectBank on Windows

First, make sure that Docker is up and running, typically by starting Docker Desktop. Then, in a new PowerShell terminal, install the Entity Framework Core tool for .NET:

```ps1
> dotnet tool install --global dotnet-ef
```

After that's done, run the startup script to start the ProjectBank server:

```ps1
> ./run.ps1
```

Note: You can also use this method on Linux and macOS if you have PowerShell installed.

## How to setup ProjectBank on Linux and macOS

First, make sure that Docker is up and running. On macOS you can do this by starting Docker Desktop. On systemd-based Linux distributions, run the following command in a terminal to start the Docker service:

```bash
$ sudo systemctl start docker
```

Then, install the Entity Framework Core tool for .NET:

```bash
$ dotnet tool install --global dotnet-ef
```

After that's done, make the startup script executable and run it to start the ProjectBank server:

```bash
$ chmod +x run.sh
$ ./run.sh
```

Note: This also works in Windows Subsystem for Linux (WSL).

## Start client session
Open you preferred web browser and navigate to the following URL:
```text
https://localhost:5001
```
