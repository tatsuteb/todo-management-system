FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:80
COPY ["WebClient/WebClient.csproj", "WebClient/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["UseCase/UseCase.csproj", "UseCase/"]
COPY ["Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "WebClient/WebClient.csproj"
COPY . .
WORKDIR "/src/WebClient"
RUN dotnet publish "WebClient.csproj" -c Release -o /app/publish
WORKDIR /app/publish
ENTRYPOINT ["dotnet", "WebClient.dll"]