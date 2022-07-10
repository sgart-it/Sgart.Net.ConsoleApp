# demo .NET 6 minimal web API + NLog + Entyty Framework 6

## Nuget

### NLog

dotnet add package NLog.Web.AspNetCore --version 5.0.0

### EF 6

dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 6.0.6

dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 6.0.6
dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0.6

dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore --version 6.0.6

## DbUpdate

dotnet ef migrations add [migration name]

dotnet ef database update

oppure

dotnet ef migrations remove

dotnet ef database update


## Links

[.NET 6 minimal web API + NLog](https://www.sgart.it/IT/informatica/net-6-minimal-web-api/post)

[.NET 6 minimal web API + Entity Framework](https://www.sgart.it/IT/informatica/net-6-minimal-web-api-con-entity-framework/post)
