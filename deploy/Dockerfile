FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Presentation.Api/Presentation.Api.csproj", "Presentation.Api/"]
COPY ["src/Infrastructure.CrossCutting/Infrastructure.CrossCutting.csproj", "Infrastructure.CrossCutting/"]
COPY ["src/Application.Services/Application.Services.csproj", "Application.Services/"]
COPY ["src/Data.Repositories/Data.Repositories.csproj", "Data.Repositories/"]
COPY ["src/Data.Models/Data.Models.csproj", "Data.Models/"]
COPY ["src/Domain.Models/Domain.Models.csproj", "Domain.Models/"]
RUN dotnet restore "Presentation.Api/Presentation.Api.csproj"

COPY ./src /src
WORKDIR "/src/Presentation.Api"
RUN dotnet build "Presentation.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Presentation.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Presentation.Api.dll"]
