# Sgart.Net5.ConsoleApp.Data

Class library con con la configurazione di EF Core 5 e le migrations

# Migration

per creare una nuova migration

=> cd Sgart.Net5.ConsoleApp.Data
=> dotnet ef migrations add NomeMigrazione

# Possibili Errori

The Entity Framework tools version '5.0.8' is older than that of the runtime '5.0.10'. Update the tools for the latest f eatures and bug fixes. 
=> dotnet tool update --global dotnet-ef

errore durante la creazione del database
=> dotnet ef database update
