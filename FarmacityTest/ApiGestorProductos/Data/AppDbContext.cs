using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
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

            // Relación entre Producto y CodigoBarra
            modelBuilder.Entity<CodigoBarra>()
                .HasOne(cb => cb.Producto)
                .WithMany(p => p.CodigosBarra)
                .HasForeignKey(cb => cb.ProductoId);

            modelBuilder.Entity<CodigoBarra>()
                .HasKey(cb => cb.ProductoId);

            // Configuración para el campo Precio de Producto
            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasColumnType("decimal(18,2)");  // Especifica precisión 18 y escala 2
        }

    }
}
