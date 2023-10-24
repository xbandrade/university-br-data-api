FROM mcr.microsoft.com/dotnet/sdk:7.0

COPY . /app

WORKDIR /app

RUN dotnet restore

RUN dotnet build

EXPOSE 80

CMD ["dotnet", "run"]
