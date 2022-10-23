#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Mensageria.PoC.Api.Publish/Mensageria.PoC.Api.Publish.csproj", "Mensageria.PoC.Api.Publish/"]
RUN dotnet restore "Mensageria.PoC.Api.Publish/Mensageria.PoC.Api.Publish.csproj"
COPY . .
WORKDIR "/src/Mensageria.PoC.Api.Publish"
RUN dotnet build "Mensageria.PoC.Api.Publish.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mensageria.PoC.Api.Publish.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mensageria.PoC.Api.Publish.dll"]