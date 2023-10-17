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





    }
}


