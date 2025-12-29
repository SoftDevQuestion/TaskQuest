FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY src/MyApp/*.csproj ./MyApp/
RUN dotnet restore ./MyApp/MyApp.csproj
COPY src/MyApp/. ./MyApp/
RUN dotnet publish ./MyApp/MyApp.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet","MyApp.dll"]
