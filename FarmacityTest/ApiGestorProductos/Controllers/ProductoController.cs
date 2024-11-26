using Microsoft.AspNetCore.Mvc;
using ApiGestorProductos.Models;
using ApiGestorProductos.Services.Interfaces;

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
            var createdProducto = await _service.CreateProductoAsync(producto);
            return CreatedAtAction(nameof(GetById), new { id = createdProducto.Id }, createdProducto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Producto producto)
        {
            if (producto == null || producto.Id != id) return BadRequest();
            var updatedProducto = await _service.UpdateProductoAsync(producto);
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
