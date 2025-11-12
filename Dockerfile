# Use the official .NET 8.0 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["HoshiVibe.csproj", "./"]
RUN dotnet restore "HoshiVibe.csproj"

# Copy the rest of the application code
COPY . .

# Build the application
RUN dotnet build "HoshiVibe.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "HoshiVibe.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the official .NET 8.0 runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Expose the port the app runs on
EXPOSE 8080
EXPOSE 8081

# Copy the published app from the publish stage
COPY --from=publish /app/publish .

# Set the entry point for the container
ENTRYPOINT ["dotnet", "HoshiVibe.dll"]
