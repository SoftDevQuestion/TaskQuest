# استفاده از ایمیج SDK 8.0 برای بیلد
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# کپی کردن فایل پروژه و ریستور کردن پکیج‌ها
COPY ["TaskQuest/TaskQuest.csproj", "TaskQuest/"]
RUN dotnet restore "TaskQuest/TaskQuest.csproj"

# کپی کردن بقیه فایل‌ها و بیلد
COPY . .
WORKDIR "/src/TaskQuest"
RUN dotnet build "TaskQuest.csproj" -c Release -o /app/build

# انتشار پروژه
FROM build AS publish
RUN dotnet publish "TaskQuest.csproj" -c Release -o /app/publish /p:UseAppHost=false

# مرحله نهایی برای اجرا
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskQuest.dll"]