# ---------- Runtime base ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# ---------- Build ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiamos primero los .csproj para aprovechar caché de Docker
COPY ["InvoicesSystem.API/InvoicesSystem.API.csproj", "InvoicesSystem.API/"]

# Restauramos dependencias
RUN dotnet restore "InvoicesSystem.API/InvoicesSystem.API.csproj"

# Copiamos el resto del código
COPY . .

# Compilamos el proyecto en modo Release
WORKDIR "/src/InvoicesSystem.API"
RUN dotnet build "InvoicesSystem.API.csproj" -c Release -o /app/build

# ---------- Publish ----------
FROM build AS publish
RUN dotnet publish "InvoicesSystem.API.csproj" -c Release -o /app/publish --no-restore /p:UseAppHost=false

# ---------- Final ----------
FROM base AS final
WORKDIR /app

# Instalamos herramientas de EF Core para migraciones
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Copiamos la aplicación publicada
COPY --from=publish /app/publish .

# Definimos el punto de entrada de la aplicación
ENTRYPOINT ["dotnet", "InvoicesSystem.API.dll"]