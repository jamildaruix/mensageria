#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Mensageria.PoC.Worker.Consumer/Mensageria.PoC.Worker.Consumer.csproj", "Mensageria.PoC.Worker.Consumer/"]
RUN dotnet restore "Mensageria.PoC.Worker.Consumer/Mensageria.PoC.Worker.Consumer.csproj"
COPY . .
WORKDIR "/src/Mensageria.PoC.Worker.Consumer"
RUN dotnet build "Mensageria.PoC.Worker.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mensageria.PoC.Worker.Consumer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mensageria.PoC.Worker.Consumer.dll"]