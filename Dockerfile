FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY HackerRank1/HackerRank1.csproj HackerRank1/
RUN dotnet restore HackerRank1/HackerRank1.csproj

COPY HackerRank1/ HackerRank1/
RUN dotnet publish HackerRank1/HackerRank1.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

ENTRYPOINT ["dotnet", "HackerRank1.dll"]
