FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG PAT

WORKDIR /source

RUN dotnet nuget add source "https://pkgs.dev.azure.com/keesingtech/KVS/_packaging/kvsalpha/nuget/v3/index.json" --name "kvsalpha" --username "useless" --password "$PAT" --store-password-in-clear-text \
    && dotnet nuget add source "https://pkgs.dev.azure.com/keesingtech/KVS/_packaging/kvsprod/nuget/v3/index.json" --name "kvsprod" --username "useless" --password "$PAT" --store-password-in-clear-text

# # copy csproj and restore as distinct layers
COPY *.sln .
COPY AreaManager.Core/*.csproj AreaManager.Core/
COPY AreaManager.Core.Test/*.csproj AreaManager.Core.Test/
COPY AreaManager.Data/*.csproj AreaManager.Data/
COPY AreaManager.UI/*.csproj AreaManager.UI/
RUN dotnet restore

# # copy everything else and build and test
COPY AreaManager.Core/. AreaManager.Core/ 
COPY AreaManager.Core.Test/. AreaManager.Core.Test/ 
COPY AreaManager.Data/. AreaManager.Data/ 
COPY AreaManager.UI/. AreaManager.UI/ 

RUN dotnet test
RUN dotnet publish AreaManager.UI/AreaManager.UI.csproj -o /app -c Release --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
EXPOSE 80
EXPOSE 443
WORKDIR /app

COPY --from=build /app .

RUN groupadd -r appuser \ 
    && useradd -r -g appuser appuser \
    && chown -R appuser:appuser /app 

USER appuser

ENV ASPNETCORE_URLS=http://*:5000

ENTRYPOINT ["dotnet", "AreaManager.UI.dll"]  
