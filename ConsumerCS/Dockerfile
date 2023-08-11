FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base

FROM base as build
WORKDIR /build
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM base as final
WORKDIR /App
COPY --from=build /build/out .
ENTRYPOINT ["dotnet", "ConsumerCS.dll"]