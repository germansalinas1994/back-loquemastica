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
using System.Net;

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
        [Authorize(Policy = "Sucursal")]
        public async Task<ApiResponse> GetPedidosSucursal()
        {
            try
            {
                string user = UserEmailFromJWT();
                List<PedidoSucursalDTO> pedidos = await _serviceSucursal.GetPedidosSucursal(user);
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

        [HttpPut]
        [Route("/CambiarEstadoEnvio")]
        [Authorize(Policy = "Sucursal")]

        public async Task<ApiResponse> CambiarEstadoEnvio([FromBody] ChangeEstadoEnvio estadoEnvio)
        {
            try
            {
                await _serviceSucursal.CambiarEstadoEnvio(estadoEnvio.IdPedido, estadoEnvio.IdEstadoEnvio);
                return new ApiResponse("El estado del pedido se modificó exitosamente");

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

        [HttpPut]
        [Route("/sucursal")]
        [Authorize(Policy = "Admin")]

        public async Task<ApiResponse> EditarSucursal([FromBody] SucursalDTO sucursal)
        {
            try
            {
                await _serviceSucursal.EditarSucursal(sucursal);
                return new ApiResponse("La sucursal se modificó exitosamente");

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
        [Route("/filtrarPedidosSucursal")]
        [Authorize(Policy = "Sucursal")]
        public async Task<ApiResponse> FiltrarPedidosSucursal([FromBody] SearchPedidoSucursalDTO search)
        {
            try
            {
                string user = UserEmailFromJWT();
                List<PedidoSucursalDTO> pedidos = await _serviceSucursal.FiltrarPedidosSucursal(search.mes, search.anio, search.estado, user);
                return new ApiResponse(pedidos, (int)HttpStatusCode.OK);
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

        [HttpPost]
        [Route("/generarReportePedidosSucursal")]
        [Authorize(Policy = "Sucursal")]

        public async Task<ApiResponse> GenerarReportePedidosSucursal([FromBody] SearchPedidoSucursalDTO search)
        {
            try
            {
                string user = UserEmailFromJWT();

                byte[] pdfBytes = await _serviceSucursal.GenerarReportePedidosSucursal(search.mes, search.anio, user);

                if (pdfBytes == null)
                {
                    throw new ApiException("No se pudo generar el reporte", (int)HttpStatusCode.BadRequest, "ERRORREPORTE");
                }

                // Convertir el PDF a Base64
                string pdfBase64 = Convert.ToBase64String(pdfBytes);

                //necesito que el nombre del archivo sea con la fecha y hora que llega por parametro

                string nombreArchivo = $"ReportePedidos_{search.mes.ToString("D2")}_{search.anio}.pdf";

                // Devolver la cadena Base64 como parte de la respuesta
                return new ApiResponse(new { pdf = pdfBase64, nombre = nombreArchivo }, (int)HttpStatusCode.OK);
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
    public class ChangeEstadoEnvio
    {
        public int IdPedido { get; set; }
        public int IdEstadoEnvio { get; set; }
    }

    public class SearchPedidoSucursalDTO
    {
        public int mes { get; set; }
        public int anio { get; set; }
        public int estado { get; set; }
    }

}


