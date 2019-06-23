FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["SBTest.AuthenticationWebApi/SBTest.AuthenticationWebApi.csproj", "SBTest.AuthenticationWebApi/"]
RUN dotnet restore "SBTest.AuthenticationWebApi/SBTest.AuthenticationWebApi.csproj"
COPY . .
WORKDIR "/src/SBTest.AuthenticationWebApi"
RUN dotnet build "SBTest.AuthenticationWebApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "SBTest.AuthenticationWebApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SBTest.AuthenticationWebApi.dll"]