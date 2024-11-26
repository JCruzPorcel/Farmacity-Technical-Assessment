namespace ApiGestorProductos.Models
{
    public class CodigoBarra
    {
        public int ProductoId { get; set; }
        public string? Codigo { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaModificacion { get; set; }

        // Relación con Producto
        public Producto? Producto { get; set; }

        public CodigoBarra()
        {
            FechaAlta = DateTime.Now;
            FechaModificacion = DateTime.Now;
        }
    }
}
