using System.Numerics;
using ManejoPlus.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Plataforma> Plataformas { get; set; }
    public DbSet<Plan> Planes { get; set; }
    public DbSet<Suscripcion> Suscripciones { get; set; }
    public DbSet<Miembro> Miembros { get; set; }
    public DbSet<HistorialPago> HistorialPagos { get; set; }

}
