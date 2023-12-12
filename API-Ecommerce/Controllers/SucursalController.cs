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
    public class SucursalController : Controller
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
                return new ApiResponse(sucursales, 200);
            }
            catch (Exception e)
            {
                return new ApiResponse(e.Message, 400);
            }
        }






    }
}


