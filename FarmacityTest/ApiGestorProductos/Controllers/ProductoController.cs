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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productos = await _service.GetAllProductosAsync();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var producto = await _service.GetProductoByIdAsync(id);
            if (producto == null) return NotFound();
            return Ok(producto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Producto producto)
        {
            if (producto == null) return BadRequest();

            // Convertir Producto a ProductoDto antes de pasar al servicio
            var productoDto = new ProductoDto
            {
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                CantidadEnStock = producto.CantidadEnStock,
                Activo = producto.Activo,
                FechaAlta = producto.FechaAlta,
                FechaModificacion = producto.FechaModificacion
            };

            var createdProducto = await _service.CreateProductoAsync(productoDto);

            // Retornar el ProductoDto creado
            return CreatedAtAction(nameof(GetById), new { id = createdProducto.Id }, createdProducto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Producto producto)
        {
            if (producto == null || producto.Id != id) return BadRequest();

            // Convertir Producto a ProductoDto antes de pasar al servicio
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
                }).ToList() // Asegúrate de convertir los códigos de barra en el DTO correspondiente
            };

            var updatedProducto = await _service.UpdateProductoAsync(productoDto);

            // Retornar el ProductoDto actualizado
            return Ok(updatedProducto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteProductoAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
