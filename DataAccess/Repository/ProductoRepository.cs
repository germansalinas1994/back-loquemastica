
using DataAccess.IRepository;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class ProductoRepository : GenericRepository<Producto>, IProductoRepository
    {
        public ProductoRepository(EcommercedbContext context) : base(context)
        {
        }

        public async Task<IList<Producto>> GetProductoCategoria()
        {
            try
            {
                return await _context.Producto.Include(c => c.IdCategoriaNavigation).ToListAsync();


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // public IList<Producto> GetProductosByCategoria(int idCategoria)
        // {
        //     return _context.Productos.Where(p => p.idCategoria == idCategoria).ToList();
        // }

        // public IList<Producto> GetProductosByCategoriaAndNombre(int idCategoria, string nombre)
        // {
        //     return _context.Productos.Where(p => p.idCategoria == idCategoria && p.Nombre.Contains(nombre)).ToList();
        // }

        // public IList<Producto> GetProductosByNombre(string nombre)
        // {
        //     return _context.Productos.Where(p => p.Nombre.Contains(nombre)).ToList();
        // }

        // public IList<Producto> GetProductosByPrecio(decimal precio)
        // {
        //     return _context.Productos.Where(p => p.Precio == precio).ToList();
        // }

        // public IList<Producto> GetProductosByPrecioAndNombre(decimal precio, string nombre)
        // {
        //     return _context.Productos.Where(p => p.Precio == precio && p.Nombre.Contains(nombre)).ToList();
        // }

        // public IList<Producto> GetProductosByPrecioAndCategoria(decimal precio, int idCategoria)
        // {
        //     return _context.Productos.Where(p => p.Precio == precio && p.idCategoria == idCategoria).ToList();
        // }


    }
}