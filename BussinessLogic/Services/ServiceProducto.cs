using System;
using System.Security.Cryptography.X509Certificates;
using BussinessLogic.DTO;
using DataAccess.IRepository;
using DataAccess.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace BussinessLogic.Services
{
    public class ServiceProducto
    {
        //Instancio el UnitOfWork que vamos a usar
        private readonly IUnitOfWork _unitOfWork;

        //Inyecto el UnitOfWork por el constructor, esto se hace para que se cree un nuevo contexto por cada vez que se llame a la clase
        public ServiceProducto(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IList<ProductoDTO>> GetAllProductos()
        {
            try
            {
                IList<Producto> productos = (await _unitOfWork.ProductoRepository.GetProductoCategoria()).Where(p => p.FechaBaja == null).ToList().OrderByDescending(x => x.FechaAlta).ToList();
                IList<ProductoDTO> productoDTO = productos.Adapt<IList<ProductoDTO>>();

                return productoDTO;

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public async Task<bool> EliminarProducto(int id)
        {
            Producto producto = await _unitOfWork.GenericRepository<Producto>().GetById(id);

            if (producto != null)
            {
                producto.FechaBaja = DateTime.Now;
                producto.FechaModificacion = DateTime.Now;
                Producto productoActualizado = await _unitOfWork.GenericRepository<Producto>().Update(producto);
            }


            return true;

        }

        public async Task<ProductoDTO> GetProductoById(int id)
        {
            Producto producto = await _unitOfWork.GenericRepository<Producto>().GetByIdIncludingRelations(id);
            return producto.Adapt<ProductoDTO>();
        }

        public async Task<int> CargarProducto(ProductoDTO producto)
        {
           Producto nuevoProducto = new Producto();
            nuevoProducto.FechaAlta = DateTime.Now;
            nuevoProducto.FechaModificacion = DateTime.Now;
            nuevoProducto.Descripcion = producto.Descripcion;
            nuevoProducto.Nombre = producto.Nombre;
            nuevoProducto.Precio = (float)producto.Precio;
            nuevoProducto.IdCategoria = producto.idCategoria;
            nuevoProducto.UrlImagen = producto.UrlImagen;
            Producto productoCargado = await _unitOfWork.GenericRepository<Producto>().Insert(nuevoProducto);

            return 0;

        }

        public async Task<int> EditarProducto(ProductoDTO producto)
        {
            try
            {
                Producto productoBase = await _unitOfWork.GenericRepository<Producto>().GetById(producto.IdProducto);

                if (productoBase != null){
                productoBase.Nombre = producto.Nombre;
                productoBase.Descripcion = producto.Descripcion;
                productoBase.Precio = producto.Precio.Adapt<float>();
                productoBase.IdCategoria = producto.idCategoria;
                productoBase.UrlImagen = producto.UrlImagen;
                productoBase.FechaModificacion = DateTime.Now;}

                Producto productoActualizado = await _unitOfWork.GenericRepository<Producto>().Update(productoBase);


            } catch (Exception ex)
            {
                throw ex;
            }
            return 0;

    }}}