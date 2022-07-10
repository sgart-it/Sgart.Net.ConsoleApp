using Microsoft.AspNetCore.Mvc;
using NLog;
using NLog.Web;
using Microsoft.EntityFrameworkCore;

// imposto NLog per leggere da appsettings.json
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("Sgart.it demo init");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // registro EF DB context

    // usando InMemoryDatabase, add services to the container.
    // Note: InMemoryDatabase da usare solo per Demo, non usare in produzione
    //builder.Services.AddDbContext<TodoDbContext>(opt => opt.UseInMemoryDatabase("SgartTodoList"));

    // oppure usando un DB reale
    builder.Services.AddDbContext<TodoDbContext>(option =>
    {
        option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

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

    // API Todo basate su TodoDbContext
    app.MapGet("/todo", async (TodoDbContext db) => await db.Todos.AsNoTracking().ToListAsync());
    app.MapGet("/todo/completed", async (TodoDbContext db) => await db.Todos.AsNoTracking().Where(x => x.Completed == true).ToListAsync());
    // /todo/text/contains?text=ciao
    app.MapGet("/todo/contains", async (string text, TodoDbContext db) =>
        await db.Todos.AsNoTracking()
            .Where(x => x.Text != null && x.Text.Contains(text, StringComparison.InvariantCultureIgnoreCase))
            .ToListAsync());
    app.MapGet("/todo/{todoId}", async (int todoId, TodoDbContext db) =>
        await db.Todos.FindAsync(todoId) is Todo todo
            ? Results.Ok(todo)
            : Results.NotFound());
    app.MapPost("/todo", async (TodoInputDTO inputTodo, TodoDbContext db) =>
    {
        // validare sempre i parametri di ingresso
        if (string.IsNullOrWhiteSpace(inputTodo.Text))
            return Results.BadRequest("Invalid Text");

        var todo = new Todo
        {
            Text = inputTodo.Text,
            Completed = inputTodo.Completed
        };

        db.Todos.Add(todo);
        await db.SaveChangesAsync();

        return Results.Created($"/todo/{todo.TodoId}", todo);
    });
    app.MapPut("/todo/{id}", async (int id, TodoInputDTO inputTodo, TodoDbContext db) =>
    {
        var todo = await db.Todos.FindAsync(id);

        if (todo is null)
            return Results.NotFound();

        // validare sempre i parametri di ingresso
        if (string.IsNullOrWhiteSpace(inputTodo.Text))
            return Results.BadRequest("Invalid Text");

        todo.Text = inputTodo.Text;
        todo.Completed = inputTodo.Completed;

        await db.SaveChangesAsync();

        return Results.NoContent();
    });
    app.MapDelete("/todo/{id}", async (int id, TodoDbContext db) =>
    {
        if (await db.Todos.FindAsync(id) is Todo todo)
        {
            db.Todos.Remove(todo);
            await db.SaveChangesAsync();
            return Results.Ok(todo);
        }

        return Results.NotFound();
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
record MultInputDTO(int V1, int V2);

// volendo posso usare direttamente la classe Todo senza creare un record
record TodoInputDTO(string Text, bool Completed);

class Todo
{
    public int TodoId { get; set; }
    public string? Text { get; set; }
    public bool Completed { get; set; }
};

// creo il context per il DB di EF
class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options)
        : base(options) { }

    public DbSet<Todo> Todos => Set<Todo>();
}