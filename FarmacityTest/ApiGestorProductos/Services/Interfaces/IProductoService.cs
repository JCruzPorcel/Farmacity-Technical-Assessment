using ApiGestorProductos.DTOs;

namespace ApiGestorProductos.Services.Interfaces
{
    public interface IProductoService
    {
        Task<ProductoDto> GetProductoByIdAsync(int id);
        Task<IEnumerable<ProductoDto>> GetAllProductosAsync(); 
        Task<ProductoDto> CreateProductoAsync(ProductoDto productoDto);
        Task<ProductoDto> UpdateProductoAsync(ProductoDto productoDto);
        Task<bool> DeleteProductoAsync(int id);
    }
}
