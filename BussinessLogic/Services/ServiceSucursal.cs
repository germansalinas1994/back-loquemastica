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
                IList<Sucursal> sucursales = await _unitOfWork.GenericRepository<Sucursal>().GetAll();
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

        public async Task<List<PedidoDTO>> GetPedidosSucursal(string user)
        {
            try
            {
                Sucursal sucursal = (await _unitOfWork.GenericRepository<Sucursal>().GetByCriteriaIncludingSpecificRelations(x => x.EmailSucursal == user,
                  query => query.Include(p => p.Pedidos).ThenInclude(pedido => pedido.Envio)
                )).FirstOrDefault();
                List<Pedido> pedidos = sucursal.Pedidos.ToList();
                return pedidos.Adapt<List<PedidoDTO>>();
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
    }
}