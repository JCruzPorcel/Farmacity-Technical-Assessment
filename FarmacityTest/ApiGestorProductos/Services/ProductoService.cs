using ApiGestorProductos.DTOs;
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

        // Obtener Producto por ID
        public async Task<ProductoDto> GetProductoByIdAsync(int id)
        {
            var producto = await _repository.GetProductoByIdAsync(id);
            if (producto == null) throw new Exception("Producto no encontrado.");

            return MapProductoToDto(producto);
        }

        // Obtener todos los Productos
        public async Task<IEnumerable<ProductoDto>> GetAllProductosAsync()
        {
            var productos = await _repository.GetAllProductosAsync();
            return productos.Select(MapProductoToDto).ToList();
        }

        // Crear un Producto
        public async Task<ProductoDto> CreateProductoAsync(ProductoDto productoDto)
        {
            if (productoDto == null)
                throw new ArgumentNullException(nameof(productoDto), "El producto no puede ser nulo.");

            var producto = MapDtoToProducto(productoDto);
            var createdProducto = await _repository.AddProductoAsync(producto);
            return MapProductoToDto(createdProducto);
        }

        // Actualizar Producto
        public async Task<ProductoDto> UpdateProductoAsync(ProductoDto productoDto)
        {
            if (productoDto == null)
                throw new ArgumentNullException(nameof(productoDto), "El producto no puede ser nulo.");

            // Buscar el producto en el repositorio
            var productoExistente = await _repository.GetProductoByIdAsync(productoDto.Id);
            if (productoExistente == null)
                throw new Exception("Producto no encontrado.");

            // Actualizar producto y códigos de barra
            UpdateProductoFields(productoExistente, productoDto);
            var updatedProducto = await _repository.UpdateProductoAsync(productoExistente);

            return MapProductoToDto(updatedProducto);
        }

        // Eliminar Producto
        public async Task<bool> DeleteProductoAsync(int id)
        {
            return await _repository.DeleteProductoAsync(id);
        }

        // Métodos auxiliares

        private ProductoDto MapProductoToDto(Producto producto)
        {
            return new ProductoDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                CantidadEnStock = producto.CantidadEnStock,
                Activo = producto.Activo,
                FechaAlta = producto.FechaAlta,
                FechaModificacion = producto.FechaModificacion,
                CodigosBarra = producto.CodigosBarra?.Select(cb => new CodigoBarraDto
                {
                    ProductoId = cb.ProductoId,
                    Codigo = cb.Codigo,
                    Activo = cb.Activo,
                    FechaAlta = cb.FechaAlta,
                    FechaModificacion = cb.FechaModificacion
                }).ToList()
            };
        }

        private Producto MapDtoToProducto(ProductoDto productoDto)
        {
            return new Producto
            {
                Nombre = productoDto.Nombre,
                Precio = productoDto.Precio,
                CantidadEnStock = productoDto.CantidadEnStock,
                Activo = productoDto.Activo,
                FechaAlta = DateTime.UtcNow,
                FechaModificacion = null,
                CodigosBarra = productoDto.CodigosBarra!.Select(cbDto => new CodigoBarra
                {
                    Codigo = cbDto.Codigo,
                    Activo = cbDto.Activo,
                    FechaAlta = DateTime.UtcNow,
                    FechaModificacion = null
                }).ToList()
            };
        }

        private void UpdateProductoFields(Producto productoExistente, ProductoDto productoDto)
        {
            productoExistente.Nombre = productoDto.Nombre;
            productoExistente.Precio = productoDto.Precio;
            productoExistente.CantidadEnStock = productoDto.CantidadEnStock;
            productoExistente.Activo = productoDto.Activo;
            productoExistente.FechaModificacion = DateTime.UtcNow;

            // Actualizar códigos de barra
            if (productoDto.CodigosBarra != null && productoDto.CodigosBarra.Any())
            {
                productoExistente.CodigosBarra.Clear();
                productoExistente.CodigosBarra = productoDto.CodigosBarra.Select(cbDto => new CodigoBarra
                {
                    Codigo = cbDto.Codigo,
                    Activo = cbDto.Activo,
                    FechaAlta = DateTime.UtcNow,
                    FechaModificacion = DateTime.UtcNow
                }).ToList();
            }
        }
    }
}
