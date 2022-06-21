#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
VOLUME /app/MLModels
ENV MLModelPath=MLModels/TrackClassifierModel.zip

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["TrackClassifier.App/TrackClassifier.App.csproj", "TrackClassifier.App/"]
RUN dotnet restore "TrackClassifier.App/TrackClassifier.App.csproj"
COPY . .
WORKDIR "/src/TrackClassifier.App"
RUN dotnet build "TrackClassifier.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TrackClassifier.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /src/MLModels /app/MLModels
ENTRYPOINT ["dotnet", "TrackClassifier.App.dll"]