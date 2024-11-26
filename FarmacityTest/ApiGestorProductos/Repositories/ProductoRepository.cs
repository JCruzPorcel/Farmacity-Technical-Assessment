using ApiGestorProductos.Data;
using ApiGestorProductos.Models;
using ApiGestorProductos.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiGestorProductos.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly AppDbContext _context;

        public ProductoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Producto> GetProductoByIdAsync(int id)
        {
            var producto = await _context.Productos.Include(p => p.CodigosBarra).FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
            {
                throw new KeyNotFoundException("Producto no encontrado");
            }

            return producto;
        }


        public async Task<IEnumerable<Producto>> GetAllProductosAsync()
        {
            return await _context.Productos.Include(p => p.CodigosBarra).ToListAsync();
        }

        public async Task<Producto> AddProductoAsync(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        public async Task<Producto> UpdateProductoAsync(Producto producto)
        {
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        public async Task<bool> DeleteProductoAsync(int id)
        {
            var producto = await GetProductoByIdAsync(id);
            if (producto == null) return false;

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
