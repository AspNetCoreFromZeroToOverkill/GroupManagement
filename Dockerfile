FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS builder
WORKDIR /app

# Copy solution and restore as distinct layers to cache dependencies
COPY ./src/CodingMilitia.PlayBall.GroupManagement.Web/*.csproj ./src/CodingMilitia.PlayBall.GroupManagement.Web/
COPY ./src/CodingMilitia.PlayBall.GroupManagement.Domain/*.csproj ./src/CodingMilitia.PlayBall.GroupManagement.Domain/
COPY ./src/CodingMilitia.PlayBall.GroupManagement.Infrastructure/*.csproj ./src/CodingMilitia.PlayBall.GroupManagement.Infrastructure/
COPY ./src/CodingMilitia.PlayBall.Shared.StartupTasks/*.csproj ./src/CodingMilitia.PlayBall.Shared.StartupTasks/
COPY ./tests/CodingMilitia.PlayBall.GroupManagement.Domain.Tests/*.csproj ./tests/CodingMilitia.PlayBall.GroupManagement.Domain.Tests/
COPY ./tests/CodingMilitia.PlayBall.GroupManagement.Infrastructure.Tests/*.csproj ./tests/CodingMilitia.PlayBall.GroupManagement.Infrastructure.Tests/
COPY *.sln ./
RUN dotnet restore

# Publish the application
COPY . ./
WORKDIR /app/src/CodingMilitia.PlayBall.GroupManagement.Web
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS runtime
WORKDIR /app
COPY --from=builder /app/src/CodingMilitia.PlayBall.GroupManagement.Web/out .
ENTRYPOINT ["dotnet", "CodingMilitia.PlayBall.GroupManagement.Web.dll"]

# Sample build command
# docker build -t codingmilitia/groupmanagement .