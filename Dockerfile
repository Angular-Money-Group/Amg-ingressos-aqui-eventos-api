FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /app
COPY . .
RUN nuget locals all -clear
RUN dotnet restore
RUN dotnet publish -o /app/published-app

FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime
WORKDIR /app
COPY --from=build /app/published-app /app

COPY ./Amg-ingressos-aqui-eventos-api/images /app/images
COPY ./Amg-ingressos-aqui-eventos-api/descriptions /app/descriptions

ENTRYPOINT [ "dotnet", "/app/Amg-ingressos-aqui-eventos-api.dll" ]
