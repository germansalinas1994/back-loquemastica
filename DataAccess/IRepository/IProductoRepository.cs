using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace DataAccess.IRepository
{

    // Esta interfaz es para que la implementen los repositorios de cada entidad, generico para que no se repita codigo
    public interface IProductoRepository : IGenericRepository<Producto>
    {
        //hago una interfaz de la clase producto para que el repositorio de producto tenga sus propios metodos especificos

        // Task<IEnumerable<Producto>> GetByCategoria(int idCategoria);
        // Task<IEnumerable<Producto>> GetByPrecio(decimal precio);
        // Task<IEnumerable<Producto>> GetByPrecio(decimal precioMin, decimal precioMax);
        // Task<IEnumerable<Producto>> GetByNombre(string nombre);

        Task<IList<Producto>> GetProductoCategoria();
        
        // //el guid es para que el repositorio de cada entidad tenga un identificador unico

    } 
}