using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace DataAccess.IRepository
{

    // Esta interfaz es para que la implementen los repositorios de cada entidad, generico para que no se repita codigo
    public interface IPublicacionRepository : IGenericRepository<Publicacion>
    {
        Task<IList<Publicacion>> GetAllPublicaciones();  

        Task<Publicacion> GetPublicacionById(int id);

        Task<List<Publicacion>> GetPublicacionesCarrito(List<int> ids);

    } 

    
}