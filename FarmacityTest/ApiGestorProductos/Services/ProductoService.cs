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

        // GetProductoByIdAsync ahora retorna ProductoDto
        public async Task<ProductoDto> GetProductoByIdAsync(int id)
        {
            try
            {
                var producto = await _repository.GetProductoByIdAsync(id);
                if (producto == null)
                {
                    throw new Exception("Producto no encontrado con el ID proporcionado.");
                }

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
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el producto: " + ex.Message);
            }
        }

        // GetAllProductosAsync ahora retorna IEnumerable<ProductoDto>
        public async Task<IEnumerable<ProductoDto>> GetAllProductosAsync()
        {
            try
            {
                var productos = await _repository.GetAllProductosAsync();
                if (productos == null || !productos.Any())
                {
                    return new List<ProductoDto>(); // Retorna una lista vacía si no hay productos
                }

                return productos.Select(producto => new ProductoDto
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
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los productos: " + ex.Message);
            }
        }

        // CreateProductoAsync ahora espera ProductoDto y retorna ProductoDto
        public async Task<ProductoDto> CreateProductoAsync(ProductoDto productoDto)
        {
            try
            {
                if (productoDto == null)
                {
                    throw new ArgumentNullException(nameof(productoDto), "El producto no puede ser nulo.");
                }

                // Las fechas de alta y modificación se asignan dentro de la lógica de negocio.
                var producto = new Producto
                {
                    Nombre = productoDto.Nombre,
                    Precio = productoDto.Precio,
                    CantidadEnStock = productoDto.CantidadEnStock,
                    Activo = productoDto.Activo,
                };

                // Si el productoDto tiene códigos de barra, los asignamos a producto
                if (productoDto.CodigosBarra != null && productoDto.CodigosBarra.Any())
                {
                    producto.CodigosBarra = productoDto.CodigosBarra.Select(cbDto => new CodigoBarra
                    {
                        Codigo = cbDto.Codigo,
                        Activo = cbDto.Activo,
                    }).ToList();
                }

                var createdProducto = await _repository.AddProductoAsync(producto);

                return new ProductoDto
                {
                    Id = createdProducto.Id,
                    Nombre = createdProducto.Nombre,
                    Precio = createdProducto.Precio,
                    CantidadEnStock = createdProducto.CantidadEnStock,
                    Activo = createdProducto.Activo,
                    CodigosBarra = createdProducto.CodigosBarra?.Select(cb => new CodigoBarraDto
                    {
                        ProductoId = cb.ProductoId,
                        Codigo = cb.Codigo,
                        Activo = cb.Activo,
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el producto: " + ex.Message);
            }
        }

        // UpdateProductoAsync ahora espera ProductoDto y retorna ProductoDto
        public async Task<ProductoDto> UpdateProductoAsync(ProductoDto productoDto)
        {
            try
            {
                if (productoDto == null)
                {
                    throw new ArgumentNullException(nameof(productoDto), "El producto no puede ser nulo.");
                }

                var producto = new Producto
                {
                    Id = productoDto.Id,
                    Nombre = productoDto.Nombre,
                    Precio = productoDto.Precio,
                    CantidadEnStock = productoDto.CantidadEnStock,
                    Activo = productoDto.Activo,
                    FechaModificacion = DateTime.UtcNow,
                };

                // Si el productoDto tiene códigos de barra, actualizarlos
                if (productoDto.CodigosBarra != null && productoDto.CodigosBarra.Any())
                {
                    producto.CodigosBarra = productoDto.CodigosBarra.Select(cbDto => new CodigoBarra
                    {
                        ProductoId = cbDto.ProductoId,
                        Codigo = cbDto.Codigo,
                        Activo = cbDto.Activo,
                        FechaAlta = cbDto.FechaAlta,
                        FechaModificacion = DateTime.UtcNow
                    }).ToList();
                }

                var updatedProducto = await _repository.UpdateProductoAsync(producto);

                return new ProductoDto
                {
                    Id = updatedProducto.Id,
                    Nombre = updatedProducto.Nombre,
                    Precio = updatedProducto.Precio,
                    CantidadEnStock = updatedProducto.CantidadEnStock,
                    Activo = updatedProducto.Activo,
                    FechaAlta = updatedProducto.FechaAlta,
                    FechaModificacion = updatedProducto.FechaModificacion,
                    CodigosBarra = updatedProducto.CodigosBarra?.Select(cb => new CodigoBarraDto
                    {
                        ProductoId = cb.ProductoId,
                        Codigo = cb.Codigo,
                        Activo = cb.Activo,
                        FechaAlta = cb.FechaAlta,
                        FechaModificacion = cb.FechaModificacion
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el producto: " + ex.Message);
            }
        }

        // DeleteProductoAsync sigue igual, ya que retorna un valor booleano
        public async Task<bool> DeleteProductoAsync(int id)
        {
            try
            {
                var result = await _repository.DeleteProductoAsync(id);
                if (!result)
                {
                    throw new Exception("No se pudo eliminar el producto.");
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el producto: " + ex.Message);
            }
        }
    }
}
