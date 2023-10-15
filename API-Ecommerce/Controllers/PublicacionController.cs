using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessLogic.DTO;
using BussinessLogic.DTO.Search;
using BussinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using AutoWrapper.Wrappers;
using DataAccess.Migrations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    public class PublicacionController : Controller
    {

        //Instancio el service que vamos a usar
        private ServicePublicacion _service;

        //Inyecto el service por el constructor
        public PublicacionController(ServicePublicacion service)
        {
            _service = service;
        }
        // GET: api/values

        //Metodo para traer todas las categorias
        [HttpGet]
        [Route("/publicaciones")]
        public async Task<ApiResponse> GetPublicaciones()
        {
            try
            {
                IList<PublicacionDTO> publicaciones = await _service.GetAllPublicaciones();
                ApiResponse response = new ApiResponse(new { data = publicaciones, cantidadPublicaciones = publicaciones.Count() });
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

        [HttpGet]
        [Route("/publicacion/{id}")]
        public async Task<ApiResponse> GetPublicacion(int id)
        {
            if (id == 0)
            {
                throw new ApiException("Debe ingresar un id de publicacion valido");
            }

            try
            {
                PublicacionDTO publicacion = await _service.GetPublicacionById(id);
                if (publicacion == null)
                {
                    throw new ApiException("No se encontro la publicacion");
                }

                ApiResponse response = new ApiResponse(new { data = publicacion });
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
        [Route("/publicacionesCarrito")]
        public async Task<ApiResponse>GetPublicacionesCarrito([FromBody] List<SearchPublicacionCarritoDTO> publicacionCarrito)
        {   
            try
            {
                if (publicacionCarrito == null || publicacionCarrito.Count == 0)
                {
                    throw new ApiException("Carrito vacío");
                }
                List<PublicacionDTO> publicaciones = (await _service.GetPublicacionesCarrito(publicacionCarrito)).ToList();

                ApiResponse response = new ApiResponse(new { data = publicaciones });

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


