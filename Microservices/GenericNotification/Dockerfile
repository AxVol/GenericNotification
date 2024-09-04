FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5056
EXPOSE 7125

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["GenericNotification.API/GenericNotification.API.csproj", "GenericNotification.API/"]
COPY ["GenericNotification.Application/GenericNotification.Application.csproj", "GenericNotification.Application/"]
COPY ["GenericNotification.DAL/GenericNotification.DAL.csproj", "GenericNotification.DAL/"]
COPY ["GenericNotification.Domain/GenericNotification.Domain.csproj", "GenericNotification.Domain/"]
COPY ["GenericNotification.Producer/GenericNotification.Producer.csproj", "GenericNotification.Producer/"]
COPY ["GenericNotification.Tests/GenericNotification.Tests.csproj", "GenericNotification.Tests/"]
RUN dotnet restore "GenericNotification.API/GenericNotification.API.csproj"
COPY . .
WORKDIR "/src/GenericNotification.API"
RUN dotnet build "GenericNotification.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GenericNotification.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GenericNotification.API.dll"]
