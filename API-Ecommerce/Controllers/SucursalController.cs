using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessLogic.DTO;
using BussinessLogic.DTO.Search;
using BussinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }


        }

        [HttpPut]
        [Route("/sucursales/{id}")]
        [Authorize(Policy = "Admin")]

        public async Task<ApiResponse> EliminarSucursal(int id)
        {
            try
            {
                await _serviceSucursal.EliminarSucursal(id);
                return new ApiResponse("la sucursal se elimino correctamente");
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

        [HttpGet]
        [Route("/getPedidosSucursal")]
        public async Task<ApiResponse> GetPedidosSucursal()
        {
            try
            {
                string user = UserEmailFromJWT();
                List<PedidoDTO> pedidos = await _serviceSucursal.GetPedidosSucursal(user);
                return new ApiResponse(pedidos);
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


        [HttpGet]
        [Route("/sucursal/{id}")]
        [Authorize(Policy = "Admin")]

        public async Task<ApiResponse> GetSucursalById(int id)
        {
            try
            {
                SucursalDTO sucursal = await _serviceSucursal.GetSucursalById(id);
                return new ApiResponse(new { data = sucursal });
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

        [HttpPost]
        [Route("/sucursal")]
        [Authorize(Policy = "Admin")]
        public async Task<ApiResponse> CargarSucursal([FromBody] SucursalDTO sucursal)
        {
            try
            {
                await _serviceSucursal.CargarSucursal(sucursal);
                return new ApiResponse("La sucursal se cargó exitosamente");

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

        // [HttpPut]
        // [Route("/producto")]
        // [Authorize(Policy = "Admin")]
        // public async Task<ApiResponse> EditarProducto([FromForm] ProductoDTO producto)
        // {
        //     try
        //     {
        //         await _service.EditarProducto(producto);
        //         return new ApiResponse("El producto se modificó exitosamente");

        //     }
        //     catch (ApiException)
        //     {
        //         throw;
        //     }
        //     catch (Exception e)
        //     {
        //         throw new ApiException(e);
        //     }
        // }


    }
}


