FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["PartagesWeb.API.csproj", ""]
RUN dotnet restore "/PartagesWeb.API.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "PartagesWeb.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "PartagesWeb.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PartagesWeb.API.dll"]