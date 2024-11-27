namespace Backend.DTOs
{
    public class CodigoBarraDto
    {
        public int ProductoId { get; set; }
        public string? Codigo { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
