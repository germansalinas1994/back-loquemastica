using System;
using System.Security.Cryptography.X509Certificates;
using BussinessLogic.DTO;
using DataAccess.IRepository;
using DataAccess.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using BussinessLogic.DTO.Search;
using MercadoPago.Resource.User;
using AutoWrapper.Wrappers;

namespace BussinessLogic.Services
{
    public class ServiceSucursal
    {
        //Instancio el UnitOfWork que vamos a usar
        private readonly IUnitOfWork _unitOfWork;

        //Inyecto el UnitOfWork por el constructor, esto se hace para que se cree un nuevo contexto por cada vez que se llame a la clase
        public ServiceSucursal(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public async Task<List<SucursalDTO>> GetSucursales()
        {
            try
            {
                IList<Sucursal> sucursales = (await _unitOfWork.GenericRepository<Sucursal>().GetByCriteria(x => x.FechaBaja == null)).OrderByDescending(x => x.FechaAlta).ToList();
                return sucursales.Adapt<List<SucursalDTO>>();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task EliminarSucursal(int id)
        {
            try
            {
                Sucursal sucursal = await _unitOfWork.GenericRepository<Sucursal>().GetById(id);
                if (sucursal == null)
                {
                    throw new Exception("La sucursal no existe");
                }
                sucursal.FechaBaja = DateTime.Now;
                sucursal.FechaModificacion = DateTime.Now;
                await _unitOfWork.GenericRepository<Sucursal>().Update(sucursal);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ApiException(e);
            }

        }

        public async Task<SucursalDTO> GetSucursalEmail(string user)
        {

            try
            {
                Sucursal sucursal = _unitOfWork.GenericRepository<Sucursal>().GetAll().Result.Where(s => s.EmailSucursal == user).FirstOrDefault();
                return sucursal.Adapt<SucursalDTO>();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public async Task<List<PedidoSucursalDTO>> GetPedidosSucursal(string user)
        {
            try
            {
                List<PedidoSucursalDTO> pedidosDTO = new List<PedidoSucursalDTO>();
                Sucursal sucursal = (await _unitOfWork.GenericRepository<Sucursal>().GetByCriteria(x => x.EmailSucursal == user)).FirstOrDefault();
                // Sucursal sucursal = (await _unitOfWork.GenericRepository<Sucursal>().GetByCriteriaIncludingSpecificRelations(x => x.EmailSucursal == user,
                //     query => query.Include(p => p.Pedidos).Include(pe => pe.Pedidos).ThenInclude(pupe => pupe.PublicacionPedido).ThenInclude(pu => pu.Publicacion).ThenInclude(pr => pr.IdProductoNavigation))).FirstOrDefault();
                List<Pedido> pedidos = (await _unitOfWork.GenericRepository<Pedido>().GetByCriteriaIncludingSpecificRelations(x => x.IdSucursalPedido == sucursal.IdSucursal,
                    query => query.Include(p => p.PublicacionPedido)
                    .ThenInclude(pu => pu.Publicacion)
                    .ThenInclude(pr => pr.IdProductoNavigation)
                    .Include(e => e.Envio)
                    .ThenInclude(ee => ee.EstadoEnvio)
                    .Include(u => u.Usuario)
                    )).ToList();
                if (pedidos != null && pedidos.Count > 0)
                {
                    pedidosDTO = pedidos.Select(p => new PedidoSucursalDTO
                    {
                        Id = p.Id,
                        Fecha = p.FechaAlta,
                        Orden_MercadoPago = p.Orden_MercadoPago,
                        EstadoEnvio = p.Envio != null ? p.Envio.EstadoEnvio.Descripcion : "Retira en la Sucursal",
                        EmailUsuario = p.Usuario.Email,
                        Total = p.Total,
                        DetallePedido = p.PublicacionPedido.Select(pp => new DetallePedidoDTO
                        {
                            // Aquí mapea los campos del detalle del pedido según tus necesidades
                            Id = pp.IdPublicacion,
                            Cantidad = pp.Cantidad,
                            Precio = pp.Precio,
                            NombreProducto = pp.Publicacion.IdProductoNavigation.Nombre,
                            SubTotal = pp.Cantidad * pp.Precio
                            // Agrega más campos según sea necesario
                        }).ToList()
                    }).ToList();


                }
                return pedidosDTO.OrderByDescending(p => p.Fecha).ToList();


            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<SucursalDTO> GetSucursalById(int id)
        {

            try
            {
                Sucursal sucursal = await _unitOfWork.GenericRepository<Sucursal>().GetById(id);
                if (sucursal == null)
                {
                    throw new Exception("La sucursal no existe");
                }
                return sucursal.Adapt<SucursalDTO>();
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

        public async Task CargarSucursal(SucursalDTO sucursal)
        {
            try
            {

                Sucursal sucursalExistente = _unitOfWork.GenericRepository<Sucursal>().GetAll().Result.Where(s => s.EmailSucursal == sucursal.EmailSucursal).FirstOrDefault();
                if (sucursalExistente != null)
                {
                    throw new Exception("El mail de la sucursal ya existe");
                }

                await _unitOfWork.BeginTransactionAsync();


                Sucursal sucursalBase = sucursal.Adapt<Sucursal>();
                sucursalBase.EmailSucursal = sucursal.EmailSucursal;
                sucursalBase.Nombre = sucursal.Nombre;
                sucursalBase.Direccion = sucursal.Direccion;
                sucursalBase.FechaAlta = DateTime.Now;
                sucursalBase.FechaModificacion = DateTime.Now;
                await _unitOfWork.GenericRepository<Sucursal>().Insert(sucursalBase);

                await _unitOfWork.CommitAsync();
            }
            catch (ApiException)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackAsync();
                throw new ApiException(e);
            }
        }

        public async Task EditarSucursal(SucursalDTO sucursal)
        {
            try
            {
                Sucursal sucursalExistente = await _unitOfWork.GenericRepository<Sucursal>().GetById(sucursal.IdSucursal);
                if (sucursalExistente == null)
                {
                    throw new Exception("La sucursal no existe");
                }
                sucursalExistente.Nombre = sucursal.Nombre;
                sucursalExistente.Direccion = sucursal.Direccion;
                sucursalExistente.EmailSucursal = sucursal.EmailSucursal;
                sucursalExistente.FechaModificacion = DateTime.Now;
                await _unitOfWork.GenericRepository<Sucursal>().Update(sucursalExistente);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ApiException(e);
            }
        }
    }
}