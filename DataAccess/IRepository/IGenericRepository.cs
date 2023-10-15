using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace DataAccess.IRepository
{

    // Esta interfaz es para que la implementen los repositorios de cada entidad, generico para que no se repita codigo
    public interface IGenericRepository<T> where T : class
    {
        Task<IList<T>> GetAll();
        Task<T?> GetById(int id);
        Task<T> Insert(T entity);
        Task<T> Update(T entity);
        Task<bool> HardDelete(int id);
        Task<bool> SoftDelete(int id);
        Task<IList<T>> GetByCriteria(Expression<Func<T, bool>> predicate);
        Task<IList<T>> GetByCriteriaMemory(Expression<Func<T, bool>> predicate);
        



        //Otros metodos que se pueden implementar en el futuro

        // Task<bool> Exist(int id);
        // Task<bool> Exist(Func<T, bool> predicate);
        // Task<bool> RemoveAll(IEnumerable<T> entities);
        // Task<IEnumerable<T>> AddRange(IEnumerable<T> entities);

        
        // //el guid es para que el repositorio de cada entidad tenga un identificador unico pero esto ya se hace ya que tenemos los id autoincrementales
        // Guid GetGuid();


    } 
}