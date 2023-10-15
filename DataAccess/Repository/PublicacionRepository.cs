
using DataAccess.IRepository;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class PublicacionRepository : GenericRepository<Publicacion>, IPublicacionRepository
    {
        public PublicacionRepository(EcommercedbContext context) : base(context)
        {
        }

        public async Task<IList<Publicacion>> GetAllPublicaciones()
        {
            try
            {
                return await _context.Publicacion.Include(x => x.IdProductoNavigation).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Publicacion> GetPublicacionById(int id)
        {
            try
            {
                //de esta forma incluyo las relaciones de la publicacion, con el producto y del producto con la categoria
                return await _context.Publicacion.Include(x => x.IdProductoNavigation).
                ThenInclude(p => p.IdCategoriaNavigation).FirstOrDefaultAsync(x => x.IdPublicacion == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<List<Publicacion>> GetPublicacionesCarrito(List<int> ids)
        {
            try
            {
                return _context.Publicacion.Include(x => x.IdProductoNavigation).Where(x => ids.Contains(x.IdPublicacion)).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}