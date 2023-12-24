using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessLogic.DTO;
using BussinessLogic.DTO.Search;
using BussinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using AutoWrapper.Wrappers;
using System.Text.Json;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Google.Protobuf.WellKnownTypes;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    public class MercadoPagoController : GenericController
    {

        //Instancio el service que vamos a usar
        private ServiceMercadoPago _service;
        private ServicePublicacion _servicePublicacion;

        private ServiceUsuario _serviceUsuario;


        //Inyecto el service por el constructor
        public MercadoPagoController(ServiceMercadoPago service, ServicePublicacion servicePublicacion, ServiceUsuario serviceUsuario)
        {
            _service = service;
            _servicePublicacion = servicePublicacion;
            _serviceUsuario = serviceUsuario;
        }


        [HttpPost]
        [Authorize(Policy = "Cliente")]
        [Route("/publicacionesCarritoMP")]
        // public async Task<ApiResponse> GetPreferenceMP([FromBody] List<SearchPublicacionCarritoDTO> publicacionCarrito)
        public async Task<ApiResponse> GetPreferenceMP([FromBody] PreferenceMercadoPagoDTO preferencePago)
        {

            try
            {
                //metodo que obtiene el email del usuario desde el token
                string user = UserEmailFromJWT();


                if (preferencePago.Publicaciones == null || preferencePago.Publicaciones.Count == 0)
                {
                    throw new ApiException("No se encontraron publicaciones en el carrito");
                }

                //verifico que el usuario sea el mismo que tengo en la base 
                UsuarioDTO usuario = await _serviceUsuario.GetUsuario(user);

                if (usuario == null)
                {
                    throw new ApiException("El usuario no existe");
                }

                List<PublicacionDTO> publicaciones = (await _servicePublicacion.GetPublicacionesCarrito(preferencePago.Publicaciones)).ToList();

                // string prefenceId = await _service.GetPreferenceMP(publicaciones, usuario.IdUsuario, preferencePago.IdDomicilio);
                string prefenceId = await _service.GetPreferenceMP(preferencePago, user);

                ApiResponse response = new ApiResponse(new { data = prefenceId });

                return response;
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
        [Route("/webhook")]
        public async Task<ApiResponse> WebHookMP([FromBody] object payment)
        {
            try
            {
                if (payment == null)
                {
                    throw new ApiException("No se recibió el pago");
                }

                // Confirmar recepción inmediatamente

                string jsonString = JsonSerializer.Serialize(payment);
                WebhookPagoMercadoPago ordenDeCompra = JsonSerializer.Deserialize<WebhookPagoMercadoPago>(jsonString);

                if (ordenDeCompra.type == "payment")
                {
                    await _service.CrearPedido(ordenDeCompra.data.id);


                }

                return new ApiResponse((int)HttpStatusCode.OK);

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
        [Route("/getOrderMercadoPago")]
        [Authorize(Policy = "Cliente")]
        public async Task<ApiResponse> GetOrderMercadoPago([FromQuery] string merchantOrderId, string paymentId)
        {
            try
            {

                if (merchantOrderId == null || paymentId == null)
                {
                    throw new ApiException("No se recibió el pago");
                }

                PedidoDTO pago = await _service.GetOrderMercadoPago(merchantOrderId, paymentId);

                return new ApiResponse(pago);


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



public class WebhookMercadoPagoOrder
{
    public string resource { get; set; }
    public string topic { get; set; }
}


public class WebhookPagoMercadoPago
{
    public string action { get; set; }
    public string api_version { get; set; }
    public DataPago data { get; set; }
    public DateTime date_created { get; set; }
    public long id { get; set; }
    public bool live_mode { get; set; }
    public string type { get; set; }
    public string user_id { get; set; }
}

public class DataPago
{
    public string id { get; set; }
}




