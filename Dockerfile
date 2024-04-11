FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
COPY HodladiWallet.csproj .
RUN dotnet restore HodladiWallet.csproj
COPY . .
RUN dotnet build HodladiWallet.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish HodladiWallet.csproj -c Release -o /app/

# final stage/image
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "HodladiWallet.dll"]