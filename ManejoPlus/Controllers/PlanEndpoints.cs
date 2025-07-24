using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace ManejoPlus.Controllers;

public static class PlanEndpoints
{
    public static void MapPlanEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Plan").WithTags(nameof(Plan));

        group.MapGet("/", async (ApplicationDbContext db) =>
        {
            return await db.Planes
                .Include(p => p.Plataforma) 
                .ToListAsync();
        })
        .WithName("GetAllPlans")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Plan>, NotFound>> (int id, ApplicationDbContext db) =>
        {
            return await db.Planes.AsNoTracking()
                .FirstOrDefaultAsync(model => model.PlanID == id)
                is Plan model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetPlanById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Plan plan, ApplicationDbContext db) =>
        {
            var existing = await db.Planes.FindAsync(id);
            if (existing == null) return TypedResults.NotFound();

            existing.PlataformaID = plan.PlataformaID;
            existing.Nombre = plan.Nombre;
            existing.Precio = plan.Precio;
            existing.Periodicidad = plan.Periodicidad;
            existing.Descripcion = plan.Descripcion;

            await db.SaveChangesAsync();
            return TypedResults.Ok();
        })
 .WithName("UpdatePlan")
 .WithOpenApi();


        group.MapPost("/", async (Plan plan, ApplicationDbContext db) =>
        {
            db.Planes.Add(plan);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Plan/{plan.PlanID}",plan);
        })
        .WithName("CreatePlan")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApplicationDbContext db) =>
        {
            var affected = await db.Planes
                .Where(model => model.PlanID == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeletePlan")
        .WithOpenApi();
    }
}
