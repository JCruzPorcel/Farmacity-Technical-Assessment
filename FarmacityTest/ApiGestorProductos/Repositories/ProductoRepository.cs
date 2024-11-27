using Backend.Data;
using Backend.Models;
using Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
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
            var productoExistente = await _context.Productos
                                                  .Include(p => p.CodigosBarra)
                                                  .FirstOrDefaultAsync(p => p.Id == producto.Id);

            if (productoExistente == null)
                throw new Exception($"Producto con ID {producto.Id} no encontrado.");

            // Verificar si el producto ha cambiado realmente
            bool productoChanged = false;
            if (producto.Nombre != productoExistente.Nombre)
            {
                productoExistente.Nombre = producto.Nombre;
                productoChanged = true;
            }
            if (producto.Precio != productoExistente.Precio)
            {
                productoExistente.Precio = producto.Precio;
                productoChanged = true;
            }
            if (producto.CantidadEnStock != productoExistente.CantidadEnStock)
            {
                productoExistente.CantidadEnStock = producto.CantidadEnStock;
                productoChanged = true;
            }
            if (producto.Activo != productoExistente.Activo)
            {
                productoExistente.Activo = producto.Activo;
                productoChanged = true;
            }

            // Solo se actualiza la fecha de modificación si el producto ha cambiado
            if (productoChanged)
            {
                productoExistente.FechaModificacion = DateTime.UtcNow;
            }

            // Actualizar los códigos de barra
            if (producto.CodigosBarra != null && producto.CodigosBarra.Any())
            {
                foreach (var codigo in producto.CodigosBarra)
                {
                    var codigoExistente = productoExistente.CodigosBarra
                                                            .FirstOrDefault(cb => cb.ProductoId == codigo.ProductoId);
                    if (codigoExistente != null)
                    {
                        // Si el código ha cambiado, se actualiza
                        if (codigo.Codigo != codigoExistente.Codigo)
                        {
                            codigoExistente.Codigo = codigo.Codigo;
                            codigoExistente.FechaModificacion = DateTime.UtcNow;
                        }
                    }
                    else
                    {
                        // Si no existe, se agrega el nuevo código
                        productoExistente.CodigosBarra.Add(new CodigoBarra
                        {
                            Codigo = codigo.Codigo,
                            Activo = codigo.Activo,
                            FechaAlta = DateTime.UtcNow,
                            FechaModificacion = DateTime.UtcNow,
                            ProductoId = productoExistente.Id
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();
            return productoExistente;
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