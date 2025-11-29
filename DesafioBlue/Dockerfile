# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY ["DesafioBlue.csproj", "./"]
RUN dotnet restore "DesafioBlue.csproj"

# copy everything else and publish
COPY . ./
RUN dotnet publish "DesafioBlue.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS="http://+:80"

# Default Postgres connection info for the wait-for script. These are safe
# defaults for the compose setup where the DB service is named `db` and
# listens on 5432.
ENV POSTGRES_HOST=db
ENV POSTGRES_PORT=5432

COPY --from=build /app/publish ./

# copy wait-for script and make executable
COPY wait-for-postgres.sh ./
RUN chmod +x ./wait-for-postgres.sh

EXPOSE 80

# Pass explicit host/port to the script so it never sees empty args from
# external invocations. The script itself will also read POSTGRES_HOST/PORT
# if needed.
ENTRYPOINT ["./wait-for-postgres.sh", "db", "5432"]
