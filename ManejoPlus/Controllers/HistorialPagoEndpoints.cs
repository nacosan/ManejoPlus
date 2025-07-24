using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace ManejoPlus.Controllers;

public static class HistorialPagoEndpoints
{
    public static void MapHistorialPagoEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/HistorialPago").WithTags(nameof(HistorialPago));
        group.MapGet("/", async (ApplicationDbContext db) =>
        {
            return await db.HistorialPagos
                .Include(h => h.Suscripcion)
                    .ThenInclude(s => s.Plataforma)
                .ToListAsync();
        })
        .WithName("GetAllHistorialPagos")
        .WithOpenApi();
        group.MapGet("/{id}", async Task<Results<Ok<HistorialPago>, NotFound>> (int id, ApplicationDbContext db) =>
        {
            return await db.HistorialPagos.AsNoTracking()
                .FirstOrDefaultAsync(model => model.PagoID == id)
                is HistorialPago model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetHistorialPagoById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, HistorialPago historialPago, ApplicationDbContext db) =>
        {
            if (id != historialPago.PagoID)
                return TypedResults.NotFound();

            var existente = await db.HistorialPagos.FindAsync(id);
            if (existente == null)
                return TypedResults.NotFound();

            existente.SubscriptionID = historialPago.SubscriptionID;
            existente.FechaPago = historialPago.FechaPago;
            existente.Monto = historialPago.Monto;
            existente.Detalle = historialPago.Detalle;

            await db.SaveChangesAsync();
            return TypedResults.Ok();
        })
 .WithName("UpdateHistorialPago")
 .WithOpenApi();


        group.MapPost("/", async (HistorialPago historialPago, ApplicationDbContext db) =>
        {
            db.HistorialPagos.Add(historialPago);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/HistorialPago/{historialPago.PagoID}",historialPago);
        })
        .WithName("CreateHistorialPago")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApplicationDbContext db) =>
        {
            var affected = await db.HistorialPagos
                .Where(model => model.PagoID == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteHistorialPago")
        .WithOpenApi();
    }
}
