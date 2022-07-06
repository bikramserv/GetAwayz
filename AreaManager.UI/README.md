# Developing locally with dependencies in docker-compose

Run Postgres

```
$ cd src
$ docker-compose up -d
```

You can now run/debug LicenseManager.UI project in Visual Studio / VS Code / Rider.

Note: Use HTTPS://localhost:5001 because Keycloak is also in HTTPS and it will redirect back your app after the log in.

# Creating EF Migrations

### Install the EF Core console tools

```
dotnet tool install --global dotnet-ef
```

### Execute the creation command

```
cd src
dotnet ef migrations add InitialCreate --project AreaManager.UI
```
