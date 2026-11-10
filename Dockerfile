# ---- Compilación ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY HotelBrisaDorada/HotelBrisaDorada.csproj HotelBrisaDorada/
RUN dotnet restore HotelBrisaDorada/HotelBrisaDorada.csproj

COPY HotelBrisaDorada/ HotelBrisaDorada/
RUN dotnet publish HotelBrisaDorada/HotelBrisaDorada.csproj -c Release -o /app/publish

# ---- Ejecución ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "HotelBrisaDorada.dll"]
