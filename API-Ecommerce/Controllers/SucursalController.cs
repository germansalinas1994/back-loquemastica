using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessLogic.DTO;
using BussinessLogic.DTO.Search;
using BussinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using AutoWrapper.Wrappers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    public class SucursalController : GenericController
    {

        //Instancio el service que vamos a usar
        private ServiceSucursal _serviceSucursal;

        //Inyecto el service por el constructor
        public SucursalController(ServiceSucursal serviceSucursal)
        {
            _serviceSucursal = serviceSucursal;
        }


        [HttpGet]
        [Route("/sucursales")]
        public async Task<ApiResponse> GetSucursales()
        {
            try
            {
                List<SucursalDTO> sucursales = await _serviceSucursal.GetSucursales();
                return new ApiResponse(sucursales);
            }
            catch(ApiException)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw new ApiException(ex);
            }
          
          
        }

        [HttpPut]
        [Route("/sucursales/{id}")]

        public async Task<ApiResponse> EliminarSucursal(int id)
        {
            await _serviceSucursal.EliminarSucursal(id);
            ApiResponse response = new ApiResponse("la sucursal se elimino correctamente");
            return response;
            
        }











    }
}


