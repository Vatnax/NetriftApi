FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy all the files
COPY ./ ./
WORKDIR ./source

# Clear Nuget cache so it doesn't affect future builds
RUN dotnet nuget locals all --clear
# Restore all of the dependencies
RUN dotnet restore

# Change working directory to the entrypoint project's folder
WORKDIR /src/source/Api
# Publishing the application
RUN dotnet publish -c release -o /app --no-restore

# Finishing the image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Api.dll"]

# Exposing ports
EXPOSE 5000
EXPOSE 5001