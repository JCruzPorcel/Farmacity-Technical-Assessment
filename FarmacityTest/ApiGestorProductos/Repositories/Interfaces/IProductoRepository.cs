using Backend.Models;

namespace Backend.Repositories.Interfaces
{
    public interface IProductoRepository
    {
        Task<Producto> GetProductoByIdAsync(int id);
        Task<IEnumerable<Producto>> GetAllProductosAsync();
        Task<Producto> AddProductoAsync(Producto producto);
        Task<Producto> UpdateProductoAsync(Producto producto);
        Task<bool> DeleteProductoAsync(int id);
    }
}
