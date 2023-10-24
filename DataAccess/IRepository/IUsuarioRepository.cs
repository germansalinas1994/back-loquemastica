using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace DataAccess.IRepository
{

    // Esta interfaz es para que la implementen los repositorios de cada entidad, generico para que no se repita codigo
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        //hago una interfaz de la clase usuario para que el repositorio de usuario tenga sus propios metodos especificos



    } 
}