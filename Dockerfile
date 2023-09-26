#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Yukidzaki/Yukidzaki.csproj", "Yukidzaki/"]
COPY ["Yukidzaki_DAL/Yukidzaki_DAL.csproj", "Yukidzaki_DAL/"]
COPY ["Yukidzaki_Domain/Yukidzaki_Domain.csproj", "Yukidzaki_Domain/"]
COPY ["Yukidzaki_Services/Yukidzaki_Services.csproj", "Yukidzaki_Services/"]
RUN dotnet restore "Yukidzaki/Yukidzaki.csproj"


COPY . .
WORKDIR "/src/Yukidzaki"
RUN dotnet build "Yukidzaki.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Yukidzaki.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Yukidzaki.dll"]