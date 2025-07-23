using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace ManejoPlus.Controllers;

public static class PlataformaEndpoints
{
    public static void MapPlataformaEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Plataforma").WithTags(nameof(Plataforma));

        group.MapGet("/", async (ApplicationDbContext db) =>
        {
            return await db.Plataformas.ToListAsync();
        })
        .WithName("GetAllPlataformas")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Plataforma>, NotFound>> (int id, ApplicationDbContext db) =>
        {
            return await db.Plataformas.AsNoTracking()
                .FirstOrDefaultAsync(model => model.PlataformaID == id)
                is Plataforma model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetPlataformaById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Plataforma plataforma, ApplicationDbContext db) =>
        {
            if (id != plataforma.PlataformaID)
                return TypedResults.NotFound();

            var existing = await db.Plataformas.FindAsync(id);
            if (existing == null)
                return TypedResults.NotFound();

            existing.Nombre = plataforma.Nombre;
            existing.Descripcion = plataforma.Descripcion;
            existing.EsPersonalizada = plataforma.EsPersonalizada;

            await db.SaveChangesAsync();
            return TypedResults.Ok();
        })
     .WithName("UpdatePlataforma")
     .WithOpenApi();

        group.MapPost("/", async (Plataforma plataforma, ApplicationDbContext db) =>
        {
            db.Plataformas.Add(plataforma);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Plataforma/{plataforma.PlataformaID}",plataforma);
        })
        .WithName("CreatePlataforma")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApplicationDbContext db) =>
        {
            var affected = await db.Plataformas
                .Where(model => model.PlataformaID == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeletePlataforma")
        .WithOpenApi();
    }
}
