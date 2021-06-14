FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /app
COPY ./ ./
RUN dotnet restore \
    && cd /app/Server \
    && dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:3.1

ARG VERSION

# libc-dev is required by gRPC
RUN apt-get update && apt-get install -y \
    libc-dev \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY --from=build /app/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "Server.dll"]