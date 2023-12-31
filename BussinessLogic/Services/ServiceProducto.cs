using System;
using System.Security.Cryptography.X509Certificates;
using BussinessLogic.DTO;
using DataAccess.IRepository;
using DataAccess.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using AutoWrapper.Wrappers;

namespace BussinessLogic.Services
{
    public class ServiceProducto
    {
        //Instancio el UnitOfWork que vamos a usar
        private readonly IUnitOfWork _unitOfWork;
        private readonly ServiceSucursal _serviceSucursal;

        private readonly ServiceGoogleCloud _serviceGoogleCloud;

        //Inyecto el UnitOfWork por el constructor, esto se hace para que se cree un nuevo contexto por cada vez que se llame a la clase
        public ServiceProducto(IUnitOfWork unitOfWork, ServiceGoogleCloud serviceGoogleCloud)
        {
            _unitOfWork = unitOfWork;
            _serviceSucursal = new ServiceSucursal(_unitOfWork);
            _serviceGoogleCloud = serviceGoogleCloud;

        }


        public async Task<IList<ProductoDTO>> GetAllProductos()
        {
            try
            {
                IList<Producto> productos = (await _unitOfWork.ProductoRepository.GetProductoCategoria()).Where(p => p.FechaBaja == null).ToList().OrderByDescending(x => x.IdProducto).ToList();

                return productos.Adapt<IList<ProductoDTO>>();

            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }


        }

        public async Task EliminarProducto(int id)
        {
            try
            {
                Producto producto = await _unitOfWork.GenericRepository<Producto>().GetById(id);

                if (producto != null)
                {
                    producto.FechaBaja = DateTime.Now;
                    producto.FechaModificacion = DateTime.Now;
                    Producto productoActualizado = await _unitOfWork.GenericRepository<Producto>().Update(producto);
                }
                else
                {
                    throw new ApiException("El producto no existe");
                }

            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }

        }

        public async Task<ProductoDTO> GetProductoById(int id)
        {
            try
            {
                Producto producto = await _unitOfWork.GenericRepository<Producto>().GetByIdIncludingRelations(id);
                return producto.Adapt<ProductoDTO>();

            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }
        }

        public async Task CargarProducto(ProductoDTO producto)
        {

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                Producto nuevoProducto = new Producto();
                nuevoProducto.FechaAlta = DateTime.Now;
                nuevoProducto.FechaModificacion = DateTime.Now;
                nuevoProducto.Descripcion = producto.Descripcion;
                nuevoProducto.Nombre = producto.Nombre;
                nuevoProducto.Precio = (float)producto.Precio;
                nuevoProducto.IdCategoria = producto.idCategoria;
                //La imagen vamos a tener que llamar a google cloudfrom, esto despues lo vemos
                nuevoProducto.UrlImagen = await _serviceGoogleCloud.SubirImagenAsync(producto.Archivo);


                // nuevoProducto.UrlImagen = producto.UrlImagen;

                Producto productoCargado = await _unitOfWork.GenericRepository<Producto>().Insert(nuevoProducto);

                IList<SucursalDTO> sucursales = (await _serviceSucursal.GetSucursales()).ToList();

                foreach (var sucursal in sucursales)
                {
                    Publicacion publicacion = new Publicacion();
                    publicacion.IdProducto = productoCargado.IdProducto;
                    publicacion.IdSucursal = sucursal.IdSucursal;
                    publicacion.Stock = 0;
                    publicacion.FechaDesde = DateTime.Now;
                    publicacion.FechaActualizacion = DateTime.Now;


                    //aca no es necesario devolverlo, en caso de que falle el insert va a tirar una excepcion
                    await _unitOfWork.GenericRepository<Publicacion>().Insert(publicacion);
                }

                await _unitOfWork.CommitAsync();

            }
            catch (ApiException)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new ApiException(ex);
            }


        }

        public async Task EditarProducto(ProductoDTO producto)
        {
            try
            {
                Producto productoBase = await _unitOfWork.GenericRepository<Producto>().GetById(producto.IdProducto);

                if (productoBase != null)
                {
                    productoBase.Nombre = producto.Nombre;
                    productoBase.Descripcion = producto.Descripcion;
                    productoBase.Precio = (float)producto.Precio;
                    productoBase.IdCategoria = producto.idCategoria;
                    productoBase.UrlImagen = producto.UrlImagen;
                    productoBase.FechaModificacion = DateTime.Now;
                }

                await _unitOfWork.GenericRepository<Producto>().Update(productoBase);


            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }


        }
    }
}