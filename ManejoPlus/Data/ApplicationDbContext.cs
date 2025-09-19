using ManejoPlus.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Plan>()
            .Property(p => p.Precio)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Miembro>()
            .Property(m => m.MontoAportado)
            .HasPrecision(18, 2);

        modelBuilder.Entity<HistorialPago>()
            .Property(h => h.Monto)
            .HasPrecision(18, 2);

      
    }
}
