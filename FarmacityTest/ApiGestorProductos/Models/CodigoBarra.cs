using Backend.Models;

public class CodigoBarra
{
    public string? Codigo { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaAlta { get; set; }
    public DateTime? FechaModificacion { get; set; }

    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }
}
