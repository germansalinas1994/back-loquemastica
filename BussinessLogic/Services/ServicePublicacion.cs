using System;
using System.Security.Cryptography.X509Certificates;
using BussinessLogic.DTO;
using DataAccess.IRepository;
using DataAccess.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using BussinessLogic.DTO.Search;
using Google.Protobuf.WellKnownTypes;
using AutoWrapper.Wrappers;
using System.Net;



namespace BussinessLogic.Services
{
    public class ServicePublicacion
    {
        //Instancio el UnitOfWork que vamos a usar
        private readonly IUnitOfWork _unitOfWork;

        private readonly ServiceSucursal _serviceSucursal;

        //Inyecto el UnitOfWork por el constructor, esto se hace para que se cree un nuevo contexto por cada vez que se llame a la clase
        public ServicePublicacion(IUnitOfWork unitOfWork, ServiceSucursal serviceSucursal)
        {
            _unitOfWork = unitOfWork;
            _serviceSucursal = serviceSucursal;
        }

        public async Task<IList<PublicacionDTO>> GetAllPublicaciones()
        {
            try
            {
                IList<Publicacion> publicaciones = await _unitOfWork.PublicacionRepository.GetAllPublicaciones();

                IList<PublicacionDTO> publicacionesDTO = publicaciones.Adapt<IList<PublicacionDTO>>();
                return publicacionesDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public async Task<PublicacionDTO> GetPublicacionById(int id)
        {
            Publicacion publicacion = await _unitOfWork.PublicacionRepository.GetPublicacionById(id);
            return publicacion.Adapt<PublicacionDTO>();

        }

        public async Task<List<PublicacionDTO>> GetPublicacionesCarrito(List<SearchPublicacionCarritoDTO> publicacionCarrito)
        {
            List<int> ids = publicacionCarrito.Select(p => p.Id).ToList();
            List<PublicacionDTO> publicaciones = (await _unitOfWork.PublicacionRepository.GetPublicacionesCarrito(ids)).Adapt<List<PublicacionDTO>>();

            foreach (var publicacion in publicaciones)
            {
                publicacion.Cantidad = publicacionCarrito.Where(p => p.Id == publicacion.IdPublicacion).FirstOrDefault().Cantidad;
            }

            return publicaciones;


        }

        public async Task<IList<PublicacionDTO>> GetPublicacionesByCategoria(int idCategoria)
        {
            List<Publicacion> publicaciones = (await _unitOfWork.GenericRepository<Publicacion>().GetByCriteria(x => x.IdProductoNavigation.IdCategoriaNavigation.IdCategoria == idCategoria)).ToList();

            return publicaciones.Adapt<List<PublicacionDTO>>();

        }

        public async Task<IList<PublicacionDTO>> GetPublicacionesSucursal(int sucursal)
        {
            try
            {
                IList<Publicacion> publicaciones = await _unitOfWork.GenericRepository<Publicacion>()
                .GetByCriteriaIncludingSpecificRelations(
                    x => x.IdSucursal == sucursal, // Tu criterio

                    query => query.Include(p => p.IdProductoNavigation) // Incluyes Producto
                                  .ThenInclude(producto => producto.IdCategoriaNavigation)
                );

                return publicaciones.Adapt<IList<PublicacionDTO>>();

            }
            catch (ApiException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public async Task<IList<PublicacionDTO>> GetPublicacionesRolSucursal(string user)
        {

            try
            {
                //ojo con esto, no es necesario hacer un metodo, se puede buscar la sucursal directamente con la consulta como en la linea 110 (No es necesario el DTO, el dto es para cuando la devolves)
                // SucursalDTO sucursal = await _serviceSucursal.GetSucursalEmail(user);

                Sucursal sucursal = (await _unitOfWork.GenericRepository<Sucursal>().GetByCriteria(x => x.EmailSucursal == user)).FirstOrDefault();

                if (sucursal == null)
                {
                    throw new ApiException("No se encontró la sucursal");
                }

                //esto esta bien pero, lo haria con suc.IdSucursal 
                IList<Publicacion> publicaciones = await _unitOfWork.GenericRepository<Publicacion>()
                    .GetByCriteriaIncludingSpecificRelations(
                        x => x.IdSucursal == sucursal.IdSucursal, // Tu criterio

                        query => query.Include(p => p.IdProductoNavigation) // Incluyes Producto
                                      .ThenInclude(producto => producto.IdCategoriaNavigation)
                    );

                return publicaciones.Adapt<IList<PublicacionDTO>>();

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

        public async Task EditarPublicacion(PublicacionDTO publicacion)
        {
            try
            {
                Publicacion publicacionBase = await _unitOfWork.GenericRepository<Publicacion>().GetById(publicacion.IdPublicacion);

                if (publicacionBase != null)
                {
                    publicacionBase.Stock = publicacion.Stock;
                    publicacionBase.FechaActualizacion = DateTime.Now;
                }

                await _unitOfWork.GenericRepository<Publicacion>().Update(publicacionBase);

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