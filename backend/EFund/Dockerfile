#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 4040
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EFund.WebAPI/EFund.WebAPI.csproj", "EFund.WebAPI/"]
COPY ["EFund.BLL/EFund.BLL.csproj", "EFund.BLL/"]
COPY ["EFund.Client.Monobank/EFund.Client.Monobank.csproj", "EFund.Client.Monobank/"]
COPY ["EFund.Common/EFund.Common.csproj", "EFund.Common/"]
COPY ["EFund.DAL/EFund.DAL.csproj", "EFund.DAL/"]
COPY ["EFund.Email/EFund.Email.csproj", "EFund.Email/"]
COPY ["EFund.Hangfire/EFund.Hangfire.csproj", "EFund.Hangfire/"]
COPY ["EFund.Mapping/EFund.Mapping.csproj", "EFund.Mapping/"]
COPY ["EFund.Seeding/EFund.Seeding.csproj", "EFund.Seeding/"]
COPY ["EFund.Validation/EFund.Validation.csproj", "EFund.Validation/"]
RUN dotnet restore "./EFund.WebAPI/./EFund.WebAPI.csproj"
COPY . .
WORKDIR "/src/EFund.WebAPI"
RUN dotnet build "EFund.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

WORKDIR "/src/EFund.WebAPI"
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EFund.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
COPY EFund.WebAPI/appsettings.json /app/publish/appsettings.json
COPY EFund.Seeding/Data /app/publish/SeedingData
COPY EFund.Email/Templates /app/publish/EmailTemplates
RUN mkdir -p /app/AppData/
RUN mkdir -p /app/AppData/Logs/
RUN mkdir -p /app/AppData/Uploads/
RUN mkdir -p /app/AppData/Users/
RUN mkdir -p /app/AppData/Default/
RUN mkdir -p /app/AppData/Fundraisings/
RUN mkdir -p /app/AppData/Reports/
RUN mkdir -p /app/AppData/Attachments/

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EFund.WebAPI.dll"]