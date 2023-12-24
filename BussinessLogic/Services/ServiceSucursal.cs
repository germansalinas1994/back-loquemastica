using System;
using System.Security.Cryptography.X509Certificates;
using BussinessLogic.DTO;
using DataAccess.IRepository;
using DataAccess.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using BussinessLogic.DTO.Search;
using MercadoPago.Resource.User;

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

        public async Task<bool> EliminarSucursal(int id)
        {

            Sucursal sucursal = await _unitOfWork.GenericRepository<Sucursal>().GetById(id);
            if (sucursal == null)
            {
                sucursal.FechaBaja = DateTime.Now;
                sucursal.FechaModificacion = DateTime.Now;
                Sucursal sucursalActualizada = await _unitOfWork.GenericRepository<Sucursal>().Update(sucursal);
            }

            return true;


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
    }
}