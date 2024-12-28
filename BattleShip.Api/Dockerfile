# Use the official .NET 6.0 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy the .csproj and restore any dependencies (via dotnet restore)
COPY BattleShip.Api.csproj ./ 
RUN dotnet restore "BattleShip.Api.csproj"

# Copy the rest of the code
COPY . ./

# Build the app in release mode and output to /app/build
WORKDIR "/src"
RUN dotnet build "BattleShip.Api.csproj" -c Release -o /app/build

# Publish the app to /app/publish
RUN dotnet publish "BattleShip.Api.csproj" -c Release -o /app/publish

# Use the official .NET 6.0 runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy the published app from the build stage
COPY --from=build /app/publish .

# Set the entry point to run the app
ENTRYPOINT ["dotnet", "BattleShip.Api.dll"]
