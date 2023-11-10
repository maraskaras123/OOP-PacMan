#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0
COPY ./bin/Release/net7.0/publish /publish
WORKDIR /publish
EXPOSE 7276
ENTRYPOINT ["dotnet", "PacMan.Server.dll"]