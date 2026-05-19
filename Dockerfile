FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["JTExpress.Api/JTExpress.Api/JTExpress.Api.csproj", "JTExpress.Api/JTExpress.Api/"]
RUN dotnet restore "JTExpress.Api/JTExpress.Api/JTExpress.Api.csproj"
COPY JTExpress.Api/JTExpress.Api/ JTExpress.Api/JTExpress.Api/
WORKDIR "/src/JTExpress.Api/JTExpress.Api"
RUN dotnet publish "JTExpress.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "JTExpress.Api.dll"]
