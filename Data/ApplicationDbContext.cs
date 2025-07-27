using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ControlBodega.Models;

namespace ControlBodega.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Planta> Plantas { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }
        public DbSet<Ingreso> Ingresos { get; set; }
        public DbSet<Egreso> Egresos { get; set; }
        // public DbSet<Devolucion> Devoluciones { get; set; } // Eliminada duplicada
        // public DbSet<Consumo> Consumos { get; set; } // Eliminado duplicado
        public DbSet<Devolucion> Devoluciones { get; set; }
        public DbSet<EgresoAlimentacion> EgresoAlimentaciones { get; set; }
        public DbSet<Consumo> Consumos { get; set; }
        public DbSet<ResumenEgreso> ResumenEgresos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Egresos -> Plantas
            modelBuilder.Entity<Egreso>()
                .HasOne(e => e.Planta)
                .WithMany()
                .HasForeignKey(e => e.IdPlanta)
                .OnDelete(DeleteBehavior.NoAction);

            // Inventarios -> Plantas
            modelBuilder.Entity<Inventario>()
                .HasOne(i => i.Planta)
                .WithMany()
                .HasForeignKey(i => i.IdPlanta)
                .OnDelete(DeleteBehavior.NoAction);

            // Ingresos -> Plantas
            modelBuilder.Entity<Ingreso>()
                .HasOne(i => i.Planta)
                .WithMany()
                .HasForeignKey(i => i.IdPlanta)
                .OnDelete(DeleteBehavior.NoAction);

            // Devoluciones -> Plantas
            modelBuilder.Entity<Devolucion>()
                .HasOne(d => d.Planta)
                .WithMany()
                .HasForeignKey(d => d.IdPlanta)
                .OnDelete(DeleteBehavior.NoAction);

            // EgresosAlimentacion -> Plantas
            modelBuilder.Entity<EgresoAlimentacion>()
                .HasOne(ea => ea.Planta)
                .WithMany()
                .HasForeignKey(ea => ea.IdPlanta)
                .OnDelete(DeleteBehavior.NoAction);

            // ResumenEgresos -> Plantas
            modelBuilder.Entity<ResumenEgreso>()
                .HasOne(re => re.Planta)
                .WithMany()
                .HasForeignKey(re => re.IdPlanta)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
