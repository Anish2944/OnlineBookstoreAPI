# Stage 1: Build
From mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy csproj & restore
COPY *.sln .
COPY OnlineBookstoreAPI/*.csproj ./OnlineBookstoreAPI/
RUN dotnet restore

# copy everything else and build
COPY . .
WORKDIR /src/OnlineBookstoreAPI
RUN dotnet publish -c Release -o /app/publish

#Satge 2: RUN
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "OnlineBookstoreAPI.dll"]
