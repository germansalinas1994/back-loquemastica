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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    public class MercadoPagoController : Controller
    {

        //Instancio el service que vamos a usar
        private ServiceMercadoPago _service;
        private ServicePublicacion _servicePublicacion;

        //Inyecto el service por el constructor
        public MercadoPagoController(ServiceMercadoPago service, ServicePublicacion servicePublicacion)
        {
            _service = service;
            _servicePublicacion = servicePublicacion;
        }



        [HttpPost]
        [Route("/publicacionesCarritoMP")]
        public async Task<ApiResponse> GetPreferenceMP([FromBody] List<SearchPublicacionCarritoDTO> publicacionCarrito)
        {
            try
            {
                if (publicacionCarrito == null || publicacionCarrito.Count == 0)
                {
                    return new ApiResponse("Carrito vacío");
                }
                List<PublicacionDTO> publicaciones = (await _servicePublicacion.GetPublicacionesCarrito(publicacionCarrito)).ToList();

                string prefenceId = await _service.GetPreferenceMP(publicaciones);

                ApiResponse response = new ApiResponse(new { data = prefenceId });

                return response;
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                throw new ApiException(ex);
            }

        }


        [HttpPost]
        [Route("/webhook")]
        public async Task<IActionResult> WebHookMP([FromBody] object payment)
        {
            try
            {
                if (payment == null)
                {
                    Console.WriteLine("Notificación de webhook inválida.");
                    return BadRequest();
                }

                // Confirmar recepción inmediatamente

                string jsonString = JsonSerializer.Serialize(payment);
                WebhookPagoMercadoPago ordenDeCompra = JsonSerializer.Deserialize<WebhookPagoMercadoPago>(jsonString);

                if (ordenDeCompra.type == "payment")
                {
                    string response = await _service.GetPaymentInfo(ordenDeCompra.data.id);


                }







                return Ok();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // Log the exception
                return BadRequest();
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