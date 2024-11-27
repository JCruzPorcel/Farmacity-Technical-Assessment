using Microsoft.EntityFrameworkCore;
using ApiGestorProductos.Models;
using ApiGestorProductos.Repositories.Interfaces;
using ApiGestorProductos.Data;

namespace ApiGestorProductos.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly AppDbContext _context;

        public ProductoRepository(AppDbContext context)
        {
            _context = context;
        }

        // Obtener Producto por ID
        public async Task<Producto> GetProductoByIdAsync(int id)
        {
            var producto = await _context.Productos
                                         .Include(p => p.CodigosBarra)
                                         .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
            {
                throw new Exception($"Producto con ID {id} no encontrado.");
            }

            return producto;
        }

        // Obtener todos los productos
        public async Task<IEnumerable<Producto>> GetAllProductosAsync()
        {
            return await _context.Productos
                                 .Include(p => p.CodigosBarra)
                                 .ToListAsync();
        }

        // Agregar un nuevo Producto
        public async Task<Producto> AddProductoAsync(Producto producto)
        {
            await _context.Productos.AddAsync(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        // Actualizar un Producto
        public async Task<Producto> UpdateProductoAsync(Producto producto)
        {
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        // Eliminar un Producto
        public async Task<bool> DeleteProductoAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return false;

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
