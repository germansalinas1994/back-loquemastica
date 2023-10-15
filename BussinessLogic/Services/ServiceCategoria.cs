using System;
using System.Security.Cryptography.X509Certificates;
using BussinessLogic.DTO;
using DataAccess.IRepository;
using DataAccess.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace BussinessLogic.Services
{
    public class ServiceCategoria
    {
        //Instancio el UnitOfWork que vamos a usar
        private readonly IUnitOfWork _unitOfWork;

        //Inyecto el UnitOfWork por el constructor, esto se hace para que se cree un nuevo contexto por cada vez que se llame a la clase
        public ServiceCategoria(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IList<CategoriaDTO>> GetAllCategorias()
        {
            try
            {   
                // IList<Categoria> categorias = (await _unitOfWork.CategoriaRepository.GetAll()).OrderByDescending(x => x.FechaDesde).ToList();
                //Traigo todas las categorias que no tengan fecha de baja
                IList<Categoria> categorias = (await _unitOfWork.CategoriaRepository.GetByCriteria(x => x.FechaHasta == null)).OrderByDescending(x => x.FechaDesde).ToList();


                IList<CategoriaDTO> categoriaDTO = categorias.Adapt<IList<CategoriaDTO>>();


                foreach (var item in categoriaDTO)
                {
                    // item.CantidadProductos = item.Producto.Count();

                    item.CantidadProductos = await _unitOfWork.CategoriaRepository.GetCantidadProductosByCategoria(item.IdCategoria);
                }

                return categoriaDTO;

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        public async Task<CategoriaDTO> GetCategoriaById(int id)
        {
            return (await _unitOfWork.CategoriaRepository.GetById(id)).Adapt<CategoriaDTO>();

        }

        public async Task<CategoriaDTO> PostCategoria(CategoriaDTO categoria)
        {
            try
            {
                // Abre una transacción antes de realizar operaciones de base de datos
                await _unitOfWork.BeginTransactionAsync();

                Categoria categoriaEntity = categoria.Adapt<Categoria>();
                categoriaEntity.FechaDesde = DateTime.Now;
                categoriaEntity.FechaModificacion = DateTime.Now;
                categoriaEntity = await _unitOfWork.CategoriaRepository.Insert(categoriaEntity);


                //lo rompo aproposito para probar que funcione el rollback

                await _unitOfWork.CommitAsync();

                return categoriaEntity.Adapt<CategoriaDTO>();
            }
            catch (Exception ex)
            {
                // Maneja la excepción y realiza un rollback en caso de error
                await _unitOfWork.RollbackAsync();
                throw ex;
            }
        }

        public async Task DeleteCategoria(int id)
        {
            bool borrado = await _unitOfWork.CategoriaRepository.SoftDelete(id);
        }

        public async Task<CategoriaDTO> EditarCategoria(CategoriaDTO cate)
        {

            await _unitOfWork.BeginTransactionAsync();

            // IList<Categoria> categoriasDescripcion = await _unitOfWork.CategoriaRepository.GetByCriteria(x => x.Descripcion == value);
            // IList<Producto> categoriasId = await _unitOfWork.ProductoRepository.GetByCriteria(x => x.IdCategoriaNavigation.Descripcion == value);

            Categoria? categoria = await _unitOfWork.CategoriaRepository.GetById(cate.IdCategoria);

            if (categoria != null)
            {
                categoria.FechaModificacion = DateTime.Now;
                categoria.Descripcion = cate.Descripcion;
                categoria.Nombre = cate.Nombre;
                await _unitOfWork.CategoriaRepository.Update(categoria);

                await _unitOfWork.CommitAsync();

                return categoria.Adapt<CategoriaDTO>();
            }
            else
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception("No se encontro la categoria");
            }
        }

        public async Task<IList<CategoriaDTO>> BuscarCategorias(string descripcion)
        {
            //Puedo Buscar por si contiene el nombre o podria buscar por is es igual al nombre
            // IList<Categoria> categorias = await _unitOfWork.CategoriaRepository.GetByCriteria(x => x.Descripcion.Contains(descripcion));

            IList<Categoria> categorias = await _unitOfWork.CategoriaRepository.GetByCriteria(x => x.Descripcion == descripcion);

            IList<CategoriaDTO> categoriaDTO = categorias.Adapt<IList<CategoriaDTO>>();

            foreach (var item in categoriaDTO)
            {
                item.CantidadProductos = await _unitOfWork.CategoriaRepository.GetCantidadProductosByCategoria(item.IdCategoria);
            }

            return categoriaDTO;
        }
    }
}