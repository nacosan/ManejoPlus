using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using ManejoPlus.Models;

namespace ManejoPlus.Controllers;

public static class MiembroEndpoints
{
    public static void MapMiembroEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Miembro").WithTags(nameof(Miembro));

        // GET: api/Miembro
        group.MapGet("/", async (ApplicationDbContext db) =>
        {
            var miembros = await db.Miembros
                .Include(m => m.Suscripcion) 
                .ToListAsync();

            return TypedResults.Ok(miembros);
        })
        .WithName("GetAllMiembros")
        .WithOpenApi();

        // GET: api/Miembro/{id}
        group.MapGet("/{id}", async Task<Results<Ok<Miembro>, NotFound>> (int id, ApplicationDbContext db) =>
        {
            var miembro = await db.Miembros
                .AsNoTracking()
                .Include(m => m.Suscripcion)
                .FirstOrDefaultAsync(m => m.MiembroID == id);

            return miembro is not null ? TypedResults.Ok(miembro) : TypedResults.NotFound();
        })
        .WithName("GetMiembroById")
        .WithOpenApi();

        // PUT: api/Miembro/{id}
        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Miembro miembro, ApplicationDbContext db) =>
        {
            if (id != miembro.MiembroID)
                return TypedResults.NotFound();

            var existente = await db.Miembros.FindAsync(id);
            if (existente == null)
                return TypedResults.NotFound();

            existente.SubscriptionID = miembro.SubscriptionID;
            existente.NombreMiembro = miembro.NombreMiembro;
            existente.EmailOpcional = miembro.EmailOpcional;
            existente.Rol = miembro.Rol;
            existente.MontoAportado = miembro.MontoAportado;
            existente.ApplicationUserId = miembro.ApplicationUserId;

            await db.SaveChangesAsync();
            return TypedResults.Ok();
        })
   .WithName("UpdateMiembro")
   .WithOpenApi();


        // POST: api/Miembro
        group.MapPost("/", async (Miembro miembro, ApplicationDbContext db) =>
        {
            db.Miembros.Add(miembro);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/api/Miembro/{miembro.MiembroID}", miembro);
        })
        .WithName("CreateMiembro")
        .WithOpenApi();

        // DELETE: api/Miembro/{id}
        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApplicationDbContext db) =>
        {
            var miembro = await db.Miembros.FindAsync(id);
            if (miembro is null) return TypedResults.NotFound();

            db.Miembros.Remove(miembro);
            await db.SaveChangesAsync();

            return TypedResults.Ok();
        })
        .WithName("DeleteMiembro")
        .WithOpenApi();
    }
}
