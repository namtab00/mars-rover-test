# Mars Rover test

Requirements:

* .NET 8
* Visual Studio 2022

## Instructions

Clone repo, then explore the API with one of the following:

### Start from VS 2022
* open `.\MarsRover.sln`
* start (with or without debugging) the launch profile named `https`
* Swagger UI should open in default browser window at URL [https://localhost:7011](https://localhost:7011)
* use Swagger UI or `.\MarsRover.API\MarsRover.API.http` from VS to invoke endpoints
* see test projects also

### Start from cmd line
* in Terminal, move to repo root
* start API with command `dotnet run --project .\MarsRover.API\MarsRover.API.csproj`
* in a browser, navigate to https://localhost:7011/swagger/index.html
