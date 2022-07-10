using Microsoft.AspNetCore.Mvc;
using NLog;
using NLog.Web;

// imposto NLog per leggere da appsettings.json
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("Sgart.it demo init");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();


    var app = builder.Build();

    app.MapGet("/", () => "Sgart.it ciao");

    app.MapGet("/sqrt/{num:int}", (int num) =>
    {
        double result = Math.Sqrt(num);

        // uso il logger
        logger.Info($"sqrt({num}) = {result}");

        return result;
    });

    app.MapPost("/mult", ([FromBody] MultInputDTO data) => data.V1 * data.V2);

    // return json object
    app.MapGet("/tan/{num:int}", (int num) => new
    {
        result = Math.Tan(num)
    });

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}

// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record
public record MultInputDTO(int V1, int V2);