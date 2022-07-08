using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using Sgart.Net.ConsoleApp.Data;
using Sgart.Net.WebReactApp2.Services;
using Sgart.Net.WebReactApp2.Models;
using Microsoft.AspNetCore.Mvc;
using Sgart.Net.ConsoleApp.BO.InputDTO;
using Sgart.Net.WebReactApp2.Routes;

// imposto NLog per leggere da appsettimgs.json
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("Sgart.it demo init");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // mappo appsettings.json
    builder.Services.AddSingleton(builder.Configuration.GetSection("Settings").Get<AppSettings>());

    // registro EF DB context, add services to the container.
    builder.Services.AddDbContext<SgartDbContext>(option =>
    {
        option.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection"),
                // imposto in quale assembly si trovano le migration
                b => b.MigrationsAssembly(typeof(SgartDbContext).Assembly.GetName().Name)
            );
    });

    // registro la classe che verrà usata tramite dependency injection
    builder.Services.AddTransient<SgartDIExampleService>();
    builder.Services.AddTransient<SimpleExcelService>();
    builder.Services.AddTransient<ExportService>();

    // Add services to the container.
    //builder.Services.AddControllersWithViews();
    //builder.Services.AddControllers();

    // Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    //app.UseRouting();

    //app.UseAuthorization();

    //app.MapControllerRoute(
    //    name: "default",
    //    pattern: "{controller}/{action=Index}/{id?}");


    // API Todo
    // le API/route le posso dichiarare in una classe a parte, per pulizia del codice, nel caso fossero molte
    
    TodoRoute.Add(app, "/api/todo");
    
    // oppure direttamente in linea

    //app.MapGet("/api/todo", async (SgartDIExampleService service) => await service.GetAllAsync());
    //app.MapGet("/api/todo/{id:int}", async (int id, SgartDIExampleService service) => await service.GetAsync(id));
    //app.MapPost("/api/todo", async ([FromBody] TodoAddDTO data, SgartDIExampleService service) =>
    //{
    //    var todo = await service.AddAsync(data);
    //    if (todo != null)
    //    {
    //        return Results.NoContent();
    //        //return Results.Created($"/api/todo/{todo.TodoId}", todo);
    //    }

    //    return Results.BadRequest();
    //});
    //app.MapPut("/api/todo", async ([FromBody] TodoEditDTO data, SgartDIExampleService service) =>
    //{
    //    if (await service.EditAsync(data))
    //        return Results.NoContent();
    //    return Results.BadRequest();
    //});
    //app.MapDelete("/api/todo/{id}", async (int id, SgartDIExampleService service) =>
    //{
    //    if (await service.DeleteAsync(id))
    //        return Results.NoContent();
    //    return Results.BadRequest();
    //});

    // API weatherforecast
    app.MapGet("/api/weatherforecast", () =>
    {
        var rng = new Random();

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = rng.Next(-20, 55),
            Summary = WeatherForecastMock.SummariesData[rng.Next(WeatherForecastMock.SummariesData.Length)]
        })
        .ToArray();
    });
    //app.MapGet("/api/xxx", () => new { Message = "Ciao" });
    //app.MapGet("/api/xxx", () => Results.OK(new { Message = "Ciao" }));


    // route di fallback
    app.MapFallbackToFile("index.html"); ;

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