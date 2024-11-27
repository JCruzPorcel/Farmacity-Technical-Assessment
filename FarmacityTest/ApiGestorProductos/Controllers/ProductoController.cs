using Microsoft.AspNetCore.Mvc;
using ApiGestorProductos.Models;
using ApiGestorProductos.Services.Interfaces;
using ApiGestorProductos.DTOs;

namespace ApiGestorProductos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _service;

        public ProductoController(IProductoService service)
        {
            _service = service;
        }

        // Obtener todos los productos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productos = await _service.GetAllProductosAsync();
            return Ok(productos);
        }

        // Obtener producto por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var producto = await _service.GetProductoByIdAsync(id);
            if (producto == null) return NotFound(new { mensaje = "Producto no encontrado." });
            return Ok(producto);
        }

        // Crear producto
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Producto producto)
        {
            if (producto == null)
                return BadRequest(new { mensaje = "Los datos del producto son inválidos." });

            try
            {
                var productoDto = new ProductoDto
                {
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    CantidadEnStock = producto.CantidadEnStock,
                    Activo = producto.Activo,
                    FechaAlta = producto.FechaAlta,
                    FechaModificacion = producto.FechaModificacion,
                    CodigosBarra = producto.CodigosBarra?.Select(cb => new CodigoBarraDto
                    {
                        Codigo = cb.Codigo,
                        Activo = cb.Activo,
                        FechaAlta = cb.FechaAlta,
                        FechaModificacion = cb.FechaModificacion
                    }).ToList()
                };

                var createdProducto = await _service.CreateProductoAsync(productoDto);

                return CreatedAtAction(nameof(GetById), new { id = createdProducto.Id }, createdProducto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al crear el producto.", detalles = ex.Message });
            }
        }

        // Actualizar producto
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Producto producto)
        {
            if (producto == null || producto.Id != id)
                return BadRequest(new { mensaje = "El ID del producto es inválido." });

            try
            {
                var productoDto = new ProductoDto
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

                var updatedProducto = await _service.UpdateProductoAsync(productoDto);

                return Ok(updatedProducto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar el producto.", detalles = ex.Message });
            }
        }

        // Eliminar producto
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteProductoAsync(id);
                if (!result)
                    return NotFound(new { mensaje = "Producto no encontrado." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al eliminar el producto.", detalles = ex.Message });
            }
        }
    }
}
