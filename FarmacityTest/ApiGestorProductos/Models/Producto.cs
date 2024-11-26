namespace ApiGestorProductos.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public decimal Precio { get; set; }
        public int CantidadEnStock { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaModificacion { get; set; }

        // Relación con CodigoBarra (uno a muchos)
        public ICollection<CodigoBarra>? CodigosBarra { get; set; }
        
        public Producto()
        {
            FechaAlta = DateTime.UtcNow;
            FechaModificacion = DateTime.UtcNow;
        }
    }
}
