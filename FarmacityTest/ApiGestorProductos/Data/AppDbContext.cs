using ApiGestorProductos.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiGestorProductos.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Producto> Productos { get; set; } = null!;
        public DbSet<CodigoBarra> CodigosBarra { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar precisión y escala para el campo 'Precio'
            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasPrecision(18, 2);  // Precisión total de 18 dígitos, 2 después del punto decimal

            modelBuilder.Entity<CodigoBarra>()
                .HasKey(cb => new { cb.ProductoId, cb.Codigo });

            // Configurar relación entre Producto y CodigoBarra
            modelBuilder.Entity<CodigoBarra>()
                .HasOne(cb => cb.Producto)
                .WithMany(p => p.CodigosBarra)
                .HasForeignKey(cb => cb.ProductoId);
        }

    }
}
