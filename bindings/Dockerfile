FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy AS base
WORKDIR /app
EXPOSE 7001

ENV ASPNETCORE_URLS=http://+:7001

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
WORKDIR /src
COPY ["bindings.csproj", "./"]
RUN dotnet restore "bindings.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "bindings.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "bindings.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "bindings.dll"]