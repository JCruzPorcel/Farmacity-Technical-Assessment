namespace ApiGestorProductos.Models
{
    public class CodigoBarra
    {
        public int ProductoId { get; set; }   // Clave foránea hacia Producto
        public string? Codigo { get; set; }    // Código único para el producto

        public bool Activo { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaModificacion { get; set; }

        // Relación con Producto
        public Producto? Producto { get; set; }
    }
}
 