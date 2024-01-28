#!/usr/bin/zsh

dotnet ef migrations add InitialCreate

dotnet ef migrations add UpdateDatabase

dotnet run seed
