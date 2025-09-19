using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace ManejoPlus.Controllers;

public static class SuscripcionEndpoints
{
    public static void MapSuscripcionEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Suscripcion").WithTags(nameof(Suscripcion));

        group.MapGet("/", async (ApplicationDbContext db) =>
        {
            return await db.Suscripciones.ToListAsync();
        })
        .WithName("GetAllSuscripcions")
        .WithOpenApi();

      group.MapGet("/{id}", async Task<Results<Ok<Suscripcion>, NotFound>> (int id, ApplicationDbContext db) =>
{
    return await db.Suscripciones.AsNoTracking()
        .FirstOrDefaultAsync(model => model.SubscriptionID == id)
        is Suscripcion model
            ? TypedResults.Ok(model)
            : TypedResults.NotFound();
})
.WithName("GetSuscripcionById")
.WithOpenApi();


        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Suscripcion suscripcion, ApplicationDbContext db) =>
        {
            if (id != suscripcion.SubscriptionID)
                return TypedResults.NotFound();

            var existente = await db.Suscripciones.FindAsync(id);
            if (existente == null)
                return TypedResults.NotFound();

            existente.PlataformaID = suscripcion.PlataformaID;
            existente.PlanID = suscripcion.PlanID;
            existente.ApplicationUserId = suscripcion.ApplicationUserId;
            existente.NombrePersonalizado = suscripcion.NombrePersonalizado;
            existente.Tipo = suscripcion.Tipo;
            existente.Descripcion = suscripcion.Descripcion;
            existente.FechaInicio = suscripcion.FechaInicio;
            existente.FechaFin = suscripcion.FechaFin;
            existente.Estado = suscripcion.Estado;

            await db.SaveChangesAsync();
            return TypedResults.Ok();
        })
     .WithName("UpdateSuscripcion")
     .WithOpenApi();

        group.MapPost("/", async (Suscripcion suscripcion, ApplicationDbContext db) =>
        {
            db.Suscripciones.Add(suscripcion);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Suscripcion/{suscripcion.SubscriptionID}",suscripcion);
        })
        .WithName("CreateSuscripcion")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApplicationDbContext db) =>
        {
            var affected = await db.Suscripciones
                .Where(model => model.SubscriptionID == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteSuscripcion")
        .WithOpenApi();
    }
}
