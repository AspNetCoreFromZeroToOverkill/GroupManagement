FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS builder
WORKDIR /app

# Copy solution and restore as distinct layers to cache dependencies
COPY ./src/CodingMilitia.PlayBall.GroupManagement.Web/*.csproj ./src/CodingMilitia.PlayBall.GroupManagement.Web/
COPY ./src/CodingMilitia.PlayBall.GroupManagement.Business/*.csproj ./src/CodingMilitia.PlayBall.GroupManagement.Business/
COPY ./src/CodingMilitia.PlayBall.GroupManagement.Business.Impl/*.csproj ./src/CodingMilitia.PlayBall.GroupManagement.Business.Impl/
COPY ./src/CodingMilitia.PlayBall.GroupManagement.Data/*.csproj ./src/CodingMilitia.PlayBall.GroupManagement.Data/
COPY ./src/CodingMilitia.PlayBall.Shared.StartupTasks/*.csproj ./src/CodingMilitia.PlayBall.Shared.StartupTasks/
COPY *.sln ./
RUN dotnet restore

# Publish the application
COPY . ./
WORKDIR /app/src/CodingMilitia.PlayBall.GroupManagement.Web
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-alpine AS runtime
WORKDIR /app
COPY --from=builder /app/src/CodingMilitia.PlayBall.GroupManagement.Web/out .
ENTRYPOINT ["dotnet", "CodingMilitia.PlayBall.GroupManagement.Web.dll"]

# Sample build command
# docker build -t codingmilitia/groupmanagement .