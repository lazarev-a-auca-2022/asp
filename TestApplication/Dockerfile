FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TestApplication.csproj", "."]
RUN dotnet restore "TestApplication.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "TestApplication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestApplication.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestApplication.dll"]