namespace ApiGestorProductos.DTOs
{
    public class ProductoDto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public decimal Precio { get; set; }
        public int CantidadEnStock { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public List<CodigoBarraDto>? CodigosBarra { get; set; }
    }
}
