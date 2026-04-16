FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/JoyitasChirinos.API/JoyitasChirinos.API.csproj", "src/JoyitasChirinos.API/"]
COPY ["src/JoyitasChirinos.Application/JoyitasChirinos.Application.csproj", "src/JoyitasChirinos.Application/"]
COPY ["src/JoyitasChirinos.Infrastructure/JoyitasChirinos.Infrastructure.csproj", "src/JoyitasChirinos.Infrastructure/"]
COPY ["src/JoyitasChirinos.Domain/JoyitasChirinos.Domain.csproj", "src/JoyitasChirinos.Domain/"]
RUN dotnet restore "src/JoyitasChirinos.API/JoyitasChirinos.API.csproj"
COPY . .
WORKDIR "/src/src/JoyitasChirinos.API"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JoyitasChirinos.API.dll"]
