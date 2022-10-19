#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AuthenticationService/AuthenticationService.csproj", "AuthenticationService/"]
COPY ["AuthenticationService.Model/AuthenticationService.Model.csproj", "AuthenticationService.Model/"]
COPY ["AuthenticationService.Utilities/AuthenticationService.Utilities.csproj", "AuthenticationService.Utilities/"]
COPY ["AuthenticationService.ViewModel/AuthenticationService.ViewModel.csproj", "AuthenticationService.ViewModel/"]
COPY ["AuthenticationService.Service/AuthenticationService.Service.csproj", "AuthenticationService.Service/"]
RUN dotnet restore "AuthenticationService/AuthenticationService.csproj"
COPY . .
WORKDIR "/src/AuthenticationService"
RUN dotnet build "AuthenticationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthenticationService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthenticationService.dll"]