#!/usr/bin/zsh

dotnet ef migrations remove
dotnet run delete
rm -rf Migrations

dotnet ef migrations add InitialCreate

dotnet ef migrations add UpdateDatabase

dotnet run seed
