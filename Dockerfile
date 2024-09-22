FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Exposing ports
EXPOSE ${HTTP_PORT}

WORKDIR /src

# Copy all the files
COPY ./ ./
WORKDIR ./source

# Clear Nuget cache so it doesn't affect future builds
RUN dotnet nuget locals all --clear
# Restore all of the dependencies
RUN dotnet restore

# Publishing the application
WORKDIR /src/source/Api
RUN dotnet publish -c release -o /app --no-restore

# Finishing the image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Api.dll"]