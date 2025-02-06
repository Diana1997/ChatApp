# Use the .NET Core SDK image for building the project
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy the solution file
COPY ChatApp.sln ./

# Copy all the projects and restore the dependencies
COPY src/Application/*.csproj ./src/Application/
COPY src/Domain/*.csproj ./src/Domain/
COPY src/Infrastructure/*.csproj ./src/Infrastructure/
COPY src/WebApi/*.csproj ./src/WebApi/
RUN dotnet restore

# Copy all source files
COPY . ./

# Build and publish the WebApi project (with dependencies)
RUN dotnet publish src/WebApi/WebApi.csproj -c Release -o out

# Use the .NET Core runtime image to run the built application
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .

# Expose the port for the Web API
EXPOSE 80

# Run the WebApi project
ENTRYPOINT ["dotnet", "WebApi.dll"]
