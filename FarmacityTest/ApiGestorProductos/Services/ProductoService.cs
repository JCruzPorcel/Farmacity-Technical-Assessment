using ApiGestorProductos.Models;
using ApiGestorProductos.Repositories.Interfaces;
using ApiGestorProductos.Services.Interfaces;

namespace ApiGestorProductos.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _repository;

        public ProductoService(IProductoRepository repository)
        {
            _repository = repository;
        }

        public Task<Producto> GetProductoByIdAsync(int id)
        {
            return _repository.GetProductoByIdAsync(id);
        }

        public Task<IEnumerable<Producto>> GetAllProductosAsync()
        {
            return _repository.GetAllProductosAsync();
        }

        public Task<Producto> CreateProductoAsync(Producto producto)
        {
            return _repository.AddProductoAsync(producto);
        }

        public Task<Producto> UpdateProductoAsync(Producto producto)
        {
            return _repository.UpdateProductoAsync(producto);
        }

        public Task<bool> DeleteProductoAsync(int id)
        {
            return _repository.DeleteProductoAsync(id);
        }
    }
}
