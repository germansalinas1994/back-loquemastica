using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.IRepository;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Linq.Expressions;

namespace DataAccess.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        //Considerar que como usamos addScoped en el startup, cada vez que se haga un request se va a crear un nuevo contexto
        //Por lo tanto no es necesario hacer un using para el contexto ya que se va a cerrar automaticamente al finalizar el request

        //Instancio el contexto que vamos a usar, para esto tengo que agregarlo en el startup
        protected readonly EcommercedbContext _context;
        public GenericRepository(EcommercedbContext mydbContext)
        {
            _context = mydbContext;
        }

        #region Traer todos los datos de una tabla
        //Metodo para traer todos los datos de una tabla
        public async Task<IList<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }
        #endregion
       
        #region buscar por ID
        //Metodo para traer un dato de un tipo de objeto por id
        public async Task<T?> GetById(int id)
        {

            //Esto lo hace por la base, hace el where de la base confirmado
            var entity = await _context.Set<T>().FindAsync(id);

            if (entity == null)
            {
                return null;
            }
            return entity;
        }
        #endregion
       
        #region insertar entidad generica
        //Metodo para insertar un dato de un tipo de objeto
        public async Task<T> Insert(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        #endregion

        #region Actualizar entidad generica 

        //Metodo para actualizar un dato de un tipo de objeto
        public async Task<T> Update(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        #endregion
       
        #region Eliminado Fisico
        //Metodo para eliminar un dato de un tipo de objeto, en este caso no se usa el soft delete
        public async Task<bool> HardDelete(int id)
        {
            var entity = await GetById(id);

            if(entity == null)
            {
                return false;
            }
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Soft Delete Generica
        //Metodo para hacer un soft delete de un dato de un tipo de objeto
        public async Task<bool> SoftDelete(int id)
        {
            var entity = await GetById(id);
            if (entity == null)
            {
                // No se encontró la entidad, retornamos false
                return false;
            }

            // Establecemos la fecha y hora actuales en la propiedad FechaHasta
            // Asumimos que todas las entidades tienen esta propiedad
            var propertyInfo = entity.GetType().GetProperty("FechaHasta");
            if (propertyInfo == null)
            {
                // La entidad no tiene la propiedad FechaHasta, retornamos false
                return false;
            }
            propertyInfo.SetValue(entity, DateTime.Now);

            // Guardamos los cambios en la base de datos
            await _context.SaveChangesAsync();

            return true;
        }


        #endregion

        #region Buscar bajo algun criterio
        //Metodo para traer datos de un tipo de objeto por algun criterio
        public async Task<IList<T>> GetByCriteria(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }


        //Otra forma de implementar el get by creiteria pero en este caso trae todo y busca en memoria, lo cual es mejor no hacer ya que no es tan eficiente
        //Es preferible usar el get by criteria ya que si hay muchos datos en la base es ineficiente
        #endregion
        
        #region Buscar bajo algun criterio en memoria
        public async Task<IList<T>> GetByCriteriaMemory(Expression<Func<T, bool>> predicate)
        {
            return await Task.FromResult(_context.Set<T>().Where(predicate).ToList());
        }
        #endregion

    }

}