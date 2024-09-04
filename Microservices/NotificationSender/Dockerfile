FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5126
EXPOSE 7022

COPY CertForDockerfile/genericnotification.crt /usr/share/ca-certificates/
COPY CertForDockerfile/mail.test.ru.crt /usr/share/ca-certificates/
RUN echo mail.test.ru.crt >> /etc/ca-certificates.conf
RUN echo genericnotification.crt >> /etc/ca-certificates.conf
RUN update-ca-certificates

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["NotificationSender.API/NotificationSender.API.csproj", "NotificationSender.API/"]
COPY ["NotificationSender.Application/NotificationSender.Application.csproj", "NotificationSender.Application/"]
COPY ["NotificationSender.Consumer/NotificationSender.Consumer.csproj", "NotificationSender.Consumer/"]
COPY ["NotificationSender.DAL/NotificationSender.DAL.csproj", "NotificationSender.DAL/"]
COPY ["NotificationSender.Domain/NotificationSender.Domain.csproj", "NotificationSender.Domain/"]
RUN dotnet restore "NotificationSender.API/NotificationSender.API.csproj"
COPY . .
WORKDIR "/src/NotificationSender.API"
RUN dotnet build "NotificationSender.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NotificationSender.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificationSender.API.dll"]
