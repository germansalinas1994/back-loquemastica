using System;
using DataAccess.IRepository;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace DataAccess.Repository
{
    public class CategoriaRepository : GenericRepository<Categoria>, ICategoriaRepository
    {


        //que hace esta linea
        public CategoriaRepository(EcommercedbContext _context) : base(_context)
        {
        }

        public async Task<IList<Categoria>> GetAllCategoriasProductos()
        {
            //busco todas las categorias con el producto incluido
            return await _context.Categoria.Include(c => c.Producto).ToListAsync();
        }

        public Task<int> GetCantidadProductosByCategoria(int id)
        {
            //busco la categoria por id y cuento la cantidad de productos que tiene
            return _context.Categoria.Where(c => c.IdCategoria == id).Select(c => c.Producto.Count).FirstOrDefaultAsync();

        }

        public async Task<Categoria?> GetByIdCategoria(int id)
        {
            //esto tambien lo implementa haciendo la consulta sql
            return await _context.Categoria.FirstOrDefaultAsync(x => x.IdCategoria == id);


        }


    }
}

