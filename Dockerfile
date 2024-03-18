FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /app
COPY . .

RUN dotnet restore
RUN dotnet publish -o /app/published-app

FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime
WORKDIR /app
COPY --from=build /app/published-app /app
COPY ./Amg-ingressos-aqui-eventos-api/images /app/images
##config 
ENV TZ=America/Sao_Paulo
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
ENTRYPOINT [ "dotnet", "/app/Amg-ingressos-aqui-eventos-api.dll" ]
