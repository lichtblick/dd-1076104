FROM mcr.microsoft.com/dotnet/runtime:6.0-alpine AS base
WORKDIR /app
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY OtlpClient.csproj OtlpClient.csproj
RUN dotnet restore
COPY ./ ./
RUN dotnet build -c Release --no-restore

FROM build AS publish
RUN dotnet publish --no-restore --no-build -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OtlpClient.dll"]