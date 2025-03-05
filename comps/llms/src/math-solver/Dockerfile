FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Copy everything
COPY comps /home/user/comps

WORKDIR /home/user/comps/llms/src/math-solver/MathSolver

# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0

ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false

WORKDIR /MathSolver
COPY --from=build /home/user/comps/llms/src/math-solver/MathSolver/out .
ENTRYPOINT ["dotnet", "MathSolver.dll"]