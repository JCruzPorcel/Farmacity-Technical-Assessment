// Servicio para la lógica de negocio de productos
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

            // Convertir entidad Producto a DTO antes de retornar
            return MapProductoToDto(producto);
        }

        // Obtener todos los productos
        public async Task<IEnumerable<ProductoDto>> GetAllProductosAsync()
        {
            var productos = await _repository.GetAllProductosAsync();
            // Convertir cada Producto en un DTO
            return productos.Select(MapProductoToDto).ToList();
        }

        // Crear un nuevo Producto
        public async Task<ProductoDto> CreateProductoAsync(ProductoDto productoDto)
        {
            if (productoDto == null)
                throw new ArgumentNullException(nameof(productoDto), "El producto no puede ser nulo.");

            // Mapear DTO a entidad Producto
            var producto = MapDtoToProducto(productoDto);

            // Guardar en el repositorio
            var createdProducto = await _repository.AddProductoAsync(producto);

            // Convertir entidad creada a DTO antes de retornar
            return MapProductoToDto(createdProducto);
        }

        // Actualizar Producto existente
        public async Task<ProductoDto> UpdateProductoAsync(ProductoDto productoDto)
        {
            if (productoDto == null)
                throw new ArgumentNullException(nameof(productoDto), "El producto no puede ser nulo.");

            // Buscar producto en la base de datos
            var productoExistente = await _repository.GetProductoByIdAsync(productoDto.Id);
            if (productoExistente == null)
                throw new Exception("Producto no encontrado.");

            // Actualizar campos del Producto y sus códigos de barra
            UpdateProductoFields(productoExistente, productoDto);

            // Guardar cambios en el repositorio
            var updatedProducto = await _repository.UpdateProductoAsync(productoExistente);

            // Convertir entidad actualizada a DTO antes de retornar
            return MapProductoToDto(updatedProducto);
        }

        // Eliminar Producto
        public async Task<bool> DeleteProductoAsync(int id)
        {
            // Eliminar por ID usando el repositorio
            return await _repository.DeleteProductoAsync(id);
        }

        // Mapeo de entidad Producto a DTO
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

        // Mapeo de DTO a entidad Producto
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

        // Actualizar campos y relaciones del Producto existente
        private void UpdateProductoFields(Producto productoExistente, ProductoDto productoDto)
        {
            // Actualizar campos simples
            productoExistente.Nombre = productoDto.Nombre;
            productoExistente.Precio = productoDto.Precio;
            productoExistente.CantidadEnStock = productoDto.CantidadEnStock;
            productoExistente.Activo = productoDto.Activo;
            productoExistente.FechaModificacion = DateTime.UtcNow;

            // Actualizar relación de códigos de barra
            var codigosAEliminar = productoExistente.CodigosBarra
                .Where(cb => !productoDto.CodigosBarra!.Any(dto => dto.Codigo == cb.Codigo))
                .ToList();

            foreach (var codigo in codigosAEliminar)
            {
                productoExistente.CodigosBarra.Remove(codigo);
            }

            foreach (var cbDto in productoDto.CodigosBarra!)
            {
                var codigoExistente = productoExistente.CodigosBarra
                    .FirstOrDefault(cb => cb.Codigo == cbDto.Codigo);

                if (codigoExistente != null)
                {
                    codigoExistente.Activo = cbDto.Activo;
                    codigoExistente.FechaModificacion = DateTime.UtcNow;
                }
                else
                {
                    productoExistente.CodigosBarra.Add(new CodigoBarra
                    {
                        Codigo = cbDto.Codigo,
                        Activo = cbDto.Activo,
                        FechaAlta = DateTime.UtcNow
                    });
                }
            }
        }
    }
}
