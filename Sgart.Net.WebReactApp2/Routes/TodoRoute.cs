using Microsoft.AspNetCore.Mvc;
using Sgart.Net.ConsoleApp.BO.InputDTO;
using Sgart.Net.WebReactApp2.Services;

namespace Sgart.Net.WebReactApp2.Routes
{
    public static class TodoRoute
    {
        public static void Add(WebApplication app, string initialPath)
        {
            app.MapGet(initialPath, async (SgartDIExampleService service) => await service.GetAllAsync());
            app.MapGet($"{initialPath}/{{id:int}}", async (int id, SgartDIExampleService service) => await service.GetAsync(id));
            app.MapPost(initialPath, async ([FromBody] TodoAddDTO data, SgartDIExampleService service) =>
            {
                var todo = await service.AddAsync(data);
                if (todo != null)
                {
                    return Results.NoContent();
                    //return Results.Created($"/api/todo/{todo.TodoId}", todo);
                }

                return Results.BadRequest();
            });
            app.MapPut(initialPath, async ([FromBody] TodoEditDTO data, SgartDIExampleService service) =>
            {
                if (await service.EditAsync(data))
                    return Results.NoContent();
                return Results.BadRequest();
            });
            app.MapDelete("/api/todo/{id}", async (int id, SgartDIExampleService service) =>
            {
                if (await service.DeleteAsync(id))
                    return Results.NoContent();
                return Results.BadRequest();
            });
        }
    }
}
