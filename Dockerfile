FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY TaskQuest/*.csproj ./TaskQuest/
RUN dotnet restore "TaskQuest/TaskQuest.csproj"

COPY . .
WORKDIR "/src/TaskQuest"
RUN dotnet build "TaskQuest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskQuest.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskQuest.dll"]