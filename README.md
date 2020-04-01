# A Map to Success

## Important Notes!

1. If you run the Docker version of this code, you will not be able to run demo 8, which uses a charting library to display charts.
2. The charting library I use depends on the version of code.  In `MapToSuccess` Framework, I used the `FSharp.Charting` NuGet package.  This package does not support .NET Core, so I changed to `XPlot.Plotly` for `MapToSuccess.Core`.
3. The last demo I show is a fractal tree.  This demo, unfortunately, only works with .NET Framework; it won't port to Core.  If you want to see it, run the Framework version of this project from a Windows device.

## Running the Code

There are **three** ways that you can get this code working!  We will take each in turn.

### Run Docker Images

The easiest method to kick off these demos is to grab them from [Docker Hub](https://hub.docker.com/repository/docker/feaselkl/presentations).  Run the following commands to pull the two Docker images and then run them in the correct order.

```
docker pull docker.io/feaselkl/presentations:map-to-success-db
docker pull docker.io/feaselkl/presentations:map-to-success
```

First, we will kick off the Map to Success database:

`docker run --name fsharpdb -p 51433:1433 docker.io/feaselkl/presentations:map-to-success-db`

Then we will kick off the Map to Success .NET app.  There is a script to build the .NET Core project `MapToSuccess.Core` and run the console application.

```
docker run -it docker.io/feaselkl/presentations:map-to-success /bin/bash
cd /root/
./run_fsharp_demo
```

### Build Docker Images

If you want to build the Docker images for map-to-success and map-to-success-db, you can do that as well.

#### Build the Database Image

The first image we will want to build is the database image.  Run the following in PowerShell:

```
docker run -d -e MSSQL_PID=Developer -e ACCEPT_EULA=Y -e SA_PASSWORD=SomeBadP@ssword3 --name fsharpdb -p 51433:1433 mcr.microsoft.com/mssql/server:2019-latest
docker exec -it fsharpdb mkdir /var/opt/mssql/backup/
docker cp SQL\MapToSuccess.bak fsharpdb:/var/opt/mssql/backup/MapToSuccess.bak
```

Then, after waiting a few seconds, run the following to restore the `MapToSuccess` database.

```
docker exec -it fsharpdb /opt/mssql-tools/bin/sqlcmd -S localhost `
	-U SA -P SomeBadP@ssword3 `
	-Q "RESTORE DATABASE MapToSuccess FROM DISK = '/var/opt/mssql/backup/MapToSuccess.bak' WITH MOVE 'MapToSuccess' TO '/var/opt/mssql/data/MapToSuccess.mdf', MOVE 'MapToSuccess_Log' TO '/var/opt/mssql/data/MapToSuccess.ldf'"
```

Now you have a database container.  If you want to save this to run again later, you can run the following command:

`docker commit -m="Create the Map to Success database." fsharpdb map-to-success-db`

#### Build the .NET Image

To run the .NET code, you would run the following inside the MapToSuccess folder:

`docker build -t map-to-success .`

Note that this will take a while:  it installs mono-devel, which can take several minutes.  Once it does complete, start a container and go:

```
docker run -it map-to-success /bin/bash
cd /root/
./run_fsharp_demo
```


### Compile and Go

The last way to do this is the old-fashioned method:  clone this GitHub repository, open up Visual Studio, and build the MapToSuccess solution.

If you do this, you *must* have the MapToSuccess database set up in SQL Server.  Then, open up the `DatabaseAccess.fs` file in the MapToSuccess and MapToSuccess.Core projects and change the connection string to point to your SQL Server instance.  Note that you do not need to use SQL authentication; you can change the connection string to Windows authentication by making it look something like:

`@"Data Source=LOCALHOST,51433;Initial Catalog=MapToSuccess;Integrated Security=True"`
