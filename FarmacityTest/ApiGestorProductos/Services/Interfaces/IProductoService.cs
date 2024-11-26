using ApiGestorProductos.Models;

namespace ApiGestorProductos.Services.Interfaces
{
    public interface IProductoService
    {
        Task<Producto> GetProductoByIdAsync(int id);
        Task<IEnumerable<Producto>> GetAllProductosAsync();
        Task<Producto> CreateProductoAsync(Producto producto);
        Task<Producto> UpdateProductoAsync(Producto producto);
        Task<bool> DeleteProductoAsync(int id);
    }
}
