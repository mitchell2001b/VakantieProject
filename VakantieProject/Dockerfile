# Use the ASP.NET Core runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy the .csproj file and restore dependencies
COPY VakantieProject.csproj ./
RUN dotnet restore "./VakantieProject.csproj"

# Copy the rest of the application code
COPY . .
WORKDIR /src
RUN dotnet build "VakantieProject.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "VakantieProject.csproj" -c Release -o /app/publish

# Build the final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VakantieProject.dll"]
