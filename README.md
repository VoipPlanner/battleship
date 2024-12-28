# README #

### What is this repository for? ###

This is a .net core 3 webapi to track the status of a battleship game for one player 

### Project structure ###

1- Web Api

2- Unit tests

3- Integration tests


### How to build and run ###

You will have to install .net core 3 runtime which can be downloaded from 

    https://dotnet.microsoft.com/download/dotnet-core/3.0 to run the project.


## To build the project you need to to the following: ##

## mac os ##

run ./run.sh from the root of the repository for interactive menu. 

## Windows or any command line/terminal ##

1- clone the repo to your local environment and cd to repository directory

2- dotnet build BattleShip.sln

3- dotnet run --project Battleship.Api/Battleship.Api.csproj

the api will run in development environment on https://localhost:5071


Alternatively, open Battleship.Api.sln using visual studio 2019 or Intelij Rider then Build and run it.



### To run unit and Integration tests: ###

dotnet test ./Battleship.Unit.Tests

dotnet test ./Battleship.Integration.Tests



### Api Endpoints: ###

## There is a postman collection in the Doc folder, you can use to work with the api. ##



In Summary:

### Get game/board status ###

 `https://localhost:5071/game`

Get Board status:

 `https://localhost:5071/board`

### Create the board ###

Post  `https://localhost:5071/create`

Put   `https://localhost:5071/game/status/setup`


### Adding battleship ###

Post `https://localhost:5071/board/ship`
 
 
### Attack player ###

Post `https://localhost:5071/game/attack`


### Change game status ###

Put   `https://localhost:5071/game/status/Playing`

Put   `https://localhost:5071/game/status/Setup`


### See it in action: following link is deployed to AWS Ecs ###

http://battleshipapi-31020185.ap-southeast-2.elb.amazonaws.com/game

http://battleshipapi-31020185.ap-southeast-2.elb.amazonaws.com/swagger
