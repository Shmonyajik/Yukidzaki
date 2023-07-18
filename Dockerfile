#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Babadzaki/Babadzaki.csproj", "Babadzaki/"]
COPY ["Babadzaki_DAL/Babadzaki_DAL.csproj", "Babadzaki_DAL/"]
COPY ["Babadzaki_Domain/Babadzaki_Domain.csproj", "Babadzaki_Domain/"]
COPY ["Babadzaki_Serivces/Babadzaki_Serivces.csproj", "Babadzaki_Serivces/"]
RUN dotnet restore "Babadzaki/Babadzaki.csproj"
COPY . .
WORKDIR "/src/Babadzaki"
RUN dotnet build "Babadzaki.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Babadzaki.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Babadzaki.dll"]