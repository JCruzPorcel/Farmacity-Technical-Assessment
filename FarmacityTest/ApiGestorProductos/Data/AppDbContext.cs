using ApiGestorProductos.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiGestorProductos.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        public DbSet<Producto> Productos { get; set; } = null!;
        public DbSet<CodigoBarra> CodigosBarra { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CodigoBarra>()
                .HasKey(cb => cb.Id);

            // Configuración para el campo Precio de Producto
            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasColumnType("decimal(18,2)");  // Especifica precisión 18 y escala 2
        }
    }
}
